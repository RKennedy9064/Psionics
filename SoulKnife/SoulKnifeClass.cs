using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Psionics.Utils;

namespace Psionics.SoulKnife;

public static class SoulKnifeClass
{
    public static void Configure()
    {
        // Bonus feat selection at level 1: Power Attack or Two-Weapon Fighting
        FeatureSelectionConfigurator.New("SoulKnifeBonusFeat", Guids.SoulKnifeBonusFeat)
            .SetDisplayName(Loc.Str("SK.BonusFeat.Name", "Soulknife Bonus Feat"))
            .SetDescription(Loc.Str("SK.BonusFeat.Desc",
                "At 1st level, a soulknife gains a bonus combat feat: Power Attack or Two-Weapon Fighting."))
            .SetIcon(FeatureRefs.PowerAttackFeature.Reference.Get().Icon)
            .SetIsClassFeature()
            .SetIgnorePrerequisites(true)
            .AddToAllFeatures(
                Guids.WeaponFocusMindBlade,
                FeatureRefs.PowerAttackFeature.ToString(),
                FeatureRefs.TwoWeaponFighting.ToString())
            .Configure();

        var progression = ProgressionConfigurator.New("SoulKnife.Progression", Guids.SoulKnifeProgression)
            .SetDisplayName(Loc.Str("SK.Progression.Name", "Soulknife"))
            .SetDescription(Loc.Str("SK.Progression.Desc",
                "Soulknives are psionic warriors who manifest a psychic blade from the power of their mind."))
            .SetClasses(Guids.SoulKnifeClass)
            // L1: Proficiencies, Wild Talent (psionic focus), Bonus Feat, Form Mind Blade, first Blade Skill
            .AddToLevelEntries(1,
                Guids.SoulKnifeProficiencies,
                Guids.GainPsionicFocusFeature,
                Guids.SoulKnifeBonusFeat,
                Guids.MindBladeFeature)
            .AddToLevelEntries(2, Guids.BladeSkillsSelection)
            .AddToLevelEntries(3, Guids.PsychicStrikeFeature, Guids.EnhancedMindBladeFeature)
            .AddToLevelEntries(4, Guids.BladeSkillsSelection)
            .AddToLevelEntries(5, Guids.EnhancedMindBladeAbilities5)
            .AddToLevelEntries(6, Guids.BladeSkillsSelection)
            .AddToLevelEntries(7, Guids.EnhancedMindBladeAbilities7)
            .AddToLevelEntries(8, Guids.BladeSkillsSelection)
            .AddToLevelEntries(9)
            .AddToLevelEntries(10, Guids.BladeSkillsSelection)
            .AddToLevelEntries(11)
            .AddToLevelEntries(12, Guids.BladeSkillsSelection, Guids.EnhancedMindBladeAbilities12)
            .AddToLevelEntries(13)
            .AddToLevelEntries(14, Guids.BladeSkillsSelection)
            .AddToLevelEntries(15)
            .AddToLevelEntries(16, Guids.BladeSkillsSelection)
            .AddToLevelEntries(17)
            .AddToLevelEntries(18, Guids.BladeSkillsSelection)
            .AddToLevelEntries(19)
            .AddToLevelEntries(20, Guids.BladeSkillsSelection)
            .Configure();

        CharacterClassConfigurator.New("SoulKnife.Class", Guids.SoulKnifeClass)
            .SetLocalizedName(Loc.Str("SK.Class.Name", "Soulknife"))
            .SetLocalizedDescription(Loc.Str("SK.Class.Desc",
                "The soulknife is a psionic warrior who manifests a blade of pure psychic energy. Unlike other weapon-users, a soulknife needs no physical weapon — her mind blade appears in her hand whenever she wills it, shaped from concentrated mental force. Soulknives rely on their Wisdom to enhance combat, and learn Blade Skills to further hone their edge."))
            .SetLocalizedDescriptionShort(Loc.Str("SK.Class.DescShort",
                "A psionic warrior who manifests a blade of pure mental energy, enhanced by Wisdom and customized through Blade Skills."))
            .SetHitDie(DiceType.D10)
            .SetSkillPoints(4)
            .SetBaseAttackBonus(StatProgressionRefs.BABFull.ToString())
            .SetFortitudeSave(StatProgressionRefs.SavesLow.ToString())
            .SetReflexSave(StatProgressionRefs.SavesHigh.ToString())
            .SetWillSave(StatProgressionRefs.SavesHigh.ToString())
            .SetClassSkills(
                StatType.SkillAthletics,
                StatType.SkillMobility,
                StatType.SkillPerception,
                StatType.SkillStealth,
                StatType.SkillPersuasion,
                StatType.SkillThievery,
                StatType.SkillUseMagicDevice)
            .SetProgression(progression)
            .SetStartingItems(ItemArmorRefs.ScalemailStandard.Reference.Get())
            .SetStartingGold(411)
            .SetPrimaryColor(6)
            .SetSecondaryColor(14)
            .SetDifficulty(2)
            .SetRecommendedAttributes(StatType.Dexterity, StatType.Wisdom)
            .SetNotRecommendedAttributes(StatType.Charisma)
            .SetSignatureAbilities(Guids.MindBladeFeature, Guids.PsychicStrikeFeature)
            .SetDefaultBuild(Guids.SoulKnifePrebuild)
            .OnConfigure(bp =>
            {
                var fighter = CharacterClassRefs.FighterClass.Reference.Get();
                bp.MaleEquipmentEntities = fighter.MaleEquipmentEntities;
                bp.FemaleEquipmentEntities = fighter.FemaleEquipmentEntities;
            })
            .Configure();
    }
}
