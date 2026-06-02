using System;
using System.Security.Cryptography;
using System.Text;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features;

/// <summary>
/// Enhanced Mind Blade: a level-scaled enhancement pool the soulknife allocates between a direct
/// enhancement bonus (an "eat-all" toggle, capped at the level's max direct bonus) and weapon special
/// abilities (one toggle each, cost = the ability's +N equivalent). Modeled on the Spirit Hunter's
/// Spirit Enhancement; budget + application handled by <see cref="MindBladeEnchantments"/>.
/// </summary>
public static class EnhancedMindBlade
{
    public static void Configure()
    {
        MindBladeEnchantments.ManagedEnchantments.Clear();

        // Enhancement enchantments are always managed (added/removed by Recompute).
        foreach (var e in new[]
        {
            WeaponEnchantmentRefs.Enhancement1, WeaponEnchantmentRefs.Enhancement2, WeaponEnchantmentRefs.Enhancement3,
            WeaponEnchantmentRefs.Enhancement4, WeaponEnchantmentRefs.Enhancement5,
        })
            MindBladeEnchantments.ManagedEnchantments.Add(
                BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(e.Reference.Guid.ToString()));

        var enhIcon = AbilityRefs.MagicWeaponGreater.Reference.Get().Icon;
        MindBladeEnchantments.AbilityToggleCosts.Clear();

        // The enhancement pool resource. Its max scales with soulknife level (= MindBladeEnchantments.GetPool):
        // 1 at level 3, +1 every 2 levels (matching min(9,(lvl-1)/2)). Each ability toggle draws from it; the
        // game shows the remaining amount on the toggle icons and darkens toggles that can't be afforded.
        AbilityResourceConfigurator.New("SKEnhMindBladePool", Guids.EnhancedMindBladePoolResource)
            .SetLocalizedName(Loc.Str("SK.EMB.Pool.Name", "Mind Blade Enhancement Pool", tagEncyclopediaEntries: false))
            .SetLocalizedDescription(Loc.Str("SK.EMB.Pool.Desc",
                "Points the soulknife allocates among her mind blade's weapon special abilities.", tagEncyclopediaEntries: false))
            .SetIcon(enhIcon)
            .SetMaxAmount(
                ResourceAmountBuilder.New(0)
                    .IncreaseByLevelStartPlusDivStep(
                        classes: [Guids.SoulKnifeClass],
                        startingLevel: 3,
                        startingBonus: 1,
                        levelsPerStep: 2,
                        bonusPerStep: 1))
            .Configure();

        // The eat-all enhancement toggle.
        var enhBuff = BuffConfigurator.New("SKEnhMindBladeEnhanceBuff", Guids.EnhancedMindBladeEnhanceBuff)
            .SetDisplayName(Loc.Str("SK.EMB.Enhance.Name", "Enhance Mind Blade"))
            .SetDescription(Loc.Str("SK.EMB.Enhance.Desc",
                "Spend the remainder of your enhancement pool (up to your maximum direct bonus) on a numeric enhancement bonus."))
            .SetIcon(enhIcon)
            .AddComponent(new MindBladeEnchantComponent { IsEnhancement = true })
            .Configure();

        ActivatableAbilityConfigurator.New("SKEnhMindBladeEnhanceToggle", Guids.EnhancedMindBladeEnhanceToggle)
            .SetDisplayName(Loc.Str("SK.EMB.Enhance.Name", "Enhance Mind Blade"))
            .SetDescription(Loc.Str("SK.EMB.Enhance.Desc",
                "Spend the remainder of your enhancement pool (up to your maximum direct bonus) on a numeric enhancement bonus."))
            .SetIcon(enhIcon)
            .SetBuff(enhBuff)
            .SetActivationType(AbilityActivationType.Immediately)
            .SetGroup(ActivatableAbilityGroup.None)
            .SetIsOnByDefault(true)
            // Display-only resource logic: shows the remaining pool on the icon (the action bar only draws a
            // count for abilities that have resource logic) but never spends — the enhance toggle consumes
            // whatever the ability toggles leave behind, computed in MindBladeEnchantments.Recompute.
            .AddActivatableAbilityResourceLogic(
                requiredResource: Guids.EnhancedMindBladePoolResource,
                spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
            .Configure();

        // Ability table (cost = +N equivalent, ReqLevel = soulknife level the ability unlocks).
        // Only standard WotR enchantments with clean generic refs.
        var abilities = new (string Key, string Name, int Cost, int ReqLevel, string Ench, UnityEngine.Sprite Icon)[]
        {
            ("Flaming",        "Flaming",         1, 5,  WeaponEnchantmentRefs.Flaming.Reference.Guid.ToString(),        AbilityRefs.BurningHands.Reference.Get().Icon),
            ("Frost",          "Frost",           1, 5,  WeaponEnchantmentRefs.Frost.Reference.Guid.ToString(),          AbilityRefs.RayOfFrost.Reference.Get().Icon),
            ("Shock",          "Shock",           1, 5,  WeaponEnchantmentRefs.Shock.Reference.Guid.ToString(),          AbilityRefs.CallLightning.Reference.Get().Icon),
            ("Corrosive",      "Corrosive",       1, 5,  WeaponEnchantmentRefs.Corrosive.Reference.Guid.ToString(),      AbilityRefs.AcidArrow.Reference.Get().Icon),
            ("Keen",           "Keen",            1, 5,  WeaponEnchantmentRefs.Keen.Reference.Guid.ToString(),           AbilityRefs.KeenEdge.Reference.Get().Icon),
            ("GhostTouch",     "Ghost Touch",     1, 5,  WeaponEnchantmentRefs.GhostTouch.Reference.Guid.ToString(),     AbilityRefs.Blur.Reference.Get().Icon),
            ("Vicious",        "Vicious",         1, 5,  WeaponEnchantmentRefs.ViciousEnchantment.Reference.Guid.ToString(), AbilityRefs.Enervation.Reference.Get().Icon),
            ("Agile",          "Agile",           1, 5,  WeaponEnchantmentRefs.Agile.Reference.Guid.ToString(),          FeatureRefs.WeaponFinesse.Reference.Get().Icon),
            ("FlamingBurst",   "Flaming Burst",   2, 7,  WeaponEnchantmentRefs.FlamingBurst.Reference.Guid.ToString(),   AbilityRefs.BurningHands.Reference.Get().Icon),
            ("IcyBurst",       "Icy Burst",       2, 7,  WeaponEnchantmentRefs.IcyBurst.Reference.Guid.ToString(),       AbilityRefs.RayOfFrost.Reference.Get().Icon),
            ("ShockingBurst",  "Shocking Burst",  2, 7,  WeaponEnchantmentRefs.ShockingBurst.Reference.Guid.ToString(),  AbilityRefs.CallLightning.Reference.Get().Icon),
            ("CorrosiveBurst", "Corrosive Burst", 2, 7,  WeaponEnchantmentRefs.CorrosiveBurst.Reference.Guid.ToString(), AbilityRefs.AcidArrow.Reference.Get().Icon),
            ("Holy",           "Holy",            2, 7,  WeaponEnchantmentRefs.Holy.Reference.Guid.ToString(),           AbilityRefs.BlessWeapon.Reference.Get().Icon),
            ("Unholy",         "Unholy",          2, 7,  WeaponEnchantmentRefs.Unholy.Reference.Guid.ToString(),         AbilityRefs.Enervation.Reference.Get().Icon),
            ("Anarchic",       "Anarchic",        2, 7,  WeaponEnchantmentRefs.Anarchic.Reference.Guid.ToString(),       AbilityRefs.AlignWeapon.Reference.Get().Icon),
            ("Axiomatic",      "Axiomatic",       2, 7,  WeaponEnchantmentRefs.Axiomatic.Reference.Guid.ToString(),      AbilityRefs.AlignWeapon.Reference.Get().Icon),
            ("BrilliantEnergy","Brilliant Energy",4, 12, WeaponEnchantmentRefs.BrilliantEnergy.Reference.Guid.ToString(), AbilityRefs.MagicWeapon.Reference.Get().Icon),
        };

        // Group each ability's toggle GUID by the level at which it unlocks.
        var tierToggles = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<string>>();
        foreach (var a in abilities)
        {
            var enchRef = BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(a.Ench);
            MindBladeEnchantments.ManagedEnchantments.Add(enchRef);

            var buff = BuffConfigurator.New($"SKEMBBuff{a.Key}", Det($"SK.EMB.Buff.{a.Key}"))
                .SetDisplayName(Loc.Str($"SK.EMB.{a.Key}.Name", a.Name))
                .SetDescription(Loc.Str($"SK.EMB.{a.Key}.Desc",
                    $"Your mind blade gains the {a.Name} weapon special ability (consumes {a.Cost} point{(a.Cost > 1 ? "s" : "")} from your enhancement pool)."))
                .SetIcon(a.Icon)
                .AddComponent(new MindBladeEnchantComponent { Cost = a.Cost, Enchantment = enchRef })
                .Configure();

            var toggleGuid = Det($"SK.EMB.Toggle.{a.Key}");
            ActivatableAbilityConfigurator.New($"SKEMBToggle{a.Key}", toggleGuid)
                .SetDisplayName(Loc.Str($"SK.EMB.{a.Key}.Name", a.Name))
                .SetDescription(Loc.Str($"SK.EMB.{a.Key}.ToggleDesc",
                    $"Toggle. Your mind blade gains the {a.Name} weapon special ability (consumes {a.Cost} point{(a.Cost > 1 ? "s" : "")} from your enhancement pool)."))
                .SetIcon(a.Icon)
                .SetBuff(buff)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetGroup(ActivatableAbilityGroup.None)
                .AddActivatableAbilityResourceLogic(
                    requiredResource: Guids.EnhancedMindBladePoolResource,
                    spendType: ActivatableAbilityResourceLogic.ResourceSpendType.TurnOn)
                .Configure();

            // The resource logic above shows the remaining pool on the icon and darkens toggles when
            // the pool hits 0; the cost map lets the Harmony patch charge >1 for +2/+4 abilities and
            // darken them when fewer than their cost remain (see MindBladeEnchantPoolPatch).
            MindBladeEnchantments.AbilityToggleCosts[toggleGuid] = a.Cost;

            if (!tierToggles.TryGetValue(a.ReqLevel, out var list))
                tierToggles[a.ReqLevel] = list = new System.Collections.Generic.List<string>();
            list.Add(toggleGuid);
        }

        // Base feature (granted at level 3): the enhancement eat-all toggle only.
        FeatureConfigurator.New("SKEnhancedMindBlade", Guids.EnhancedMindBladeFeature)
            .SetDisplayName(Loc.Str("SK.EMB.Name", "Enhanced Mind Blade"))
            .SetDescription(Loc.Str("SK.EMB.Desc",
                "At 3rd level, a soulknife's mind blade gains an enhancement pool that grows as she levels. " +
                "She allocates it between a direct enhancement bonus (the Enhance Mind Blade toggle, up to her " +
                "maximum direct bonus) and weapon special abilities (one toggle each), which become available as " +
                "she gains levels: +1 abilities at 5th level, +2 abilities at 7th, and Brilliant Energy at 12th. " +
                "Toggle special abilities first, then enable the enhancement toggle to spend the remaining points."))
            .SetIcon(enhIcon)
            .SetIsClassFeature()
            .AddAbilityResources(resource: Guids.EnhancedMindBladePoolResource, restoreAmount: true)
            .AddFacts([Guids.EnhancedMindBladeEnhanceToggle])
            .Configure();

        // Tier features (granted at the unlock level): the abilities that become available then.
        ConfigureTier(Guids.EnhancedMindBladeAbilities5,  5,  tierToggles, enhIcon,
            "Mind Blade Special Abilities (+1)",
            "At 5th level, the soulknife may allocate her enhancement pool toward +1 weapon special abilities: " +
            "Flaming, Frost, Shock, Corrosive, Keen, Ghost Touch, Vicious, and Agile.");
        ConfigureTier(Guids.EnhancedMindBladeAbilities7,  7,  tierToggles, enhIcon,
            "Mind Blade Special Abilities (+2)",
            "At 7th level, the soulknife may allocate her enhancement pool toward +2 weapon special abilities: " +
            "Flaming Burst, Icy Burst, Shocking Burst, Corrosive Burst, Holy, Unholy, Anarchic, and Axiomatic.");
        ConfigureTier(Guids.EnhancedMindBladeAbilities12, 12, tierToggles, enhIcon,
            "Mind Blade Special Abilities (+4)",
            "At 12th level, the soulknife may allocate her enhancement pool toward the Brilliant Energy weapon " +
            "special ability.");
    }

    private static void ConfigureTier(
        string featureGuid, int level,
        System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<string>> tierToggles,
        UnityEngine.Sprite icon, string name, string desc)
    {
        if (!tierToggles.TryGetValue(level, out var toggles) || toggles.Count == 0) return;

        FeatureConfigurator.New($"SKEnhancedMindBladeAbilities{level}", featureGuid)
            .SetDisplayName(Loc.Str($"SK.EMB.Tier{level}.Name", name))
            .SetDescription(Loc.Str($"SK.EMB.Tier{level}.Desc", desc))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFacts([.. toggles])
            .Configure();
    }

    private static string Det(string key)
    {
        using var md5 = MD5.Create();
        return new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(key))).ToString();
    }
}
