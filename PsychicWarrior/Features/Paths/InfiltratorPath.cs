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

public static class InfiltratorPath
{
    public static void Configure()
    {
        var icon = AbilityRefs.CauseFear.Reference.Get().Icon;
        var maneuverIcon = FeatureRefs.SkillFocusDiplomacy.Reference.Get().Icon;
        var expandedIcon = AbilityRefs.Blur.Reference.Get().Icon;

        var trance = TranceHelper.BuildTrance(
            baseName: "Infiltrator",
            tranceFeatureGuid: Guids.InfiltratorTrance,
            tranceBuffGuid: Guids.InfiltratorTranceBuff,
            parentAbilityGuid: Guids.InfiltratorPathParent,
            maneuverAbilityGuid: Guids.InfiltratorManeuverAbility,
            expandedManeuverAbilityGuid: Guids.InfiltratorExpandedAbility,
            displayName: "Infiltrator",
            featureDescription: "Beginning at 3rd level, your psionic focus makes you more effective at social manipulation and precision strikes. You gain a +2 competence bonus to Persuasion and +1 competence bonus to damage rolls, each increasing by 1 every four psychic warrior levels.",
            icon: icon,
            addBuffComponents: b =>
            {
                b.AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SkillPersuasion, value: 1);
                b.AddContextRankConfig(ContextRankConfigs.CasterLevel()
                    .WithCustomProgression((2, 0), (6, 1), (10, 2), (14, 3), (18, 4), (20, 5)));
                b.AddContextStatBonus(stat: StatType.SkillPersuasion, descriptor: ModifierDescriptor.Competence, value: ContextValues.Rank());
                b.AddContextStatBonus(stat: StatType.AdditionalDamage, descriptor: ModifierDescriptor.Competence, value: ContextValues.Rank());
            });

        var maneuverBuff = BuffConfigurator.New("InfiltratorManeuverBuff", Guids.InfiltratorManeuverBuff)
            .SetDisplayName(Loc.Str("PW.InfiltratorManeuver.BuffName", "Infiltrator Maneuver"))
            .SetDescription(Loc.Str("PW.InfiltratorManeuver.BuffDesc",
                "Your psionic aura radiates menace. You gain +4 competence to Persuasion and +2 competence to damage for 1 round."))
            .SetIcon(maneuverIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SkillPersuasion, value: 4)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalDamage, value: 2)
            .Configure();

        var maneuverAbility = AbilityConfigurator.New("InfiltratorManeuver", Guids.InfiltratorManeuverAbility)
            .SetDisplayName(Loc.Str("PW.InfiltratorManeuverAb.Name", "Infiltrator Maneuver"))
            .SetDescription(Loc.Str("PW.InfiltratorManeuverAb.Desc",
                "Swift Action. Expend psionic focus to project a menacing aura, gaining +4 competence to Persuasion and +2 competence to damage for 1 round."))
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

        // Expanded — Hidden Step: +20 speed (enhancement) + +6 stealth for 1 round
        var expandedBuff = BuffConfigurator.New("InfiltratorExpandedManeuverBuff", Guids.InfiltratorExpandedBuff)
            .SetDisplayName(Loc.Str("PW.InfiltratorExpanded.BuffName", "Hidden Step"))
            .SetDescription(Loc.Str("PW.InfiltratorExpanded.BuffDesc",
                "Psionic shadow-step: +20 ft enhancement speed and +6 competence to Stealth for 1 round."))
            .SetIcon(expandedIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: StatType.Speed, value: 20)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SkillStealth, value: 6)
            .Configure();

        var expandedAbility = AbilityConfigurator.New("InfiltratorExpandedManeuverAbility", Guids.InfiltratorExpandedAbility)
            .SetDisplayName(Loc.Str("PW.InfiltratorExpandedAb.Name", "Hidden Step"))
            .SetDescription(Loc.Str("PW.InfiltratorExpandedAb.Desc",
                "Swift Action. Expend psionic focus to glide as a shadow: +20 ft enhancement speed and +6 competence to Stealth for 1 round."))
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
            .AddAbilityShowIfCasterHasFact(not: false, unitFact: Guids.InfiltratorExpandedFeature)
            .Configure();

        FeatureConfigurator.New("InfiltratorExpandedManeuver", Guids.InfiltratorExpandedFeature)
            .SetDisplayName(Loc.Str("PW.InfiltratorExpandedFeat.Name", "Hidden Step"))
            .SetDescription(Loc.Str("PW.InfiltratorExpandedFeat.Desc",
                "You learn the Hidden Step maneuver: a swift-action self-buff granting +20 ft speed and +6 competence to Stealth for 1 round."))
            .SetIcon(expandedIcon)
            .SetIsClassFeature()
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerInfiltratorExpanded)
            .AddPrerequisiteFeature(Guids.InfiltratorPath)
            .Configure();

        FeatureConfigurator.New("InfiltratorPath", Guids.InfiltratorPath)
            .SetDisplayName(Loc.Str("PW.InfiltratorPath.Name", "Infiltrator Path"))
            .SetDescription(Loc.Str("PW.InfiltratorPath.Desc",
                "You focus on deception and precise lethal strikes. You gain +2 competence to Persuasion and +1 to damage (trance), and can expend psionic focus for enhanced menace (maneuver)."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFeatureOnClassLevel(feature: trance.ToString(), level: 3, clazz: Guids.PsychicWarriorClass)
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerInfiltratorManeuver)
            .AddPrerequisiteNoFeature(Guids.InfiltratorPath)
            .Configure();
    }
}
