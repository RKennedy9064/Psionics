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
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features.Paths;

public static class AsceticPath
{
    public static void Configure()
    {
        var icon = FeatureRefs.Mobility.Reference.Get().Icon;
        var maneuverIcon = AbilityRefs.ShieldOfFaith.Reference.Get().Icon;
        var expandedIcon = AbilityRefs.Aid.Reference.Get().Icon;

        var trance = TranceHelper.BuildTrance(
            baseName: "Ascetic",
            tranceFeatureGuid: Guids.AsceticTrance,
            tranceBuffGuid: Guids.AsceticTranceBuff,
            parentAbilityGuid: Guids.AsceticPathParent,
            maneuverAbilityGuid: Guids.AsceticManeuverAbility,
            expandedManeuverAbilityGuid: Guids.AsceticExpandedAbility,
            displayName: "Ascetic",
            featureDescription: "Beginning at 3rd level, your psionic discipline allows you to move and react with supernatural efficiency. You gain a +1 competence bonus to AC, increasing by 1 every four psychic warrior levels (+2 at 7th, +3 at 11th, +4 at 15th, +5 at 19th).",
            icon: icon,
            addBuffComponents: b =>
            {
                b.AddContextRankConfig(ContextRankConfigs.CasterLevel()
                    .WithCustomProgression((2, 0), (6, 1), (10, 2), (14, 3), (18, 4), (20, 5)));
                b.AddContextStatBonus(
                    stat: StatType.AC,
                    descriptor: ModifierDescriptor.Competence,
                    value: ContextValues.Rank());
            });

        var maneuverBuff = BuffConfigurator.New("AsceticManeuverBuff", Guids.AsceticManeuverBuff)
            .SetDisplayName(Loc.Str("PW.AsceticManeuver.BuffName", "Ascetic Maneuver"))
            .SetDescription(Loc.Str("PW.AsceticManeuver.BuffDesc",
                "You gain a +4 dodge bonus to AC for 1 round."))
            .SetIcon(maneuverIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.AC, value: 4)
            .Configure();

        var maneuverAbility = AbilityConfigurator.New("AsceticManeuver", Guids.AsceticManeuverAbility)
            .SetDisplayName(Loc.Str("PW.AsceticManeuverAb.Name", "Ascetic Maneuver"))
            .SetDescription(Loc.Str("PW.AsceticManeuverAb.Desc",
                "Swift Action. Expend psionic focus to enter a defensive stance, gaining +4 dodge to AC for 1 round."))
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

        // Expanded — Wholeness of Body: instant self-heal equal to caster level
        // (buff retained as a stub so its GUID still registers; not applied)
        BuffConfigurator.New("AsceticExpandedManeuverBuff", Guids.AsceticExpandedBuff)
            .SetDisplayName(Loc.Str("PW.AsceticExpanded.BuffName", "Wholeness of Body"))
            .SetDescription(Loc.Str("PW.AsceticExpanded.BuffDesc", "Psychometabolic self-healing."))
            .SetIcon(expandedIcon)
            .Configure();

        var expandedAbility = AbilityConfigurator.New("AsceticExpandedManeuverAbility", Guids.AsceticExpandedAbility)
            .SetDisplayName(Loc.Str("PW.AsceticExpandedAb.Name", "Wholeness of Body"))
            .SetDescription(Loc.Str("PW.AsceticExpandedAb.Desc",
                "Swift Action. Expend psionic focus to heal yourself for hit points equal to your manifester level."))
            .SetIcon(maneuverIcon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .RemoveBuff(Guids.PsionicFocusBuff)
                    .Add(new ContextActionLog { Message = "[WholenessOfBody] healing manifester level HP", LogRank = true })
                    .HealTarget(value: ContextDice.Value(DiceType.One, ContextValues.Rank(), 0)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddAbilityShowIfCasterHasFact(not: false, unitFact: Guids.AsceticExpandedFeature)
            .Configure();

        FeatureConfigurator.New("AsceticExpandedManeuver", Guids.AsceticExpandedFeature)
            .SetDisplayName(Loc.Str("PW.AsceticExpandedFeat.Name", "Wholeness of Body"))
            .SetDescription(Loc.Str("PW.AsceticExpandedFeat.Desc",
                "You learn the Wholeness of Body maneuver: a swift action that heals you for hit points equal to your manifester level."))
            .SetIcon(expandedIcon)
            .SetIsClassFeature()
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerAsceticExpanded)
            .AddPrerequisiteFeature(Guids.AsceticPath)
            .Configure();

        FeatureConfigurator.New("AsceticPath", Guids.AsceticPath)
            .SetDisplayName(Loc.Str("PW.AsceticPath.Name", "Ascetic Path"))
            .SetDescription(Loc.Str("PW.AsceticPath.Desc",
                "You focus on psionic body mastery. You gain a +1 competence bonus to AC (trance) and can expend psionic focus to adopt a defensive stance (maneuver)."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFeatureOnClassLevel(feature: trance.ToString(), level: 3, clazz: Guids.PsychicWarriorClass)
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerAsceticManeuver)
            .AddPrerequisiteNoFeature(Guids.AsceticPath)
            .Configure();
    }
}
