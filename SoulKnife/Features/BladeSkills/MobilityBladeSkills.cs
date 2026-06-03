using System.Linq;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.BladeSkills;

/// <summary>
/// Mobility blade skills: Ghost Step and Cleave Space (focus-fueled teleports, reusing the base
/// game's Dimension Door teleport component) and Reaching Blade (a toggle that doubles reach).
/// </summary>
public static class MobilityBladeSkills
{
    public static void Configure()
    {
        BuildTeleport("SKGhostStep", Guids.BladeSkillGhostStep, Guids.BladeSkillGhostStepAbility,
            "Ghost Step", "You teleport a short distance (close range) by expending your psionic focus. This movement does not provoke attacks of opportunity.",
            AbilityRange.Close, 4, prerequisiteFeature: null);

        BuildTeleport("SKCleaveSpace", Guids.BladeSkillCleaveSpace, Guids.BladeSkillCleaveSpaceAbility,
            "Cleave Space", "You cut through reality itself, teleporting a medium distance by expending your psionic focus. This movement does not provoke attacks of opportunity.",
            AbilityRange.Medium, 10, prerequisiteFeature: Guids.BladeSkillGhostStep);

        BuildReachingBlade();
        BuildExtendedStrike();
    }

    // Extended Strike — a brief burst of extended reach (1 round). Reuses the reach-doubling buff.
    private static void BuildExtendedStrike()
    {
        var buff = BuffConfigurator.New("SKExtendedStrikeBuff", Guids.BladeSkillExtendedStrikeBuff)
            .SetDisplayName(Loc.Str("SK.ExtendedStrike.Name", "Extended Strike"))
            .SetDescription(Loc.Str("SK.ExtendedStrike.BuffDesc",
                "Your mind blade extends, doubling your natural reach until the start of your next turn."))
            .SetIcon(AbilityRefs.EnlargePerson.Reference.Get().Icon)
            .AddComponent(new ReachMultiplicator { m_Multiplicator = 2, m_Descriptor = ModifierDescriptor.UntypedStackable })
            .Configure();

        AbilityConfigurator.New("SKExtendedStrikeAbility", Guids.BladeSkillExtendedStrikeAbility)
            .SetDisplayName(Loc.Str("SK.ExtendedStrike.Name", "Extended Strike"))
            .SetDescription(Loc.Str("SK.ExtendedStrike.AbDesc",
                "Lengthen your mind blade, doubling your natural reach until the start of your next turn so you can strike a distant foe."))
            .SetIcon(AbilityRefs.EnlargePerson.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)))
            .Configure();

        FeatureConfigurator.New("SKExtendedStrike", Guids.BladeSkillExtendedStrike)
            .SetDisplayName(Loc.Str("SK.ExtendedStrike.Name", "Extended Strike"))
            .SetDescription(Loc.Str("SK.ExtendedStrike.Desc",
                "As a swift action, you can lengthen your mind blade to double your natural reach until the start of your next turn."))
            .SetIcon(AbilityRefs.EnlargePerson.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddFacts([Guids.BladeSkillExtendedStrikeAbility])
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 12)
            .Configure();
    }

    private static void BuildTeleport(string name, string featureGuid, string abilityGuid,
        string displayName, string description, AbilityRange range, int minLevel, string prerequisiteFeature)
    {
        AbilityConfigurator.New(name + "Ability", abilityGuid)
            .SetDisplayName(Loc.Str($"SK.{name}.Name", displayName))
            .SetDescription(Loc.Str($"SK.{name}.Desc", description))
            .SetIcon(AbilityRefs.Blink.Reference.Get().Icon)
            .SetType(AbilityType.SpellLike)
            .SetRange(range)
            .SetActionType(UnitCommand.CommandType.Move)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts(facts: [BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(ActionsBuilder.New().RemoveBuff(Guids.PsionicFocusBuff))
            .OnConfigure(bp =>
            {
                // Reuse the base game's Dimension Door teleport behaviour.
                var teleport = AbilityRefs.DimensionDoor.Reference.Get()
                    .ComponentsArray.OfType<AbilityCustomDimensionDoor>().FirstOrDefault();
                if (teleport != null)
                    bp.ComponentsArray = [.. bp.ComponentsArray, teleport];
            })
            .Configure();

        var feature = FeatureConfigurator.New(name, featureGuid)
            .SetDisplayName(Loc.Str($"SK.{name}.Name", displayName))
            .SetDescription(Loc.Str($"SK.{name}.Desc", description))
            .SetIcon(AbilityRefs.Blink.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddFacts([abilityGuid])
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, minLevel);

        if (prerequisiteFeature != null)
            feature = feature.AddPrerequisiteFeature(prerequisiteFeature);

        feature.Configure();
    }

    private static void BuildReachingBlade()
    {
        var buff = BuffConfigurator.New("SKReachingBladeBuff", Guids.BladeSkillReachingBladeBuff)
            .SetDisplayName(Loc.Str("SK.ReachingBlade.Name", "Reaching Blade"))
            .SetDescription(Loc.Str("SK.ReachingBlade.BuffDesc",
                "Your mind blade extends, doubling your natural reach so you can strike foes at a distance."))
            .SetIcon(AbilityRefs.EnlargePerson.Reference.Get().Icon)
            .AddComponent(new ReachMultiplicator { m_Multiplicator = 2, m_Descriptor = ModifierDescriptor.UntypedStackable })
            .Configure();

        var toggle = ActivatableAbilityConfigurator.New("SKReachingBladeToggle", Guids.BladeSkillReachingBladeToggle)
            .SetDisplayName(Loc.Str("SK.ReachingBlade.Name", "Reaching Blade"))
            .SetDescription(Loc.Str("SK.ReachingBlade.ToggleDesc",
                "Toggle. Your mind blade extends, doubling your natural reach so you can strike foes at a distance."))
            .SetIcon(AbilityRefs.EnlargePerson.Reference.Get().Icon)
            .SetBuff(buff)
            .SetActivationType(AbilityActivationType.Immediately)
            .SetDeactivateImmediately(true)
            .SetGroup(ActivatableAbilityGroup.None)
            .Configure();

        FeatureConfigurator.New("SKReachingBlade", Guids.BladeSkillReachingBlade)
            .SetDisplayName(Loc.Str("SK.ReachingBlade.Name", "Reaching Blade"))
            .SetDescription(Loc.Str("SK.ReachingBlade.Desc",
                "You can extend your mind blade to double your natural reach, letting you strike foes at a distance."))
            .SetIcon(AbilityRefs.EnlargePerson.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddFacts([toggle.AssetGuid.ToString()])
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 8)
            .Configure();
    }
}
