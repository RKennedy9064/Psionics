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
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features.Paths;

public static class BrawlerPath
{
    public static void Configure()
    {
        var icon = FeatureRefs.ImprovedBullRush.Reference.Get().Icon;
        var maneuverIcon = AbilityRefs.BullsStrength.Reference.Get().Icon;
        var expandedIcon = FeatureRefs.Toughness.Reference.Get().Icon;

        // Trance + grouped path-powers parent
        var trance = TranceHelper.BuildTrance(
            baseName: "Brawler",
            tranceFeatureGuid: Guids.BrawlerTrance,
            tranceBuffGuid: Guids.BrawlerTranceBuff,
            parentAbilityGuid: Guids.BrawlerPathParent,
            maneuverAbilityGuid: Guids.BrawlerManeuver,
            expandedManeuverAbilityGuid: Guids.BrawlerExpandedManeuverAbility,
            displayName: "Brawler",
            featureDescription: "Your Wisdom modifier is added as a competence bonus to your Combat Maneuver Bonus, reflecting your psionic insight in grappling and close combat.",
            icon: icon,
            addBuffComponents: b => b
                .AddContextStatBonus(
                    stat: StatType.AdditionalCMB,
                    descriptor: ModifierDescriptor.Competence,
                    value: new ContextValue { ValueType = ContextValueType.Rank })
                .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom)));

        // Maneuver buff: +7 competence to damage (≈2d6) on next attack, then removes itself
        var maneuverBuff = BuffConfigurator.New("BrawlerManeuverBuff", Guids.BrawlerManeuverBuff)
            .SetDisplayName(Loc.Str("PW.BrawlerManeuver.BuffName", "Brawler Maneuver"))
            .SetDescription(Loc.Str("PW.BrawlerManeuver.BuffDesc",
                "Your next attack deals +7 bonus damage from psionic energy channeled through your strike."))
            .SetIcon(maneuverIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalDamage, value: 7)
            .AddInitiatorAttackRollTrigger(ActionsBuilder.New().RemoveBuff(Guids.BrawlerManeuverBuff))
            .Configure();

        var maneuverAbility = AbilityConfigurator.New("BrawlerManeuver", Guids.BrawlerManeuver)
            .SetDisplayName(Loc.Str("PW.BrawlerManeuverAb.Name", "Brawler Maneuver"))
            .SetDescription(Loc.Str("PW.BrawlerManeuverAb.Desc",
                "Swift Action. Expend psionic focus to channel energy into your next strike, dealing +7 bonus damage on hit."))
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

        // Expanded � Steel Sinews: counter-attack reflex, +4 dodge AC + Fast Healing 2 for 1 round
        var expandedBuff = BuffConfigurator.New("BrawlerExpandedManeuverBuff", Guids.BrawlerExpandedManeuverBuff)
            .SetDisplayName(Loc.Str("PW.BrawlerExpanded.BuffName", "Steel Sinews"))
            .SetDescription(Loc.Str("PW.BrawlerExpanded.BuffDesc",
                "Your sinews harden against blows. You gain +4 dodge bonus to AC and Fast Healing 2 for 1 round."))
            .SetIcon(expandedIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.AC, value: 4)
            .AddEffectFastHealing(heal: 2)
            .Configure();

        var expandedAbility = AbilityConfigurator.New("BrawlerExpandedManeuverAbility", Guids.BrawlerExpandedManeuverAbility)
            .SetDisplayName(Loc.Str("PW.BrawlerExpandedAb.Name", "Steel Sinews"))
            .SetDescription(Loc.Str("PW.BrawlerExpandedAb.Desc",
                "Swift Action. Expend psionic focus to harden your sinews. Gain +4 dodge bonus to AC and Fast Healing 2 for 1 round."))
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
            .AddAbilityShowIfCasterHasFact(not: false, unitFact: Guids.BrawlerExpandedManeuver)
            .Configure();

        FeatureConfigurator.New("BrawlerExpandedManeuver", Guids.BrawlerExpandedManeuver)
            .SetDisplayName(Loc.Str("PW.BrawlerExpandedFeat.Name", "Steel Sinews"))
            .SetDescription(Loc.Str("PW.BrawlerExpandedFeat.Desc",
                "You learn the Steel Sinews maneuver: a swift-action self-buff that grants +4 dodge AC and Fast Healing 2 for 1 round."))
            .SetIcon(expandedIcon)
            .SetIsClassFeature()
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerBrawlerExpanded)
            .AddPrerequisiteFeature(Guids.BrawlerPath)
            .Configure();

        FeatureConfigurator.New("BrawlerPath", Guids.BrawlerPath)
            .SetDisplayName(Loc.Str("PW.BrawlerPath.Name", "Brawler Path"))
            .SetDescription(Loc.Str("PW.BrawlerPath.Desc",
                "You overpower foes with grapples and close combat.\n" +
                "\n" +
                "Trance (3rd level): while focused, add your Wisdom modifier as a competence bonus to your Combat Maneuver Bonus.\n" +
                "\n" +
                "Maneuver: expend psionic focus (swift action) to channel energy into your next strike — +7 bonus damage on hit.\n" +
                "\n" +
                "Expanded Maneuver: expend psionic focus to harden your sinews — +4 dodge AC and Fast Healing 2 for 1 round.\n" +
                "\n" +
                "A second path can be chosen at 9th level."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFeatureOnClassLevel(feature: trance.ToString(), level: 3, clazz: Guids.PsychicWarriorClass)
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerBrawlerManeuver)
            .AddPrerequisiteNoFeature(Guids.BrawlerPath)
            .Configure();
    }
}
