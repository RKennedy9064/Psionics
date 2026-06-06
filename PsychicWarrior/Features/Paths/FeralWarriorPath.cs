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

public static class FeralWarriorPath
{
    public static void Configure()
    {
        var icon = AbilityRefs.Rage.Reference.Get().Icon;
        var maneuverIcon = AbilityRefs.MagicFang.Reference.Get().Icon;
        var expandedIcon = FeatureRefs.ImprovedCriticalLongsword.Reference.Get().Icon;

        var trance = TranceHelper.BuildTrance(
            baseName: "FeralWarrior",
            tranceFeatureGuid: Guids.FeralWarriorTrance,
            tranceBuffGuid: Guids.FeralWarriorTranceBuff,
            parentAbilityGuid: Guids.FeralWarriorPathParent,
            maneuverAbilityGuid: Guids.FeralWarriorManeuverAbility,
            expandedManeuverAbilityGuid: Guids.FeralWarriorExpandedAbility,
            displayName: "Feral Warrior",
            featureDescription: "Beginning at 3rd level, your psionic focus channels through your body's natural weapons. You gain a +1 competence bonus to attack rolls made with natural weapons, increasing by 1 every four psychic warrior levels (+2 at 7th, +3 at 11th, +4 at 15th, +5 at 19th).",
            icon: icon,
            addBuffComponents: b => b.AddComponent(new FeralWarriorTranceAttackBonus()));

        // +7 ≈ 2d6 average; removes after next attack
        var maneuverBuff = BuffConfigurator.New("FeralWarriorManeuverBuff", Guids.FeralWarriorManeuverBuff)
            .SetDisplayName(Loc.Str("PW.FeralWarriorManeuver.BuffName", "Feral Warrior Maneuver"))
            .SetDescription(Loc.Str("PW.FeralWarriorManeuver.BuffDesc",
                "Your next attack deals +7 bonus damage channeled through your feral psionic energy."))
            .SetIcon(maneuverIcon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalDamage, value: 7)
            .AddInitiatorAttackRollTrigger(ActionsBuilder.New().RemoveBuff(Guids.FeralWarriorManeuverBuff))
            .Configure();

        var maneuverAbility = AbilityConfigurator.New("FeralWarriorManeuver", Guids.FeralWarriorManeuverAbility)
            .SetDisplayName(Loc.Str("PW.FeralWarriorManeuverAb.Name", "Feral Warrior Maneuver"))
            .SetDescription(Loc.Str("PW.FeralWarriorManeuverAb.Desc",
                "Swift Action. Expend psionic focus to channel feral energy into your next strike, dealing +7 bonus damage."))
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

        // Expanded � Feral Rend: keen weapon edge (doubled threat range) for 1 round
        var expandedBuff = BuffConfigurator.New("FeralWarriorExpandedManeuverBuff", Guids.FeralWarriorExpandedBuff)
            .SetDisplayName(Loc.Str("PW.FeralWarriorExpanded.BuffName", "Feral Rend"))
            .SetDescription(Loc.Str("PW.FeralWarriorExpanded.BuffDesc",
                "Your weapons take on a feral edge � doubled critical threat range."))
            .SetIcon(expandedIcon)
            .AddWeaponCriticalEdgeIncreaseStackable(value: 1)
            .Configure();

        var expandedAbility = AbilityConfigurator.New("FeralWarriorExpandedManeuverAbility", Guids.FeralWarriorExpandedAbility)
            .SetDisplayName(Loc.Str("PW.FeralWarriorExpandedAb.Name", "Feral Rend"))
            .SetDescription(Loc.Str("PW.FeralWarriorExpandedAb.Desc",
                "Swift Action. Expend psionic focus to imbue your weapons with feral edge � doubled critical threat range for 1 round."))
            .SetIcon(maneuverIcon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .RemoveBuff(Guids.PsionicFocusBuff)
                    .Add(new ContextActionLog { Message = "[FeralWarriorPath] Feral Rend: applying keen crit edge 1 round" })
                    .ApplyBuff(expandedBuff, ContextDuration.Fixed(1)))
            .AddAbilityShowIfCasterHasFact(not: false, unitFact: Guids.FeralWarriorExpandedFeature)
            .Configure();

        FeatureConfigurator.New("FeralWarriorExpandedManeuver", Guids.FeralWarriorExpandedFeature)
            .SetDisplayName(Loc.Str("PW.FeralWarriorExpandedFeat.Name", "Feral Rend"))
            .SetDescription(Loc.Str("PW.FeralWarriorExpandedFeat.Desc",
                "You learn the Feral Rend maneuver: a swift-action self-buff that imbues your weapons with feral edge � doubled critical threat range for 1 round."))
            .SetIcon(expandedIcon)
            .SetIsClassFeature()
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerFeralWarriorExpanded)
            .AddPrerequisiteFeature(Guids.FeralWarriorPath)
            .Configure();

        FeatureConfigurator.New("FeralWarriorPath", Guids.FeralWarriorPath)
            .SetDisplayName(Loc.Str("PW.FeralWarriorPath.Name", "Feral Warrior Path"))
            .SetDescription(Loc.Str("PW.FeralWarriorPath.Desc",
                "You channel psionic power through your body's natural weapons.\n" +
                "\n" +
                "Trance (3rd level): while focused, gain a +1 competence bonus to attack rolls with natural weapons, increasing by +1 every four levels (+2 at 7th, +3 at 11th, +4 at 15th, +5 at 19th).\n" +
                "\n" +
                "Maneuver: expend psionic focus (swift action) to channel feral energy into your next strike — +7 bonus damage.\n" +
                "\n" +
                "Expanded Maneuver (Feral Edge): expend psionic focus to imbue your natural weapons with a feral edge — doubled critical threat range for 1 round.\n" +
                "\n" +
                "A second path can be chosen at 9th level."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFeatureOnClassLevel(feature: trance.ToString(), level: 3, clazz: Guids.PsychicWarriorClass)
            .AddFeatureIfHasFact(checkedFact: Guids.MartialPowerFeature, feature: Guids.MartialPowerFeralWarriorManeuver)
            .AddPrerequisiteNoFeature(Guids.FeralWarriorPath)
            .Configure();
    }
}
