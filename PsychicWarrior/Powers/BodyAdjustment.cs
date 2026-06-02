using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

/// <summary>
/// Body Adjustment (Conjuration) — Heal yourself 2d8 + caster level (max +10).
/// </summary>
public static class BodyAdjustment
{
    public static void Configure()
    {
        AbilityConfigurator.New("PWBodyAdjustment", Guids.PowerBodyAdjustment)
            .SetDisplayName(Loc.Str("PW.BodyAdjustment.Name", "Body Adjustment", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.BodyAdjustment.Desc",
                "Through psionic biomanipulation, you heal yourself. Heals 2d8 + 1d8 per 2 manifester levels above 3.",
                tagEncyclopediaEntries: false))
            .SetIcon(AbilityRefs.CureModerateWounds.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(2, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[BodyAdjustment] healing (rank=dice count; 2d8 at ML3 → 10d8 at ML20)", LogRank = true })
                    .HealTarget(
                        value: ContextDice.Value(DiceType.D8, ContextValues.Rank(), 0)))
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (4, 2), (6, 3), (8, 4), (10, 5), (12, 6), (14, 7), (16, 8), (18, 9), (20, 10)))
            .AddSpellComponent(SpellSchool.Conjuration)
            .Configure();
    }
}
