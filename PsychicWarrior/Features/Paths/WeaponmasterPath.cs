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

public static class WeaponmasterPath
{
    public static void Configure()
    {
        var icon = FeatureRefs.PowerAttackFeature.Reference.Get().Icon;
        var maneuverIcon = FeatureRefs.WeaponSpecializationGreatsword.Reference.Get().Icon;
        var expandedIcon = AbilityRefs.ExpeditiousRetreat.Reference.Get().Icon;

        // Trance (and grouped path-powers parent) — see TranceHelper for shape
        var trance = TranceHelper.BuildTrance(
            baseName: "Weaponmaster",
            tranceFeatureGuid: Guids.WeaponmasterTrance,
            tranceBuffGuid: Guids.WeaponmasterTranceBuff,
            parentAbilityGuid: Guids.WeaponmasterPathParent,
            maneuverAbilityGuid: Guids.WeaponmasterManeuver,
            expandedManeuverAbilityGuid: Guids.WeaponmasterExpandedManeuverAbility,
            displayName: "Weaponmaster",
            featureDescription: "Beginning at 3rd level, you gain a +1 competence bonus to attack rolls made with manufactured weapons, increasing by 1 every four psychic warrior levels (+2 at 7th, +3 at 11th, +4 at 15th, +5 at 19th).",
            icon: icon,
            addBuffComponents: b => b.AddComponent(new WeaponmasterTranceAttackBonus()));

        // Maneuver buff: +2 dodge to AC + +2 competence to next attack (riposte spirit)
        var maneuverBuff = BuffConfigurator.New("WeaponmasterManeuverBuff", Guids.WeaponmasterManeuverBuff)
            .SetDisplayName(Loc.Str("PW.WMManeuver.BuffName", "Weaponmaster Maneuver"))
            .SetDescription(Loc.Str("PW.WMManeuver.BuffDesc",
                "You are in a counter-attack stance, gaining +2 dodge to AC and +2 competence to your next attack roll."))
            .SetIcon(maneuverIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.AC, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalAttackBonus, value: 2)
            .Configure();

        // Maneuver ability: swift, expend focus, apply buff 1 round
        var maneuverAbility = AbilityConfigurator.New("WeaponmasterManeuver", Guids.WeaponmasterManeuver)
            .SetDisplayName(Loc.Str("PW.WMManeuverAb.Name", "Weaponmaster Maneuver"))
            .SetDescription(Loc.Str("PW.WMManeuverAb.Desc",
                "Swift Action. Expend psionic focus to enter a counter-attack stance, gaining +2 dodge to AC and +2 competence to your next attack roll for 1 round."))
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

        // Expanded — Burst of Speed: +30 ft speed + +4 attack for 1 round (charge bonus)
        var expandedBuff = BuffConfigurator.New("WeaponmasterExpandedManeuverBuff", Guids.WeaponmasterExpandedManeuverBuff)
            .SetDisplayName(Loc.Str("PW.WMExpanded.BuffName", "Burst of Speed"))
            .SetDescription(Loc.Str("PW.WMExpanded.BuffDesc",
                "A psionic burst of momentum carries you forward: +30 ft speed and +4 competence to attack rolls."))
            .SetIcon(expandedIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: StatType.Speed, value: 30)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalAttackBonus, value: 4)
            .Configure();

        var expandedAbility = AbilityConfigurator.New("WeaponmasterExpandedManeuverAbility", Guids.WeaponmasterExpandedManeuverAbility)
            .SetDisplayName(Loc.Str("PW.WMExpandedAb.Name", "Burst of Speed"))
            .SetDescription(Loc.Str("PW.WMExpandedAb.Desc",
                "Swift Action. Expend psionic focus to unleash a burst of psionic momentum: +30 ft speed and +4 competence to attack rolls for 1 round."))
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
            .AddAbilityShowIfCasterHasFact(not: false, unitFact: Guids.WeaponmasterExpandedManeuver)
            .Configure();

        FeatureConfigurator.New("WeaponmasterExpandedManeuver", Guids.WeaponmasterExpandedManeuver)
            .SetDisplayName(Loc.Str("PW.WMExpandedFeat.Name", "Burst of Speed"))
            .SetDescription(Loc.Str("PW.WMExpandedFeat.Desc",
                "You learn the Burst of Speed maneuver: a swift-action self-buff granting +30 ft speed and +4 competence to attack rolls for 1 round."))
            .SetIcon(expandedIcon)
            .SetIsClassFeature()
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerWeaponmasterExpanded)
            .AddPrerequisiteFeature(Guids.WeaponmasterPath)
            .Configure();

        // Path: grants trance + maneuver
        FeatureConfigurator.New("WeaponmasterPath", Guids.WeaponmasterPath)
            .SetDisplayName(Loc.Str("PW.WeaponmasterPath.Name", "Weaponmaster Path"))
            .SetDescription(Loc.Str("PW.WeaponmasterPath.Desc",
                "You focus on martial superiority in melee. You gain a +1 competence bonus to attack rolls (trance) and can expend psionic focus to enter a counter-attack stance (maneuver)."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFeatureOnClassLevel(feature: trance.ToString(), level: 3, clazz: Guids.PsychicWarriorClass)
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerWeaponmasterManeuver)
            .AddPrerequisiteNoFeature(Guids.WeaponmasterPath)
            .Configure();
    }
}
