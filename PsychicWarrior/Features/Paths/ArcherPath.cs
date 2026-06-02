using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features.Paths;

public static class ArcherPath
{
    public static void Configure()
    {
        var icon = FeatureRefs.RapidShotFeature.Reference.Get().Icon;
        var maneuverIcon = FeatureRefs.Manyshot.Reference.Get().Icon;
        var expandedIcon = AbilityRefs.Haste.Reference.Get().Icon;

        var trance = TranceHelper.BuildTrance(
            baseName: "Archer",
            tranceFeatureGuid: Guids.ArcherTrance,
            tranceBuffGuid: Guids.ArcherTranceBuff,
            parentAbilityGuid: Guids.ArcherPathParent,
            maneuverAbilityGuid: Guids.ArcherManeuverAbility,
            expandedManeuverAbilityGuid: Guids.ArcherExpandedAbility,
            displayName: "Archer",
            featureDescription: "Beginning at 3rd level, your psionic focus steadies your aim. You gain a +1 competence bonus to attack rolls made with ranged or thrown weapons (natural weapons do not count), increasing by 1 every four psychic warrior levels (+2 at 7th, +3 at 11th, +4 at 15th, +5 at 19th).",
            icon: icon,
            addBuffComponents: b =>
            {
                b.AddContextRankConfig(ContextRankConfigs.CasterLevel()
                    .WithCustomProgression((2, 0), (6, 1), (10, 2), (14, 3), (18, 4), (20, 5)));
                b.AddComponent(new AttackTypeAttackBonus
                {
                    Type = WeaponRangeType.Ranged,
                    AttackBonus = 0,
                    Descriptor = ModifierDescriptor.Competence,
                    Value = new ContextValue { ValueType = ContextValueType.Rank }
                });
            });

        var maneuverBuff = BuffConfigurator.New("ArcherManeuverBuff", Guids.ArcherManeuverBuff)
            .SetDisplayName(Loc.Str("PW.ArcherManeuver.BuffName", "Archer Maneuver"))
            .SetDescription(Loc.Str("PW.ArcherManeuver.BuffDesc",
                "Your next attack gains a +4 competence bonus to the attack roll."))
            .SetIcon(maneuverIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalAttackBonus, value: 4)
            .AddInitiatorAttackRollTrigger(ActionsBuilder.New().RemoveBuff(Guids.ArcherManeuverBuff))
            .Configure();

        var maneuverAbility = AbilityConfigurator.New("ArcherManeuver", Guids.ArcherManeuverAbility)
            .SetDisplayName(Loc.Str("PW.ArcherManeuverAb.Name", "Archer Maneuver"))
            .SetDescription(Loc.Str("PW.ArcherManeuverAb.Desc",
                "Swift Action. Expend psionic focus to channel precision into your next attack, gaining a +4 competence bonus to the attack roll."))
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

        // Expanded — Twin Shot: Haste-like acceleration for 1 round
        var expandedBuff = BuffConfigurator.New("ArcherExpandedManeuverBuff", Guids.ArcherExpandedBuff)
            .SetDisplayName(Loc.Str("PW.ArcherExpanded.BuffName", "Twin Shot"))
            .SetDescription(Loc.Str("PW.ArcherExpanded.BuffDesc",
                "Psionic acceleration drives your arms: gain the benefits of haste — extra attack on full attacks, +1 dodge AC, +1 Reflex, +30 ft speed."))
            .SetIcon(expandedIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.AC, value: 1)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.SaveReflex, value: 1)
            .AddBuffMovementSpeed(descriptor: ModifierDescriptor.Enhancement, value: 30)
            .AddBuffExtraAttack(haste: true, number: 1)
            .Configure();

        var expandedAbility = AbilityConfigurator.New("ArcherExpandedManeuverAbility", Guids.ArcherExpandedAbility)
            .SetDisplayName(Loc.Str("PW.ArcherExpandedAb.Name", "Twin Shot"))
            .SetDescription(Loc.Str("PW.ArcherExpandedAb.Desc",
                "Swift Action. Expend psionic focus to unleash a Twin Shot: gain the benefits of haste for 1 round (extra attack on full attacks, +1 dodge AC, +1 Reflex, +30 ft speed)."))
            .SetIcon(maneuverIcon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .RemoveBuff(Guids.PsionicFocusBuff)
                    .Add(new ContextActionLog { Message = "[ArcherPath] Twin Shot: applying haste 1 round" })
                    .ApplyBuff(expandedBuff, ContextDuration.Fixed(1)))
            .AddAbilityShowIfCasterHasFact(not: false, unitFact: Guids.ArcherExpandedFeature)
            .Configure();

        FeatureConfigurator.New("ArcherExpandedManeuver", Guids.ArcherExpandedFeature)
            .SetDisplayName(Loc.Str("PW.ArcherExpandedFeat.Name", "Twin Shot"))
            .SetDescription(Loc.Str("PW.ArcherExpandedFeat.Desc",
                "You learn the Twin Shot maneuver: a swift-action haste-like buff for 1 round."))
            .SetIcon(expandedIcon)
            .SetIsClassFeature()
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerArcherExpanded)
            .AddPrerequisiteFeature(Guids.ArcherPath)
            .Configure();

        FeatureConfigurator.New("ArcherPath", Guids.ArcherPath)
            .SetDisplayName(Loc.Str("PW.ArcherPath.Name", "Archer Path"))
            .SetDescription(Loc.Str("PW.ArcherPath.Desc",
                "You focus on ranged precision. You gain a +1 competence bonus to attack rolls (trance) and can expend psionic focus to enhance your next attack (maneuver)."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFeatureOnClassLevel(feature: trance.ToString(), level: 3, clazz: Guids.PsychicWarriorClass)
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerArcherManeuver)
            .AddPrerequisiteNoFeature(Guids.ArcherPath)
            .Configure();
    }
}
