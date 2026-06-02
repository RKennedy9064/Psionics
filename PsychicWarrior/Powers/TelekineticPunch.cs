using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class TelekineticPunch
{
    public static void Configure()
    {
        AbilityConfigurator.New("PWTelekineticPunch", Guids.PowerTelekineticPunch)
            .SetDisplayName(Loc.Str("PW.TelekineticPunch.Name", "Telekinetic Punch"))
            .SetDescription(Loc.Str("PW.TelekineticPunch.Desc",
                "You deliver a telekinetic punch to your target, dealing 1d4+1 points of force damage. A successful Will save negates the damage."))
            .SetIcon(AbilityRefs.TelekineticFist.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Close)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
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
                                    .DealDamage(
                                        damageType: new DamageTypeDescription { Type = DamageType.Direct },
                                        value: new ContextDiceValue
                                        {
                                            DiceType = DiceType.D4,
                                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 },
                                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 }
                                        }))))
            .Configure();
    }
}
