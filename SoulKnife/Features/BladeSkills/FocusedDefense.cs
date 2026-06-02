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
/// Blade skill: Focused Defense. While maintaining psionic focus, add your Wisdom modifier as a
/// dodge bonus to AC. Implemented as a focus-gated buff (mirrors the Psionic Dodge feat) so the
/// bonus is a real stat modifier visible on the character sheet — a <c>RuleCalculateAC</c> handler
/// only applies during an incoming attack and never shows in the AC panel.
/// </summary>
public static class FocusedDefense
{
    public static void Configure()
    {
        // The AC buff itself: Wisdom modifier as a dodge bonus to AC.
        BuffConfigurator.New("SKFocusedDefenseBuff", Guids.BladeSkillFocusedDefenseBuff)
            .SetDisplayName(Loc.Str("SK.FocusedDefense.Name", "Focused Defense"))
            .SetDescription(Loc.Str("SK.FocusedDefense.BuffDesc",
                "While maintaining psionic focus, you add your Wisdom modifier as a dodge bonus to your Armor Class."))
            .SetIcon(FeatureRefs.Dodge.Reference.Get().Icon)
            .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom))
            .AddContextStatBonus(StatType.AC, ContextValues.Rank(), descriptor: ModifierDescriptor.Dodge)
            .Configure();

        // The blade skill feature. If focus is already up when taken, apply the buff immediately;
        // remove it if the feature is somehow lost.
        FeatureConfigurator.New("SKFocusedDefense", Guids.BladeSkillFocusedDefense)
            .SetDisplayName(Loc.Str("SK.FocusedDefense.Name", "Focused Defense"))
            .SetDescription(Loc.Str("SK.FocusedDefense.Desc",
                "While maintaining psionic focus, you add your Wisdom modifier as a dodge bonus to your Armor Class."))
            .SetIcon(FeatureRefs.Dodge.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 4)
            .AddFactContextActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.BladeSkillFocusedDefenseBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.BladeSkillFocusedDefenseBuff))
            .Configure();

        // Apply/remove the AC buff alongside psionic focus (only if the soulknife has the skill).
        BuffConfigurator.For(Guids.PsionicFocusBuff)
            .AddBuffActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.BladeSkillFocusedDefense),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.BladeSkillFocusedDefenseBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.BladeSkillFocusedDefenseBuff))
            .Configure();
    }
}
