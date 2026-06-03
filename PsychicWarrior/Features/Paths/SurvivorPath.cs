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
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features.Paths;

public static class SurvivorPath
{
    public static void Configure()
    {
        var icon = FeatureRefs.GreatFortitude.Reference.Get().Icon;
        var maneuverIcon = FeatureRefs.IronWill.Reference.Get().Icon;
        var expandedIcon = AbilityRefs.FalseLife.Reference.Get().Icon;

        var trance = TranceHelper.BuildTrance(
            baseName: "Survivor",
            tranceFeatureGuid: Guids.SurvivorTrance,
            tranceBuffGuid: Guids.SurvivorTranceBuff,
            parentAbilityGuid: Guids.SurvivorPathParent,
            maneuverAbilityGuid: Guids.SurvivorManeuverAbility,
            expandedManeuverAbilityGuid: Guids.SurvivorExpandedAbility,
            displayName: "Survivor",
            featureDescription: "Your psionic focus toughens your body. You gain DR 2/— and Mettle (when you succeed on a Fortitude or Will saving throw against an effect that normally allows a partial save, you instead take no effect at all).",
            icon: icon,
            addBuffComponents: b =>
            {
                b.AddComponent(new AddDamageResistancePhysical { Value = 2, BypassedByMaterial = false });
                b.AddEvasion(SavingThrowType.Fortitude);
                b.AddEvasion(SavingThrowType.Will);
            });

        // Mettle: Fort/Will saves spike for 1 round
        var maneuverBuff = BuffConfigurator.New("SurvivorManeuverBuff", Guids.SurvivorManeuverBuff)
            .SetDisplayName(Loc.Str("PW.SurvivorManeuver.BuffName", "Survivor Maneuver"))
            .SetDescription(Loc.Str("PW.SurvivorManeuver.BuffDesc",
                "You steel yourself against magic and harm, gaining +4 competence to Fortitude and Will saves for 1 round."))
            .SetIcon(maneuverIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SaveFortitude, value: 4)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SaveWill, value: 4)
            .Configure();

        var maneuverAbility = AbilityConfigurator.New("SurvivorManeuver", Guids.SurvivorManeuverAbility)
            .SetDisplayName(Loc.Str("PW.SurvivorManeuverAb.Name", "Survivor Maneuver"))
            .SetDescription(Loc.Str("PW.SurvivorManeuverAb.Desc",
                "Swift Action. Expend psionic focus to steel yourself against magic and physical harm, gaining +4 competence to Fortitude and Will saves for 1 round."))
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

        // Expanded — Survivor's Resolve: temp HP equal to caster level for 1 minute
        var expandedBuff = BuffConfigurator.New("SurvivorExpandedManeuverBuff", Guids.SurvivorExpandedBuff)
            .SetDisplayName(Loc.Str("PW.SurvivorExpanded.BuffName", "Survivor's Resolve"))
            .SetDescription(Loc.Str("PW.SurvivorExpanded.BuffDesc",
                "Psionic resolve hardens flesh — temporary hit points equal to your manifester level."))
            .SetIcon(expandedIcon)
            .AddTemporaryHitPointsFromAbilityValue(
                descriptor: ModifierDescriptor.UntypedStackable,
                value: ContextValues.Rank())
            .Configure();

        var expandedAbility = AbilityConfigurator.New("SurvivorExpandedManeuverAbility", Guids.SurvivorExpandedAbility)
            .SetDisplayName(Loc.Str("PW.SurvivorExpandedAb.Name", "Survivor's Resolve"))
            .SetDescription(Loc.Str("PW.SurvivorExpandedAb.Desc",
                "Swift Action. Expend psionic focus to gain temporary hit points equal to your manifester level (1 minute)."))
            .SetIcon(maneuverIcon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .RemoveBuff(Guids.PsionicFocusBuff)
                    .ApplyBuff(expandedBuff, ContextDuration.Fixed(1, DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddAbilityShowIfCasterHasFact(not: false, unitFact: Guids.SurvivorExpandedFeature)
            .Configure();

        FeatureConfigurator.New("SurvivorExpandedManeuver", Guids.SurvivorExpandedFeature)
            .SetDisplayName(Loc.Str("PW.SurvivorExpandedFeat.Name", "Survivor's Resolve"))
            .SetDescription(Loc.Str("PW.SurvivorExpandedFeat.Desc",
                "You learn the Survivor's Resolve maneuver: a swift-action self-buff granting temporary hit points equal to your manifester level for 1 minute."))
            .SetIcon(expandedIcon)
            .SetIsClassFeature()
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerSurvivorExpanded)
            .AddPrerequisiteFeature(Guids.SurvivorPath)
            .Configure();

        FeatureConfigurator.New("SurvivorPath", Guids.SurvivorPath)
            .SetDisplayName(Loc.Str("PW.SurvivorPath.Name", "Survivor Path"))
            .SetDescription(Loc.Str("PW.SurvivorPath.Desc",
                "You focus on endurance and resilience. Your trance grants DR 2/— and Mettle (partial-effect Fort/Will saves are negated entirely on a success). Your maneuver lets you expend psionic focus to spike your Fortitude and Will saves."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFeatureOnClassLevel(feature: trance.ToString(), level: 3, clazz: Guids.PsychicWarriorClass)
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerSurvivorManeuver)
            .AddPrerequisiteNoFeature(Guids.SurvivorPath)
            .Configure();
    }
}
