using System;
using System.Collections.Generic;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Psionics.Utils;

namespace Psionics.HarmonyPatches;

/// <summary>
/// Psionic Proficiency (Ex): A psychic warrior treats his base attack bonus as equal to his psychic
/// warrior level for the purposes of requirements for psionic feats. Base attack bonuses granted from
/// other classes are unaffected and are added normally.
///
/// Implementation: postfix patch on <see cref="PrerequisiteStatValue.CheckInternal"/>. When a BAB
/// prerequisite is being evaluated for a tagged psionic feat and the unit has Psychic Warrior class
/// levels, the prerequisite is treated as met if PW level >= required value. This produces
/// max(real BAB, PW level) semantics: the real BAB check runs first; if it fails, we substitute.
/// </summary>
[HarmonyPatch(typeof(PrerequisiteStatValue), "CheckInternal")]
public static class PsionicProficiencyPatch
{
    private static readonly HashSet<string> PsionicFeatGuids = new(StringComparer.OrdinalIgnoreCase)
    {
        Guids.PsionicMeditationFeat,
        Guids.PsionicWeaponFeat,
    };

    private static BlueprintCharacterClass _pwClass;

    public static void RegisterPsionicFeat(string guid)
    {
        if (!string.IsNullOrEmpty(guid))
        {
            PsionicFeatGuids.Add(guid);
        }
    }

    [HarmonyPostfix]
    public static void Postfix(
        PrerequisiteStatValue __instance,
        UnitDescriptor unit,
        ref bool __result)
    {
        if (__result) return;
        if (__instance.Stat != StatType.BaseAttackBonus) return;
        if (unit == null) return;

        var owner = __instance.OwnerBlueprint;
        if (owner == null) return;
        if (!PsionicFeatGuids.Contains(owner.AssetGuid.ToString())) return;

        _pwClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.PsychicWarriorClass);
        if (_pwClass == null) return;

        var pwLevel = unit.Progression.GetClassLevel(_pwClass);
        if (pwLevel <= 0) return;

        if (pwLevel >= __instance.Value)
        {
            __result = true;
        }
    }
}
