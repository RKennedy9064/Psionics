using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Controllers.Combat;
using Kingmaker.EntitySystem.Entities;
using Psionics.Utils;

namespace Psionics.HarmonyPatches;

/// <summary>
/// Suppresses movement AoOs for units that have the UpTheWalls buff (i.e., focused + have the feat).
/// UnitCondition.ImmuneToAttackOfOpportunity does not cover movement-triggered AoOs in WotR.
/// </summary>
[HarmonyPatch(typeof(UnitCombatEngagementController), "ProvokeAttackOfOpportunity")]
public static class UpTheWallsPatch
{
    private static BlueprintBuff _buff;

    [HarmonyPrefix]
    public static bool Prefix(UnitEntityData unit)
    {
        if (unit == null) return true;

        _buff ??= BlueprintTool.Get<BlueprintBuff>(Guids.UpTheWallsBuff);
        if (_buff == null)
        {
            UnityEngine.Debug.LogWarning("[UpTheWallsPatch] buff blueprint is null");
            return true;
        }

        bool hasBuff = unit.Buffs.HasFact(_buff);
        UnityEngine.Debug.Log($"[UpTheWallsPatch] unit={unit.CharacterName} hasBuff={hasBuff} → {(hasBuff ? "SUPPRESSED" : "allowed")}");
        return !hasBuff;
    }
}
