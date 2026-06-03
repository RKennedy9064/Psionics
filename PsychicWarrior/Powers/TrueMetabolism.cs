using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class TrueMetabolism
{
    public static void Configure()
    {
        var icon = AbilityRefs.CureCriticalWounds.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWTrueMetabolismBuff", Guids.PowerTrueMetabolismBuff)
            .SetDisplayName(Loc.Str("PW.TrueMetabolism.BuffName", "True Metabolism", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.TrueMetabolism.BuffDesc",
                "You are regenerating rapidly. Fast healing 5 at ML 9, improving by 1 per 4 manifester levels.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddBuffActions(
                newRound: ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[TrueMetabolism] fast healing tick", LogRank = true })
                    .HealTarget(ContextDice.Value(DiceType.One, ContextValues.Rank(), 0)))
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (12, 5), (16, 6), (20, 7)))
            .Configure();

        AbilityConfigurator.New("PWTrueMetabolism", Guids.PowerTrueMetabolism)
            .SetDisplayName(Loc.Str("PW.TrueMetabolism.Name", "True Metabolism", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.TrueMetabolism.Desc",
                "Your cells regenerate at a remarkable rate. Fast healing 5 at ML 9, improving by 1 per 4 manifester levels.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(5, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[TrueMetabolism] applying fast healing (rank=ML; buff scales FH5/FH6/FH7 at ML 9/13/17)", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
