using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class InertialBarrier
{
    public static void Configure()
    {
        var icon = AbilityRefs.StoneskinCommunal.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWInertialBarrierBuff", Guids.PowerInertialBarrierBuff)
            .SetDisplayName(Loc.Str("PW.InertialBarrier.BuffName", "Inertial Barrier"))
            .SetDescription(Loc.Str("PW.InertialBarrier.BuffDesc",
                "Your body is surrounded by an invisible field of psionic force. DR 5/- at ML 7, improving by 1 per 2 manifester levels."))
            .SetIcon(icon)
            .AddDamageResistancePhysical(value: ContextValues.Rank())
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (8, 5), (10, 6), (12, 7), (14, 8), (16, 9), (18, 10), (20, 11)))
            .Configure();

        AbilityConfigurator.New("PWInertialBarrier", Guids.PowerInertialBarrier)
            .SetDisplayName(Loc.Str("PW.InertialBarrier.Name", "Inertial Barrier"))
            .SetDescription(Loc.Str("PW.InertialBarrier.Desc",
                "You surround your body with an invisible field of psionic force. DR 5/- at ML 7, improving by 1 per 2 manifester levels."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.10MinPerML", "10 minutes per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[InertialBarrier] applying DR (scales 5â†'11 with ML)", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.TenMinutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(4, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Abjuration)
            .Configure();
    }
}
