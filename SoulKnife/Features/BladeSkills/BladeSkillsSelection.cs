using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.BladeSkills;

public static class BladeSkillsSelection
{
    // General psionic feats a soulknife may pick via the Psionic Training blade skill.
    // (Excludes the Psychic Warrior path-specific "Advanced Path" feats.)
    private static readonly string[] PsionicFeats =
    [
        Guids.PsionicMeditationFeat,
        Guids.PsionicWeaponFeat,
        Guids.PsionicFistFeat,
        Guids.PsionicShotFeat,
        Guids.IntuitiveShotFeat,
        Guids.PsionicBodyFeat,
        Guids.SpeedOfThoughtFeat,
        Guids.PsionicDodgeFeat,
        Guids.CriticalRefocusFeat,
        Guids.GreaterPsionicWeaponFeat,
        Guids.GreaterPsionicFistFeat,
        Guids.GreaterPsionicShotFeat,
        Guids.RapidMetabolismFeat,
        Guids.CombatManifestationFeat,
        Guids.DeepImpactFeat,
        Guids.UpTheWallsFeat,
        Guids.PsionicEndowmentFeat,
        Guids.GreaterPsionicEndowmentFeat,
        Guids.PsionicCriticalFeat,
        Guids.RecklessOffenseFeat,
        Guids.AlignedAttackFeat,
        Guids.WoundingAttackFeat,
        Guids.FellShotFeat,
        Guids.UnavoidableStrikeFeat,
        Guids.IntuitiveFightingFeat,
    ];

