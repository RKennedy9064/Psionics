using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.BladeSkills;

/// <summary>
/// Blade skill: Focused Offense. While maintaining psionic focus, add your Wisdom modifier as an
/// insight bonus to attack and weapon damage rolls. Implemented as a focus-gated buff (mirrors
/// Focused Defense / Psionic Dodge) so it appears as a visible condition with a portrait icon,
/// rather than a silent rule-handler component.
/// </summary>
public static class FocusedOffense
{
    public static void Configure()
    {
        BuffConfigurator.New("SKFocusedOffenseBuff", Guids.BladeSkillFocusedOffenseBuff)
            .SetDisplayName(Loc.Str("SK.FocusedOffense.Name", "Focused Offense"))
            .SetDescription(Loc.Str("SK.FocusedOffense.BuffDesc",
                "While maintaining psionic focus, you add your Wisdom modifier to your attack and weapon damage rolls."))
            .SetIcon(FeatureRefs.WeaponSpecializationGreatsword.Reference.Get().Icon)
            .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom))
            .AddContextStatBonus(StatType.AdditionalAttackBonus, ContextValues.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
            .AddContextStatBonus(StatType.AdditionalDamage, ContextValues.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
            .Configure();

        FeatureConfigurator.New("SKFocusedOffense", Guids.BladeSkillFocusedOffense)
            .SetDisplayName(Loc.Str("SK.FocusedOffense.Name", "Focused Offense"))
            .SetDescription(Loc.Str("SK.FocusedOffense.Desc",
                "While maintaining psionic focus, you add your Wisdom modifier to your attack rolls and damage rolls instead of your Strength modifier."))
            .SetIcon(FeatureRefs.WeaponSpecializationGreatsword.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddFactContextActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.BladeSkillFocusedOffenseBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.BladeSkillFocusedOffenseBuff))
            .Configure();

        BuffConfigurator.For(Guids.PsionicFocusBuff)
            .AddBuffActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.BladeSkillFocusedOffense),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.BladeSkillFocusedOffenseBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.BladeSkillFocusedOffenseBuff))
            .Configure();
    }
}
