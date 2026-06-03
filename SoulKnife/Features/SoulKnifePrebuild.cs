using System;
using System.Security.Cryptography;
using System.Text;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features;

/// <summary>
/// The Soulknife's recommended ("premade") build. Registering this and pointing the class at it via
/// SetDefaultBuild is what makes the class-selection screen show the "Premade Build Balance" radar chart
/// and the "Use Recommended Build" button (mirrors PrebuildPsychicWarriorFeatureList).
///
/// This encodes the two-handed damage build from InternalDocuments/SOULKNIFE_BUILDS.md (minus the
/// Psionic Training pick, which cascades into a nested selection the prebuild can't express cleanly).
/// </summary>
public static class SoulKnifePrebuild
{
    // FighterFeatSelection feat, available in the L1 Soulknife bonus-feat selection.
    private const string PowerAttack = "9972f33f977fc724c838e59641b2fca5";

    public static void Configure()
    {
        FeatureConfigurator.New("SoulKnifePrebuild", Guids.SoulKnifePrebuild)
            .SetDisplayName(Loc.Str("SK.Prebuild.Name", "Soulknife"))
            .SetDescription(Loc.Str("SK.Prebuild.Desc",
                "A two-handed mind blade striker. Channels psionic focus for Focused Offense (Wisdom to "
                + "attack and damage), widens and multiplies critical hits (Keen + Deadly Blow), and layers "
                + "Psychic Strike, energy blades, and Exploding Critical for heavy sustained melee damage."))
            .AddClassLevels(
                characterClass: Guids.SoulKnifeClass,
                levels: 20,
                raceStat: StatType.Strength,
                levelsStat: StatType.Strength,
                skills:
                [
                    StatType.SkillAthletics,
                    StatType.SkillPersuasion,
                    StatType.SkillPerception,
                    StatType.SkillMobility,
                ],
                selections:
                [
                    // L1 bonus feat — Power Attack
                    Sel(Guids.SoulKnifeBonusFeat, PowerAttack),
                    // L1 Form Mind Blade — manifest as a greatsword (two-handed, 2d6/19-20/×2)
                    Sel(Guids.MindBladeFeature, DeterministicGuid("SK.MB.Weapon.Greatsword")),
                    // Blade skills at every even level (L2 → L20), in prerequisite-safe order
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillFocusedOffense),    // L2
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillTelekineticEdge),   // L4
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillPowerfulStrikes),   // L6
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillFireBlade),         // L8
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillDeadlyBlow),        // L10
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillExplodingCritical), // L12
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillFirestorm),         // L14
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillImprovedEnhancement), // L16
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillIceBlade),          // L18
                    Sel(Guids.BladeSkillsSelection, Guids.BladeSkillVampiricBlade),     // L20
                ])
            .AddStatsDistributionPreset(
                targetPoints: 20,
                strength: 16, dexterity: 12, constitution: 14,
                intelligence: 10, wisdom: 14, charisma: 8)
            .AddStatsDistributionPreset(
                targetPoints: 25,
                strength: 16, dexterity: 13, constitution: 14,
                intelligence: 12, wisdom: 14, charisma: 10)
            .AddBuildBalanceRadarChart(
                melee: 5, ranged: 1, magic: 1, defense: 2, support: 1, control: 1)
            .OnConfigure(_ => UnityEngine.Debug.Log("[SK] SoulKnifePrebuild configured OK"))
            .Configure();
    }

    private static SelectionEntry Sel(string selectionGuid, string featureGuid) =>
        new()
        {
            m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>(selectionGuid),
            m_Features = [BlueprintTool.GetRef<BlueprintFeatureReference>(featureGuid)],
        };

    private static string DeterministicGuid(string key)
    {
        using var md5 = MD5.Create();
        return new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(key))).ToString();
    }
}
