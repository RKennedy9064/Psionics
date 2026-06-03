using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Psionics.Shared.Mechanics;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.BladeSkills;

/// <summary>
/// Status-effect blade skills (Stunning Blade, Dazzling Blade, Wing Clip). Each is a standard-action
/// strike against one foe; on a failed Fortitude save the target gains a status condition applied by
/// a mod-owned buff (no external buff blueprint needed). Save DC uses the engine default for the
/// ability (as with the mod's other supernatural abilities).
/// </summary>
public static class CombatBladeSkills
{
    public static void Configure()
    {
        // Stunning Blade — requires (and expends) psionic focus + a psychic strike charge.
        var stunBuff = ConditionBuff("SKStunnedBuff", Guids.BladeSkillStunnedBuff, "Stunned (Mind Blade)",
            UnitCondition.Stunned, AbilityRefs.CauseFear.Reference.Get().Icon);

        AbilityConfigurator.New("SKStunningBladeAbility", Guids.BladeSkillStunningBladeAbility)
            .SetDisplayName(Loc.Str("SK.StunningBlade.Name", "Stunning Blade"))
            .SetDescription(Loc.Str("SK.StunningBlade.Desc",
                "Expend your psionic focus and a psychic strike charge to deliver a stunning blow. The target must succeed at a Fortitude save or be stunned for 1 round."))
            .SetIcon(AbilityRefs.CauseFear.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Touch)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .SetCanTargetEnemies(true).SetCanTargetSelf(false).SetCanTargetFriends(false)
            .AddAbilityCasterHasFacts(facts:
            [
                BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff),
                BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsychicStrikeChargeBuff),
            ])
            .AddAbilityEffectRunAction(ActionsBuilder.New()
                .RemoveBuff(Guids.PsionicFocusBuff)
                .RemoveBuff(Guids.PsychicStrikeChargeBuff)
                .SavingThrow(type: SavingThrowType.Fortitude, onResult: ActionsBuilder.New()
                    .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuff(stunBuff, ContextDuration.Fixed(1)))))
            .Configure();

        SkillFeature("SKStunningBlade", Guids.BladeSkillStunningBlade, "Stunning Blade",
            "You can deliver a stunning strike with your mind blade by expending your psionic focus and a psychic strike charge (Fortitude negates).",
            Guids.BladeSkillStunningBladeAbility, AbilityRefs.CauseFear.Reference.Get().Icon, 6);

        // Dazzling Blade — channel light to dazzle a foe (single-target simplification of the AoE).
        var dazzleBuff = ConditionBuff("SKDazzledBuff", Guids.BladeSkillDazzledBuff, "Dazzled (Mind Blade)",
            UnitCondition.Dazzled, AbilityRefs.Blur.Reference.Get().Icon);

        AbilityConfigurator.New("SKDazzlingBladeAbility", Guids.BladeSkillDazzlingBladeAbility)
            .SetDisplayName(Loc.Str("SK.DazzlingBlade.Name", "Dazzling Blade"))
            .SetDescription(Loc.Str("SK.DazzlingBlade.Desc",
                "Channel psionic energy through your mind blade to dazzle a foe. The target must succeed at a Fortitude save or be dazzled for 1 minute."))
            .SetIcon(AbilityRefs.Blur.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Close)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Point)
            .SetCanTargetEnemies(true).SetCanTargetSelf(false).SetCanTargetFriends(false)
            .AddAbilityEffectRunAction(ActionsBuilder.New()
                .SavingThrow(type: SavingThrowType.Fortitude, onResult: ActionsBuilder.New()
                    .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuff(dazzleBuff, ContextDuration.Fixed(1, DurationRate.Minutes)))))
            .Configure();

        SkillFeature("SKDazzlingBlade", Guids.BladeSkillDazzlingBlade, "Dazzling Blade",
            "You can channel psionic energy through your mind blade to dazzle a foe (Fortitude negates).",
            Guids.BladeSkillDazzlingBladeAbility, AbilityRefs.Blur.Reference.Get().Icon, 2);

        // Wing Clip — a precise strike that immobilizes rather than wounds.
        var wingBuff = ConditionBuff("SKWingClipBuff", Guids.BladeSkillWingClipBuff, "Wing Clip",
            UnitCondition.CantMove, AbilityRefs.Slow.Reference.Get().Icon);

        AbilityConfigurator.New("SKWingClipAbility", Guids.BladeSkillWingClipAbility)
            .SetDisplayName(Loc.Str("SK.WingClip.Name", "Wing Clip"))
            .SetDescription(Loc.Str("SK.WingClip.Desc",
                "Make a precise strike that hampers movement instead of wounding. The target must succeed at a Fortitude save or be unable to move for 1 round."))
            .SetIcon(AbilityRefs.Slow.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Touch)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .SetCanTargetEnemies(true).SetCanTargetSelf(false).SetCanTargetFriends(false)
            .AddAbilityEffectRunAction(ActionsBuilder.New()
                .SavingThrow(type: SavingThrowType.Fortitude, onResult: ActionsBuilder.New()
                    .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuff(wingBuff, ContextDuration.Fixed(1)))))
            .Configure();

        SkillFeature("SKWingClip", Guids.BladeSkillWingClip, "Wing Clip",
            "You can make a precise mind blade strike that immobilizes a foe instead of wounding it (Fortitude negates).",
            Guids.BladeSkillWingClipAbility, AbilityRefs.Slow.Reference.Get().Icon, 2);

        // Dispelling Strike — expend focus + psychic strike to dispel magic on a foe.
        AbilityConfigurator.New("SKDispellingStrikeAbility", Guids.BladeSkillDispellingStrikeAbility)
            .SetDisplayName(Loc.Str("SK.DispellingStrike.Name", "Dispelling Strike"))
            .SetDescription(Loc.Str("SK.DispellingStrike.Desc",
                "Expend your psionic focus and a psychic strike charge to strike a foe with disruptive energy, attempting to dispel magical effects on the target (as dispel magic)."))
            .SetIcon(AbilityRefs.DisruptingWeapon.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Touch)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .SetCanTargetEnemies(true).SetCanTargetSelf(false).SetCanTargetFriends(false)
            .AddAbilityCasterHasFacts(facts:
            [
                BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff),
                BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsychicStrikeChargeBuff),
            ])
            .AddAbilityEffectRunAction(ActionsBuilder.New()
                .RemoveBuff(Guids.PsionicFocusBuff)
                .RemoveBuff(Guids.PsychicStrikeChargeBuff)
                .DispelMagic(
                    buffType: ContextActionDispelMagic.BuffType.All,
                    checkType: RuleDispelMagic.CheckType.CasterLevel,
                    maxSpellLevel: 9))
            .Configure();

        SkillFeature("SKDispellingStrike", Guids.BladeSkillDispellingStrike, "Dispelling Strike",
            "You can expend your psionic focus and a psychic strike charge to dispel magic on a foe you strike.",
            Guids.BladeSkillDispellingStrikeAbility, AbilityRefs.DisruptingWeapon.Reference.Get().Icon, 8);

        BuildPsychokineticBlast();
        BuildMarkOfTheChallenger();

        // Furious Charge — extra damage on a charge attack (reuses the Powerful Charge mechanic).
        FeatureConfigurator.New("SKFuriousCharge", Guids.BladeSkillFuriousCharge)
            .SetDisplayName(Loc.Str("SK.FuriousCharge.Name", "Furious Charge"))
            .SetDescription(Loc.Str("SK.FuriousCharge.Desc",
                "When you charge, your mind blade strikes with extra force, dealing 2 additional points of damage on the charge attack."))
            .SetIcon(AbilityRefs.ChargeAbility.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 8)
            .AddPowerfulCharge(useContextBonus: true, value: 2)
            .Configure();
    }

    // Psychokinetic Blast — expend a psychic strike charge to deal its damage to the target and
    // everything around it. Damage dice scale on the psychic strike track (1 at 3rd, +1 / 4 levels).
    private static void BuildPsychokineticBlast()
    {
        var rank = ContextRankConfigs.ClassLevel([Guids.SoulKnifeClass]);
        rank.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
        rank.m_StartLevel = 3;
        rank.m_StepLevel = 4;

        AbilityConfigurator.New("SKPsychokineticBlastAbility", Guids.BladeSkillPsychokineticBlastAbility)
            .SetDisplayName(Loc.Str("SK.PsychokineticBlast.Name", "Psychokinetic Blast"))
            .SetDescription(Loc.Str("SK.PsychokineticBlast.Desc",
                "Expend a psychic strike charge to unleash its energy in a burst, dealing your psychic strike damage to the target and all creatures around it."))
            .SetIcon(AbilityRefs.MagicMissile.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Close)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Point)
            .SetCanTargetEnemies(true).SetCanTargetSelf(false).SetCanTargetFriends(false)
            .AddAbilityCasterHasFacts(facts: [BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsychicStrikeChargeBuff)])
            .AddContextRankConfig(rank)
            .AddAbilityTargetsAround(radius: new Feet(10), targetType: TargetType.Enemy)
            .AddAbilityEffectRunAction(ActionsBuilder.New()
                .DealDamage(DamageTypes.Energy(DamageEnergyType.Magic), ContextDice.Value(DiceType.D8, ContextValues.Rank()))
                .OnContextCaster(ActionsBuilder.New().RemoveBuff(Guids.PsychicStrikeChargeBuff)))
            .Configure();

        SkillFeature("SKPsychokineticBlast", Guids.BladeSkillPsychokineticBlast, "Psychokinetic Blast",
            "You can expend a psychic strike charge to deal its damage to a foe and everything around it.",
            Guids.BladeSkillPsychokineticBlastAbility, AbilityRefs.MagicMissile.Reference.Get().Icon, 4);
    }

    // Mark of the Challenger — strike a foe to goad it; it takes a −2 penalty on attack rolls.
    private static void BuildMarkOfTheChallenger()
    {
        var buff = BuffConfigurator.New("SKMarkOfTheChallengerBuff", Guids.BladeSkillMarkOfTheChallengerBuff)
            .SetDisplayName(Loc.Str("SK.MarkOfTheChallenger.BuffName", "Marked"))
            .SetDescription(Loc.Str("SK.MarkOfTheChallenger.BuffDesc",
                "Goaded by the soulknife: −2 penalty on attack rolls."))
            .SetIcon(AbilityRefs.CauseFear.Reference.Get().Icon)
            .AddComponent(new MarkOfTheChallengerComponent())
            .Configure();

        AbilityConfigurator.New("SKMarkOfTheChallengerAbility", Guids.BladeSkillMarkOfTheChallengerAbility)
            .SetDisplayName(Loc.Str("SK.MarkOfTheChallenger.Name", "Mark of the Challenger"))
            .SetDescription(Loc.Str("SK.MarkOfTheChallenger.Desc",
                "Strike a foe and mark it as your quarry. The target takes a −2 penalty on its attack rolls until the start of your next turn."))
            .SetIcon(AbilityRefs.CauseFear.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Touch)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .SetCanTargetEnemies(true).SetCanTargetSelf(false).SetCanTargetFriends(false)
            .AddAbilityEffectRunAction(ActionsBuilder.New()
                .ApplyBuff(buff, ContextDuration.Fixed(1)))
            .Configure();

        SkillFeature("SKMarkOfTheChallenger", Guids.BladeSkillMarkOfTheChallenger, "Mark of the Challenger",
            "You can mark a foe with a strike, imposing a −2 penalty on its attack rolls.",
            Guids.BladeSkillMarkOfTheChallengerAbility, AbilityRefs.CauseFear.Reference.Get().Icon, 2);
    }

    private static BlueprintBuff ConditionBuff(string name, string guid, string displayName,
        UnitCondition condition, UnityEngine.Sprite icon)
    {
        return BuffConfigurator.New(name, guid)
            .SetDisplayName(Loc.Str($"SK.{name}.Name", displayName))
            .SetDescription(Loc.Str($"SK.{name}.Desc", $"Afflicted with {condition}."))
            .SetIcon(icon)
            .AddComponent(new AddCondition { Condition = condition })
            .Configure();
    }

    private static void SkillFeature(string name, string guid, string displayName, string description,
        string abilityGuid, UnityEngine.Sprite icon, int minLevel)
    {
        FeatureConfigurator.New(name, guid)
            .SetDisplayName(Loc.Str($"SK.{name}.FeatureName", displayName))
            .SetDescription(Loc.Str($"SK.{name}.FeatureDesc", description))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFacts([abilityGuid])
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, minLevel)
            .Configure();
    }
}