    public static void Configure()
    {
        // Focused Offense and Focused Defense are configured in their own files
        // (FocusedOffense.Configure / FocusedDefense.Configure), referenced by GUID below.

        var evasion = FeatureConfigurator.New("SKBladeSkillEvasion", Guids.BladeSkillEvasion)
            .SetDisplayName(Loc.Str("SK.Evasion.Name", "Evasion"))
            .SetDescription(Loc.Str("SK.Evasion.Desc",
                "If you make a successful Reflex saving throw against an attack that normally deals half damage on a successful save, you instead take no damage."))
            .SetIcon(FeatureRefs.Evasion.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddEvasion()
            .Configure();

        var improvedEvasion = FeatureConfigurator.New("SKBladeSkillImprovedEvasion", Guids.BladeSkillImprovedEvasion)
            .SetDisplayName(Loc.Str("SK.ImprovedEvasion.Name", "Improved Evasion"))
            .SetDescription(Loc.Str("SK.ImprovedEvasion.Desc",
                "This works like Evasion, except that while you take no damage on a successful Reflex saving throw against attacks, you henceforth take only half damage on a failed save."))
            .SetIcon(FeatureRefs.ImprovedEvasion.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddImprovedEvasion()
            .AddPrerequisiteFeature(Guids.BladeSkillEvasion)
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 10)
            .Configure();

        var powerfulStrikes = FeatureConfigurator.New("SKPowerfulStrikes", Guids.BladeSkillPowerfulStrikes)
            .SetDisplayName(Loc.Str("SK.PowerfulStrikes.Name", "Powerful Strikes"))
            .SetDescription(Loc.Str("SK.PowerfulStrikes.Desc",
                "Your psychic strike deals an additional 1d8 points of damage."))
            .SetIcon(FeatureRefs.PowerAttackFeature.Reference.Get().Icon)
            .SetIsClassFeature()
            .Configure();

        var vampiricBlade = FeatureConfigurator.New("SKVampiricBlade", Guids.BladeSkillVampiricBlade)
            .SetDisplayName(Loc.Str("SK.VampiricBlade.Name", "Vampiric Blade"))
            .SetDescription(Loc.Str("SK.VampiricBlade.Desc",
                "When you deal psychic strike damage to a living creature with your mind blade, you heal a number of hit points equal to half the psychic strike damage dealt."))
            .SetIcon(AbilityRefs.VampiricTouch.Reference.Get().Icon)
            .SetIsClassFeature()
            .Configure();

        var improvedEnhancement = FeatureConfigurator.New("SKImprovedEnhancement", Guids.BladeSkillImprovedEnhancement)
            .SetDisplayName(Loc.Str("SK.ImprovedEnhancement.Name", "Improved Enhancement"))
            .SetDescription(Loc.Str("SK.ImprovedEnhancement.Desc",
                "The enhancement bonus of your mind blade increases by 1 (to a maximum of +5)."))
            .SetIcon(AbilityRefs.MagicWeaponGreater.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 12)
            .Configure();

        var telekineticEdge = FeatureConfigurator.New("SKTelekineticEdge", Guids.BladeSkillTelekineticEdge)
            .SetDisplayName(Loc.Str("SK.TelekineticEdge.Name", "Telekinetic Edge"))
            .SetDescription(Loc.Str("SK.TelekineticEdge.Desc",
                "Your mind blade is so sharp it ignores the damage reduction of your foes."))
            .SetIcon(AbilityRefs.TrueStrike.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddComponent(new IgnoreDamageReductionOnAttack
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(Guids.MindBladeLightWeaponType) })
            .AddComponent(new IgnoreDamageReductionOnAttack
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(Guids.MindBladeOneHandedWeaponType) })
            .AddComponent(new IgnoreDamageReductionOnAttack
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(Guids.MindBladeTwoHandedWeaponType) })
            .AddComponent(new IgnoreDamageReductionOnAttack
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(Guids.MindBladeDoubleWeaponType) })
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 4)
            .Configure();

        var trapfinder = FeatureConfigurator.New("SKTrapfinder", Guids.BladeSkillTrapfinder)
            .SetDisplayName(Loc.Str("SK.Trapfinder.Name", "Trapfinder"))
            .SetDescription(Loc.Str("SK.Trapfinder.Desc",
                "You gain a bonus to Perception checks to locate traps, scaling with your soulknife level."))
            .SetIcon(FeatureRefs.SkillFocusPerception.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddContextRankConfig(ContextRankConfigs.ClassLevel([Guids.SoulKnifeClass]))
            .AddTrapPerceptionBonus(descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .Configure();

        var mindBladeFinesse = FeatureConfigurator.New("SKMindBladeFinesse", Guids.BladeSkillMindBladeFinesse)
            .SetDisplayName(Loc.Str("SK.MindBladeFinesse.Name", "Mind Blade Finesse"))
            .SetDescription(Loc.Str("SK.MindBladeFinesse.Desc",
                "You use your Dexterity modifier instead of your Strength modifier on attack rolls with your mind blade, in any form (including two-handed)."))
            .SetIcon(FeatureRefs.WeaponFinesse.Reference.Get().Icon)
            .SetIsClassFeature()
            .Configure();

        var weaponSpecial = FeatureConfigurator.New("SKWeaponSpecialTrip", Guids.BladeSkillWeaponSpecial)
            .SetDisplayName(Loc.Str("SK.WeaponSpecial.Name", "Weapon Special (Trip)"))
            .SetDescription(Loc.Str("SK.WeaponSpecial.Desc",
                "Your mind blade is shaped for tripping. On a successful melee hit, you make a free trip combat maneuver against the target."))
            .SetIcon(FeatureRefs.PowerAttackFeature.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddComponent(new MindBladeTripOnHitComponent())
            .Configure();

        var reapersBlade = FeatureConfigurator.New("SKReapersBlade", Guids.BladeSkillReapersBlade)
            .SetDisplayName(Loc.Str("SK.ReapersBlade.Name", "Reaper's Blade"))
            .SetDescription(Loc.Str("SK.ReapersBlade.Desc",
                "Whenever you reduce a foe to 0 or fewer hit points with a psychic strike, your psychic strike immediately recharges."))
            .SetIcon(AbilityRefs.Enervation.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 10)
            .Configure();

        var deadlyBlow = FeatureConfigurator.New("SKDeadlyBlow", Guids.BladeSkillDeadlyBlow)
            .SetDisplayName(Loc.Str("SK.DeadlyBlow.Name", "Deadly Blow"))
            .SetDescription(Loc.Str("SK.DeadlyBlow.Desc",
                "The critical multiplier of your mind blade increases by 1 (for example, ×2 becomes ×3)."))
            .SetIcon(AbilityRefs.Disintegrate.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 10)
            .AddComponent(new WeaponTypeCriticalMultiplierIncrease
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(Guids.MindBladeLightWeaponType), AdditionalMultiplier = 1 })
            .AddComponent(new WeaponTypeCriticalMultiplierIncrease
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(Guids.MindBladeOneHandedWeaponType), AdditionalMultiplier = 1 })
            .AddComponent(new WeaponTypeCriticalMultiplierIncrease
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(Guids.MindBladeTwoHandedWeaponType), AdditionalMultiplier = 1 })
            .AddComponent(new WeaponTypeCriticalMultiplierIncrease
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(Guids.MindBladeDoubleWeaponType), AdditionalMultiplier = 1 })
            .Configure();

        var knifeToTheSoul = FeatureConfigurator.New("SKKnifeToTheSoul", Guids.BladeSkillKnifeToTheSoul)
            .SetDisplayName(Loc.Str("SK.KnifeToTheSoul.Name", "Knife to the Soul"))
            .SetDescription(Loc.Str("SK.KnifeToTheSoul.Desc",
                "Your psychic strike also savages the mind: it deals Intelligence damage equal to half its dice (rounded down, minimum 1) in addition to its normal damage."))
            .SetIcon(AbilityRefs.MindBlank.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 12)
            .Configure();

        var devastatingBlade = FeatureConfigurator.New("SKDevastatingBlade", Guids.BladeSkillDevastatingBlade)
            .SetDisplayName(Loc.Str("SK.DevastatingBlade.Name", "Devastating Blade"))
            .SetDescription(Loc.Str("SK.DevastatingBlade.Desc",
                "Your Knife to the Soul grows more potent, dealing Intelligence damage equal to your full psychic strike dice."))
            .SetIcon(AbilityRefs.Enervation.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddPrerequisiteFeature(Guids.BladeSkillKnifeToTheSoul)
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 16)
            .Configure();

        var explodingCritical = FeatureConfigurator.New("SKExplodingCritical", Guids.BladeSkillExplodingCritical)
            .SetDisplayName(Loc.Str("SK.ExplodingCritical.Name", "Exploding Critical"))
            .SetDescription(Loc.Str("SK.ExplodingCritical.Desc",
                "When you confirm a critical hit with your mind blade, you may expend your psionic focus to deal psychic strike damage, even if you had no psychic strike charged."))
            .SetIcon(AbilityRefs.Disintegrate.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 12)
            .AddComponent(new ExplodingCriticalComponent())
            .Configure();

        var telekineticAthleticism = FeatureConfigurator.New("SKTelekineticAthleticism", Guids.BladeSkillTelekineticAthleticism)
            .SetDisplayName(Loc.Str("SK.TelekineticAthleticism.Name", "Telekinetic Athleticism"))
            .SetDescription(Loc.Str("SK.TelekineticAthleticism.Desc",
                "You gain the benefits of the Speed of Thought feat, telekinetically propelling yourself while psionically focused."))
            .SetIcon(AbilityRefs.Haste.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddFacts([Guids.SpeedOfThoughtFeat])
            .Configure();

        // Psionic Training is itself a feature selection; picking it in the Blade Skill list
        // cascades the level-up UI into the psionic feat choice. Populate via AddToAllFeatures
        // (setting m_AllFeatures in OnConfigure is overwritten by Configure()).
        var psionicTrainingConf = FeatureSelectionConfigurator.New("SKPsionicTraining", Guids.BladeSkillPsionicTraining)
            .SetDisplayName(Loc.Str("SK.PsionicTraining.Name", "Psionic Training"))
            .SetDescription(Loc.Str("SK.PsionicTraining.Desc",
                "You gain a psionic feat of your choice. You must meet the feat's prerequisites."))
            .SetIcon(FeatureRefs.MartialWeaponProficiency.Reference.Get().Icon)
            .SetIsClassFeature()
            .SetIgnorePrerequisites(false);
        foreach (var g in PsionicFeats)
            psionicTrainingConf = psionicTrainingConf.AddToAllFeatures(g);
        var psionicTraining = psionicTrainingConf.Configure();

        FeatureSelectionConfigurator.New("SKBladeSkillsSelection", Guids.BladeSkillsSelection)
            .SetDisplayName(Loc.Str("SK.BladeSkills.Name", "Blade Skill"))
            .SetDescription(Loc.Str("SK.BladeSkills.Desc",
                "At 2nd level and every two levels thereafter, a soulknife may choose a blade skill to enhance her mind blade or improve her abilities."))
            .SetIcon(AbilityRefs.MagicWeapon.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddToAllFeatures(
                Guids.BladeSkillFocusedOffense,
                Guids.BladeSkillFocusedDefense,
                evasion,
                improvedEvasion,
                powerfulStrikes,
                vampiricBlade,
                improvedEnhancement,
                telekineticEdge,
                trapfinder,
                mindBladeFinesse,
                weaponSpecial,
                reapersBlade,
                deadlyBlow,
                knifeToTheSoul,
                devastatingBlade,
                explodingCritical,
                telekineticAthleticism,
                psionicTraining,
                Guids.BladeSkillFireBlade,
                Guids.BladeSkillIceBlade,
                Guids.BladeSkillLightningBlade,
                Guids.BladeSkillThunderBlade,
                Guids.BladeSkillGhostStep,
                Guids.BladeSkillCleaveSpace,
                Guids.BladeSkillReachingBlade,
                Guids.BladeSkillStunningBlade,
                Guids.BladeSkillDazzlingBlade,
                Guids.BladeSkillWingClip,
                Guids.BladeSkillDispellingStrike,
                Guids.BladeSkillPsychokineticBlast,
                Guids.BladeSkillMarkOfTheChallenger,
                Guids.BladeSkillFirestorm,
                Guids.BladeSkillFreezingIce,
                Guids.BladeSkillLightningArc,
                Guids.BladeSkillResoundingThunder,
                Guids.BladeSkillExtendedStrike,
                Guids.BladeSkillFuriousCharge)
            .Configure();
    }
}
