using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features.Paths;

public static class GladiatorPath
{
    public static void Configure()
    {
        var icon = AbilityRefs.BullsStrength.Reference.Get().Icon;
        var maneuverIcon = FeatureRefs.ImprovedBullRush.Reference.Get().Icon;
        var expandedIcon = AbilityRefs.Bless.Reference.Get().Icon;

        var trance = TranceHelper.BuildTrance(
            baseName: "Gladiator",
            tranceFeatureGuid: Guids.GladiatorTrance,
            tranceBuffGuid: Guids.GladiatorTranceBuff,
            parentAbilityGuid: Guids.GladiatorPathParent,
            maneuverAbilityGuid: Guids.GladiatorManeuverAbility,
            expandedManeuverAbilityGuid: Guids.GladiatorExpandedAbility,
            displayName: "Gladiator",
            featureDescription: "Beginning at 3rd level, your psionic focus enhances your combat maneuver capabilities. You gain a +2 competence bonus to CMB, increasing by 1 every four psychic warrior levels (+3 at 7th, +4 at 11th, +5 at 15th, +6 at 19th).",
            icon: icon,
            addBuffComponents: b =>
            {
                b.AddContextRankConfig(ContextRankConfigs.CasterLevel()
                    .WithCustomProgression((3, 2), (7, 3), (11, 4), (15, 5), (19, 6)));
                b.AddContextStatBonus(
                    stat: StatType.AdditionalCMB,
                    descriptor: ModifierDescriptor.Competence,
                    value: ContextValues.Rank());
            });

        var maneuverBuff = BuffConfigurator.New("GladiatorManeuverBuff", Guids.GladiatorManeuverBuff)
            .SetDisplayName(Loc.Str("PW.GladiatorManeuver.BuffName", "Gladiator Maneuver"))
            .SetDescription(Loc.Str("PW.GladiatorManeuver.BuffDesc",
                "You gain a +4 competence bonus to CMB for 1 round."))
            .SetIcon(maneuverIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalCMB, value: 4)
            .Configure();

        var maneuverAbility = AbilityConfigurator.New("GladiatorManeuver", Guids.GladiatorManeuverAbility)
            .SetDisplayName(Loc.Str("PW.GladiatorManeuverAb.Name", "Gladiator Maneuver"))
            .SetDescription(Loc.Str("PW.GladiatorManeuverAb.Desc",
                "Swift Action. Expend psionic focus to gain a +4 competence bonus to CMB for 1 round, enhancing trip, disarm, and other maneuvers."))
            .SetIcon(maneuverIcon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .RemoveBuff(Guids.PsionicFocusBuff)
                    .ApplyBuff(maneuverBuff, ContextDuration.Fixed(1)))
            .Configure();

        // Expanded � Gladiator's Will: +4 competence to all saves for 1 round
        var expandedBuff = BuffConfigurator.New("GladiatorExpandedManeuverBuff", Guids.GladiatorExpandedBuff)
            .SetDisplayName(Loc.Str("PW.GladiatorExpanded.BuffName", "Gladiator's Will"))
            .SetDescription(Loc.Str("PW.GladiatorExpanded.BuffDesc",
                "Iron focus carries you through anything: +4 competence to all saving throws for 1 round."))
            .SetIcon(expandedIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SaveFortitude, value: 4)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SaveReflex, value: 4)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SaveWill, value: 4)
            .Configure();

        var expandedAbility = AbilityConfigurator.New("GladiatorExpandedManeuverAbility", Guids.GladiatorExpandedAbility)
            .SetDisplayName(Loc.Str("PW.GladiatorExpandedAb.Name", "Gladiator's Will"))
            .SetDescription(Loc.Str("PW.GladiatorExpandedAb.Desc",
                "Swift Action. Expend psionic focus to steel your resolve: gain +4 competence to all saving throws for 1 round."))
            .SetIcon(maneuverIcon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .RemoveBuff(Guids.PsionicFocusBuff)
                    .ApplyBuff(expandedBuff, ContextDuration.Fixed(1)))
            .AddAbilityShowIfCasterHasFact(not: false, unitFact: Guids.GladiatorExpandedFeature)
            .Configure();

        FeatureConfigurator.New("GladiatorExpandedManeuver", Guids.GladiatorExpandedFeature)
            .SetDisplayName(Loc.Str("PW.GladiatorExpandedFeat.Name", "Gladiator's Will"))
            .SetDescription(Loc.Str("PW.GladiatorExpandedFeat.Desc",
                "You learn the Gladiator's Will maneuver: a swift-action self-buff granting +4 competence to all saving throws for 1 round."))
            .SetIcon(expandedIcon)
            .SetIsClassFeature()
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerGladiatorExpanded)
            .AddPrerequisiteFeature(Guids.GladiatorPath)
            .Configure();

        FeatureConfigurator.New("GladiatorPath", Guids.GladiatorPath)
            .SetDisplayName(Loc.Str("PW.GladiatorPath.Name", "Gladiator Path"))
            .SetDescription(Loc.Str("PW.GladiatorPath.Desc",
                "You dominate the arena with combat maneuvers and brute force.\n" +
                "\n" +
                "Trance (3rd level): while focused, gain a +2 competence bonus to CMB, increasing by +1 every four levels (+3 at 7th, +4 at 11th, +5 at 15th, +6 at 19th).\n" +
                "\n" +
                "Maneuver: expend psionic focus (swift action) for +4 competence to CMB for 1 round (trip, disarm, and other maneuvers).\n" +
                "\n" +
                "Expanded Maneuver: expend psionic focus to steel your resolve — +4 competence to all saving throws for 1 round.\n" +
                "\n" +
                "A second path can be chosen at 9th level."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFeatureOnClassLevel(feature: trance.ToString(), level: 3, clazz: Guids.PsychicWarriorClass)
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerGladiatorManeuver)
            .AddPrerequisiteNoFeature(Guids.GladiatorPath)
            .Configure();
    }
}
