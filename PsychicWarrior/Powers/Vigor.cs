using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class Vigor
{
    public static void Configure()
    {
        AbilityConfigurator.New("PsychicWarriorVigor", Guids.PowerVigor)
            .SetDisplayName(Loc.Str("PW.Vigor.Name", "Vigor"))
            .SetDescription(Loc.Str("PW.Vigor.Desc",
                "You suffuse yourself with psionic power, gaining temporary hit points as the false life spell (1d10 + 1/manifester level, max +10)."))
            .SetIcon(AbilityRefs.FalseLife.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1Hour", "1 hour"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(1, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[Vigor] applying FalseLifeBuff", LogRank = false })
                    .ApplyBuff(
                        buff: BuffRefs.FalseLifeBuff.ToString(),
                        durationValue: ContextDuration.Fixed(1, DurationRate.Hours)))
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
