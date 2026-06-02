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
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features.Paths;

public static class DervishPath
{
    public static void Configure()
    {
        var icon = FeatureRefs.TwoWeaponFighting.Reference.Get().Icon;
        var maneuverIcon = FeatureRefs.TwoWeaponFightingImproved.Reference.Get().Icon;
        var expandedIcon = AbilityRefs.Haste.Reference.Get().Icon;

        var trance = TranceHelper.BuildTrance(
            baseName: "Dervish",
            tranceFeatureGuid: Guids.DervishTrance,
            tranceBuffGuid: Guids.DervishTranceBuff,
            parentAbilityGuid: Guids.DervishPathParent,
            maneuverAbilityGuid: Guids.DervishManeuverAbility,
            expandedManeuverAbilityGuid: Guids.DervishExpandedAbility,
            displayName: "Dervish",
            featureDescription: "Beginning at 3rd level, while maintaining psionic focus, you gain a +1 competence bonus to attack rolls when wielding two weapons. This bonus increases by 1 every four psychic warrior levels thereafter (+2 at 7th, +3 at 11th, +4 at 15th, +5 at 19th).",
            icon: icon,
            addBuffComponents: b => b.AddComponent(new DervishTranceAttackBonus()));

        var maneuverBuff = BuffConfigurator.New("DervishManeuverBuff", Guids.DervishManeuverBuff)
            .SetDisplayName(Loc.Str("PW.DervishManeuver.BuffName", "Dervish Maneuver"))
            .SetDescription(Loc.Str("PW.DervishManeuver.BuffDesc",
                "You enter a whirling combat stance, gaining +2 competence to attack rolls and +2 competence to damage rolls for 1 round."))
            .SetIcon(maneuverIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalAttackBonus, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalDamage, value: 2)
            .Configure();

        var maneuverAbility = AbilityConfigurator.New("DervishManeuver", Guids.DervishManeuverAbility)
            .SetDisplayName(Loc.Str("PW.DervishManeuverAb.Name", "Dervish Maneuver"))
            .SetDescription(Loc.Str("PW.DervishManeuverAb.Desc",
                "Swift Action. Expend psionic focus to enter a whirling combat stance, gaining +2 competence to attack and damage for 1 round."))
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

        // Expanded — Whirlwind Step: Haste-like spinning attack for 1 round
        var expandedBuff = BuffConfigurator.New("DervishExpandedManeuverBuff", Guids.DervishExpandedBuff)
            .SetDisplayName(Loc.Str("PW.DervishExpanded.BuffName", "Whirlwind Step"))
            .SetDescription(Loc.Str("PW.DervishExpanded.BuffDesc",
                "You spin in a deadly whirlwind: gain the benefits of haste (extra attack, +1 dodge AC, +1 Reflex, +30 ft speed)."))
            .SetIcon(expandedIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.AC, value: 1)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.SaveReflex, value: 1)
            .AddBuffMovementSpeed(descriptor: ModifierDescriptor.Enhancement, value: 30)
            .AddBuffExtraAttack(haste: true, number: 1)
            .Configure();

        var expandedAbility = AbilityConfigurator.New("DervishExpandedManeuverAbility", Guids.DervishExpandedAbility)
            .SetDisplayName(Loc.Str("PW.DervishExpandedAb.Name", "Whirlwind Step"))
            .SetDescription(Loc.Str("PW.DervishExpandedAb.Desc",
                "Swift Action. Expend psionic focus to enter a whirlwind dance: gain haste for 1 round (extra attack on full attacks, +1 dodge AC, +1 Reflex, +30 ft speed)."))
            .SetIcon(maneuverIcon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .RemoveBuff(Guids.PsionicFocusBuff)
                    .Add(new ContextActionLog { Message = "[DervishPath] Whirlwind Step: applying haste 1 round" })
                    .ApplyBuff(expandedBuff, ContextDuration.Fixed(1)))
            .AddAbilityShowIfCasterHasFact(not: false, unitFact: Guids.DervishExpandedFeature)
            .Configure();

        FeatureConfigurator.New("DervishExpandedManeuver", Guids.DervishExpandedFeature)
            .SetDisplayName(Loc.Str("PW.DervishExpandedFeat.Name", "Whirlwind Step"))
            .SetDescription(Loc.Str("PW.DervishExpandedFeat.Desc",
                "You learn the Whirlwind Step maneuver: a swift-action haste-like buff for 1 round."))
            .SetIcon(expandedIcon)
            .SetIsClassFeature()
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerDervishExpanded)
            .AddPrerequisiteFeature(Guids.DervishPath)
            .Configure();

        FeatureConfigurator.New("DervishPath", Guids.DervishPath)
            .SetDisplayName(Loc.Str("PW.DervishPath.Name", "Dervish Path"))
            .SetDescription(Loc.Str("PW.DervishPath.Desc",
                "You focus on swift dual-weapon mastery. You gain a +1 competence bonus to attack rolls (trance) and can expend psionic focus to enter a whirling combat stance (maneuver)."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFeatureOnClassLevel(feature: trance.ToString(), level: 3, clazz: Guids.PsychicWarriorClass)
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerDervishManeuver)
            .AddPrerequisiteNoFeature(Guids.DervishPath)
            .Configure();
    }
}
