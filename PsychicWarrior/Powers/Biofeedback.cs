using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class Biofeedback
{
    public static void Configure()
    {
        var buff = BuffConfigurator.New("PWBiofeedbackBuff", Guids.PowerBiofeedbackBuff)
            .SetDisplayName(Loc.Str("PW.Biofeedback.BuffName", "Biofeedback"))
            .SetDescription(Loc.Str("PW.Biofeedback.BuffDesc",
                "A psionic biofeedback loop reduces all physical damage you take. DR 2/- at ML 1, improving by 1 per 3 manifester levels."))
            .SetIcon(AbilityRefs.IronBody.Reference.Get().Icon)
            .AddDamageResistancePhysical(value: ContextValues.Rank())
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (3, 2), (6, 3), (9, 4), (12, 5), (15, 6), (18, 7), (20, 8)))
            .Configure();

        AbilityConfigurator.New("PWBiofeedback", Guids.PowerBiofeedback)
            .SetDisplayName(Loc.Str("PW.Biofeedback.Name", "Biofeedback"))
            .SetDescription(Loc.Str("PW.Biofeedback.Desc",
                "You create a biofeedback loop that protects you from harm. DR 2/- at ML 1, improving by 1 per 3 manifester levels."))
            .SetIcon(AbilityRefs.IronBody.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1Hour", "1 hour"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(1, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[Biofeedback] applying DR (rank=ML; buff scales DR2..DR8 at ML 1..20)", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Hours)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
