using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Psionics.Utils;

namespace Psionics.PsychicWarrior;

public static class PsychicWarriorClass
{
    public static void Configure()
    {
        var progression = ProgressionConfigurator.New("PsychicWarrior.Progression", Guids.PsychicWarriorProgression)
            .SetDisplayName(Loc.Str("PW.Progression.Name", "Psychic Warrior"))
            .SetDescription(Loc.Str("PW.Progression.Desc",
                "Psychic Warriors are psionic combatants who fuse martial skill with the power of the mind."))
            .SetClasses(Guids.PsychicWarriorClass)
            .SetUIGroups(
                UIGroupBuilder.New()
                    .AddGroup(Guids.BonusFeatSelection)
                    .AddGroup(Guids.Proficiencies, Guids.GainPsionicFocusFeature, Guids.PsionicProficiency)
                    .AddGroup(
                        Guids.PathSelectionLevel1,
                        Guids.PathExpandedManeuver,
                        Guids.TalentsSelection,
                        Guids.PathSkillBonusSelection,
                        Guids.MartialPowerFeature)
                    .AddGroup(Guids.SecondaryPathSelection, Guids.TwistingPaths, Guids.Pathweaving))
            // Level 1: proficiencies, focus, psionic proficiency, path, 2 talent picks, bonus feat
            .AddToLevelEntries(1,
                Guids.Proficiencies,
                Guids.GainPsionicFocusFeature,
                Guids.PsionicProficiency,
                Guids.PathSelectionLevel1,
                Guids.TalentsSelection,  // talent pick 1
                Guids.TalentsSelection,  // talent pick 2
                Guids.BonusFeatSelection)
            .AddToLevelEntries(2, Guids.BonusFeatSelection)
            .AddToLevelEntries(3, Guids.PathExpandedManeuver)
            .AddToLevelEntries(4, Guids.PathSkillBonusSelection)
            .AddToLevelEntries(5, Guids.BonusFeatSelection)
            .AddToLevelEntries(6, Guids.MartialPowerFeature)
            .AddToLevelEntries(7, Guids.PathSkillBonusSelection)
            .AddToLevelEntries(8, Guids.BonusFeatSelection)
            .AddToLevelEntries(9, Guids.SecondaryPathSelection, Guids.PathExpandedManeuver)
            .AddToLevelEntries(10, Guids.PathSkillBonusSelection)
            .AddToLevelEntries(11, Guids.BonusFeatSelection)
            .AddToLevelEntries(12, Guids.TwistingPaths)
            .AddToLevelEntries(13, Guids.PathSkillBonusSelection)
            .AddToLevelEntries(14, Guids.BonusFeatSelection)
            .AddToLevelEntries(15, Guids.Pathweaving)
            .AddToLevelEntries(16, Guids.PathSkillBonusSelection)
            .AddToLevelEntries(17, Guids.BonusFeatSelection)
            .AddToLevelEntries(19, Guids.PathSkillBonusSelection)
            .AddToLevelEntries(20, Guids.BonusFeatSelection, Guids.EternalWarriorFeature)
            .Configure();

        CharacterClassConfigurator.New("PsychicWarrior.Class", Guids.PsychicWarriorClass)
            .SetLocalizedName(Loc.Str("PW.Class.Name", "Psychic Warrior"))
            .SetLocalizedDescription(Loc.Str("PW.Class.Desc",
                "Psychic Warriors are psionic combatants who fuse martial skill with the power of the mind. "
                + "By attuning their mental energy through Psionic Focus, they augment their attacks, harden "
                + "their bodies, and channel minor psionic effects — all without sacrificing the fighting "
                + "prowess of a dedicated warrior. At 1st level they choose a Path, a martial discipline "
                + "that defines their style; a second Path is available at 9th level. Bonus combat and "
                + "psionic feats round out a highly adaptable martial toolkit."))
            .SetLocalizedDescriptionShort(Loc.Str("PW.Class.DescShort",
                "A psionic warrior who channels mental focus to augment attacks, harden the body, "
                + "and channel minor psionic effects — without sacrificing martial prowess."))
            .SetHitDie(DiceType.D8)
            .SetSkillPoints(4)
            .SetBaseAttackBonus(StatProgressionRefs.BABMedium.ToString())
            .SetFortitudeSave(StatProgressionRefs.SavesHigh.ToString())
            .SetReflexSave(StatProgressionRefs.SavesLow.ToString())
            .SetWillSave(StatProgressionRefs.SavesLow.ToString())
            .SetClassSkills(
                StatType.SkillAthletics,
                StatType.SkillMobility,
                StatType.SkillPerception,
                StatType.SkillStealth)
            .SetSpellbook(Guids.Spellbook)
            .SetProgression(progression)
            .SetStartingItems(ItemArmorRefs.ScalemailStandard.Reference.Get())
            .SetStartingGold(411)
            .SetPrimaryColor(1)
            .SetSecondaryColor(1)
            .SetDifficulty(1)
            .SetRecommendedAttributes(StatType.Strength, StatType.Wisdom)
            .SetNotRecommendedAttributes(StatType.Charisma)
            .SetSignatureAbilities(
                Guids.GainPsionicFocusFeature,
                Guids.PsionicProficiency,
                Guids.MartialPowerFeature,
                Guids.EternalWarriorFeature)
            .SetDefaultBuild(Guids.PrebuildPsychicWarriorFeatureList)
            .OnConfigure(bp =>
            {
                var fighter = CharacterClassRefs.FighterClass.Reference.Get();
                bp.MaleEquipmentEntities = fighter.MaleEquipmentEntities;
                bp.FemaleEquipmentEntities = fighter.FemaleEquipmentEntities;
            })
            .Configure();
    }
}
