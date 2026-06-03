using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
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

public static class ConcealingAmorpha
{
    public static void Configure()
    {
        var icon = AbilityRefs.Blur.Reference.Get().Icon;

        AbilityConfigurator.New("PWConcealingAmorpha", Guids.PowerConcealingAmorpha)
            .SetDisplayName(Loc.Str("PW.ConcealingAmorpha.Name", "Concealing Amorpha"))
            .SetDescription(Loc.Str("PW.ConcealingAmorpha.Desc",
                "You surround yourself with a distortion of ectoplasmic shimmer that grants you a 20% miss chance."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[ConcealingAmorpha] applying 20% miss chance (blur, 1 min/ML)", LogRank = true })
                    .ApplyBuff(BuffRefs.BlurBuff.ToString(), ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(2, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
