using System;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.HarmonyPatches;

/// <summary>
/// Enhanced Mind Blade pool: the ability toggles use the game's <see cref="ActivatableAbilityResourceLogic"/>
/// against a custom pool resource, which gives the shaman-style "remaining points on the icon" display and
/// auto-darkening for free. Two gaps in the stock logic are patched here:
///
/// 1. <c>CalcResourceCost</c> spends a flat 1 per toggle (it has no weight field). We override it to charge
///    the ability's real +N cost (1/2/4) so a +2 burst reserves 2 points, etc.
/// 2. <c>IsAvailable</c> only requires ≥1 point, so a +2 toggle could be switched on with a single point left
///    (and then under-spend / not spend). We require the full cost so weighted toggles darken correctly.
///
/// Both are scoped to our toggles via <see cref="MindBladeEnchantments.AbilityToggleCosts"/>; every other
/// activatable ability is untouched.
///
/// IMPORTANT: <c>IsAvailable</c> is polled by the action-bar UI when no component runtime context is active,
/// so the context-bound <c>__instance.Fact</c>/<c>Owner</c> accessors throw "ComponentRuntime is unavailable"
/// there — a throwing postfix makes the game treat the toggle as permanently unavailable. We therefore read
/// identity/owner from the <c>EntityFactComponent runtime</c> argument (direct references, context-free) and
/// wrap everything in try/catch so the patch can never disable an ability.
/// </summary>
[HarmonyPatch(typeof(ActivatableAbilityResourceLogic))]
public static class MindBladeEnchantPoolPatch
{
    private static BlueprintAbilityResource _pool;
    private static BlueprintAbilityResource Pool =>
        _pool ??= BlueprintTool.Get<BlueprintAbilityResource>(Guids.EnhancedMindBladePoolResource);

    private static bool TryGetCost(EntityFact fact, out int cost)
    {
        cost = 0;
        var guid = fact?.Blueprint?.AssetGuid.ToString();
        return guid != null && MindBladeEnchantments.AbilityToggleCosts.TryGetValue(guid, out cost);
    }

    // Charge the ability's real +N cost. CalcResourceCost runs inside the spend (context is valid), but we
    // still guard it — a throw here would abort the spend.
    [HarmonyPatch("CalcResourceCost")]
    [HarmonyPostfix]
    public static void CalcResourceCost_Postfix(ActivatableAbilityResourceLogic __instance, ref int __result)
    {
        try
        {
            if (TryGetCost(__instance.Fact, out var cost))
                __result = cost;
        }
        catch { /* leave stock cost */ }
    }

    // Require the full +N cost for weighted (+2/+4) toggles so they darken correctly. Uses the runtime
    // argument for context-free identity/owner; never throws.
    [HarmonyPatch("IsAvailable", new Type[] { typeof(EntityFactComponent) })]
    [HarmonyPostfix]
    public static void IsAvailable_Postfix(EntityFactComponent runtime, ref bool __result)
    {
        if (!__result) return;
        try
        {
            var fact = runtime?.Fact;
            if (!TryGetCost(fact, out var cost) || cost <= 1) return;

            // Already on? Its cost is already reserved — leave available so it can be switched off.
            if (fact is ActivatableAbility aa && aa.IsOn) return;

            if (runtime.Owner is not UnitEntityData owner) return;
            var pool = Pool;
            if (pool == null) return;

            if (owner.Resources.GetResourceAmount(pool) < cost)
                __result = false;
        }
        catch { /* never disable the ability on error */ }
    }
}
