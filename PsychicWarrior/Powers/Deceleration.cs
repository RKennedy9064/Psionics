using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class Deceleration
{
    public static void Configure()
    {
        var buff = BuffConfigurator.New("PWDecelerationBuff", Guids.PowerDecelerationBuff)
            .SetDisplayName(Loc.Str("PW.Deceleration.BuffName", "Deceleration"))
            .SetDescription(Loc.Str("PW.Deceleration.BuffDesc", "Movement speed is reduced by 10 feet."))
            .SetIcon(AbilityRefs.Slow.Reference.Get().Icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Penalty, stat: StatType.Speed, value: -10)
            .Configure();

        AbilityConfigurator.New("PWDeceleration", Guids.PowerDeceleration)
            .SetDisplayName(Loc.Str("PW.Deceleration.Name", "Deceleration"))
            .SetDescription(Loc.Str("PW.Deceleration.Desc",
                "You project a telekinetic force to slow a target. The target's movement speed is reduced by 10 feet(Will negates)."))
            .SetIcon(AbilityRefs.Slow.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Close)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1Min", "1 minute"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Point)
            .SetCanTargetEnemies(true)
            .SetCanTargetSelf(false)
            .SetCanTargetFriends(false)
            .SetSpellResistance(false)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .SavingThrow(
                        type: SavingThrowType.Will,
                        onResult: ActionsBuilder.New()
                            .ConditionalSaved(
                                failed: ActionsBuilder.New()
                                    .ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Minutes)))))
            .Configure();
    }
}
