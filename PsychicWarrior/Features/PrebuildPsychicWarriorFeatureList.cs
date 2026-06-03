using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features;

public static class PrebuildPsychicWarriorFeatureList
{
    // External combat feat GUIDs (from FighterFeatSelection, available in BonusFeatSelection)
    private const string PowerAttack = "9972f33f977fc724c838e59641b2fca5";
    private const string Toughness = "d09b20029e9abfe4480b356c92095623";
    private const string FuriousFocus = "f09b89812cc94b89a09069671002b899";
    private const string CombatReflexes = "0f8939ae6f220984e8fb568abbdfba95";

    public static void Configure()
    {
        FeatureConfigurator.New("PsychicWarriorPrebuild", Guids.PrebuildPsychicWarriorFeatureList)
            .SetDisplayName(Loc.Str("PW.Prebuild.Name", "Psychic Warrior"))
            .SetDescription(Loc.Str("PW.Prebuild.Desc",
                "A melee-focused Psychic Warrior built around the Gladiator path. "
                + "Channels Psionic Focus to augment attacks and endure punishment."))
            .AddClassLevels(
                characterClass: Guids.PsychicWarriorClass,
                levels: 20,
                raceStat: StatType.Strength,
                levelsStat: StatType.Wisdom,
                skills:
                [
                    StatType.SkillAthletics,
                    StatType.SkillPerception,
                    StatType.SkillMobility,
                    StatType.SkillStealth,
                ],
                selections:
                [
                    // L1: Gladiator path
                    Sel(Guids.PathSelectionLevel1,     Guids.GladiatorPath),
                    // L1: Talent 1 — Burst
                    Sel(Guids.TalentsSelection,        Guids.TalentBurst),
                    // L1: Talent 2 — Valor
                    Sel(Guids.TalentsSelection,        Guids.TalentValor),
                    // L1 bonus feat — Power Attack
                    Sel(Guids.BonusFeatSelection,      PowerAttack),
                    // L2 bonus feat — Toughness
                    Sel(Guids.BonusFeatSelection,      Toughness),
                    // L4 path skill — Athletics
                    Sel(Guids.PathSkillBonusSelection, Guids.PathSkillAthletics),
                    // L5 bonus feat — Furious Focus
                    Sel(Guids.BonusFeatSelection,      FuriousFocus),
                    // L7 path skill — Athletics
                    Sel(Guids.PathSkillBonusSelection, Guids.PathSkillAthletics),
                    // L8 bonus feat — Combat Reflexes
                    Sel(Guids.BonusFeatSelection,      CombatReflexes),
                    // L9 secondary path — Brawler
                    Sel(Guids.SecondaryPathSelection,  Guids.BrawlerPath),
                    // L10 path skill — Perception
                    Sel(Guids.PathSkillBonusSelection, Guids.PathSkillPerception),
                    // L11 bonus feat — Psionic Weapon
                    Sel(Guids.BonusFeatSelection,      Guids.PsionicWeaponFeat),
                    // L13 path skill — Athletics
                    Sel(Guids.PathSkillBonusSelection, Guids.PathSkillAthletics),
                    // L14 bonus feat — Psionic Body
                    Sel(Guids.BonusFeatSelection,      Guids.PsionicBodyFeat),
                    // L16 path skill — Athletics
                    Sel(Guids.PathSkillBonusSelection, Guids.PathSkillAthletics),
                    // L17 bonus feat — Rapid Metabolism
                    Sel(Guids.BonusFeatSelection,      Guids.RapidMetabolismFeat),
                    // L19 path skill — Athletics
                    Sel(Guids.PathSkillBonusSelection, Guids.PathSkillAthletics),
                    // L20 bonus feat — Psionic Endowment
                    Sel(Guids.BonusFeatSelection,      Guids.PsionicEndowmentFeat),
                ])
            .AddStatsDistributionPreset(
                targetPoints: 20,
                strength: 16, dexterity: 12, constitution: 14,
                intelligence: 8, wisdom: 14, charisma: 8)
            .AddStatsDistributionPreset(
                targetPoints: 25,
                strength: 17, dexterity: 12, constitution: 14,
                intelligence: 10, wisdom: 14, charisma: 8)
            .AddBuildBalanceRadarChart(
                melee: 4, ranged: 1, magic: 2, defense: 3, support: 1, control: 1)
            .OnConfigure(_ => UnityEngine.Debug.Log("[PW] PrebuildPsychicWarriorFeatureList configured OK"))
            .Configure();
    }

    private static SelectionEntry Sel(string selectionGuid, string featureGuid) =>
        new()
        {
            m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>(selectionGuid),
            m_Features = [BlueprintTool.GetRef<BlueprintFeatureReference>(featureGuid)],
        };
}
