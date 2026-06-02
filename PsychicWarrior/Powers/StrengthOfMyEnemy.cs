using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

/// <summary>
/// Strength of My Enemy (Psychometabolism â†' Divination)  -  Self-imbue.
///
/// RAW: "Each successful melee attack you make while manifesting this power grants you a +1
/// enhancement bonus to Strength as you absorb 2 points of Strength from your opponent. The
/// maximum enhancement bonus you can gain from this power is +6."
///
/// Implementation:
///   —¢ Main buff applied to caster/level (Personal range).
///   —¢ Main buff carries <c>AddInitiatorAttackWithWeaponTrigger</c> on melee hit.
///   —¢ On hit, a Conditional checks the caster's current rank of the caster-stack buff. If
///     the rank is below 6, both buffs are applied:
///       - Caster gets the rank-stacking buff (StackingType.Rank, max 6, +1 enhancement Str per rank)
///       - Enemy gets a stackable buff (StackingType.Stack, -2 untyped Str)
///   —¢ The conditional cap on the caster's buff rank prevents enemy stacks beyond what RAW allows.
/// </summary>
public static class StrengthOfMyEnemy
{
    public static void Configure()
    {
        var icon = AbilityRefs.Enervation.Reference.Get().Icon;

        // Caster-stacking buff: each rank adds +1 enhancement Strength, max 6 ranks.
        var casterBuff = BuffConfigurator.New("PWStrengthOfMyEnemyCasterBuff", Guids.PowerStrengthOfMyEnemyCasterBuff)
            .SetDisplayName(Loc.Str("PW.StrengthOfMyEnemy.CasterBuff.Name", "Strength of My Enemy", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.StrengthOfMyEnemy.CasterBuff.Desc",
                "You have absorbed Strength from your foes, gaining a +1 enhancement bonus per stack (max +6).",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetStacking(StackingType.Rank)
            .SetRanks(6)
            .AddContextStatBonus(
                stat: StatType.Strength,
                descriptor: ModifierDescriptor.Enhancement,
                value: ContextValues.Rank())
            .AddContextRankConfig(
                ContextRankConfigs.BuffRank(Guids.PowerStrengthOfMyEnemyCasterBuff))
            .Configure();

        // Enemy debuff: -2 Strength per stack, stackable.
        var enemyDebuff = BuffConfigurator.New("PWStrengthOfMyEnemyEnemyDebuff", Guids.PowerStrengthOfMyEnemyEnemyDebuff)
            .SetDisplayName(Loc.Str("PW.StrengthOfMyEnemy.EnemyDebuff.Name", "Strength Drained", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.StrengthOfMyEnemy.EnemyDebuff.Desc",
                "Your Strength has been psionically drained  -  2 points per stack.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetStacking(StackingType.Stack)
            .AddStatBonus(descriptor: ModifierDescriptor.UntypedStackable, stat: StatType.Strength, value: -2)
            .Configure();

        // Main buff with the on-hit trigger. Caps via Conditional on the caster-stack buff's rank.
        var mainBuff = BuffConfigurator.New("PWStrengthOfMyEnemyMainBuff", Guids.PowerStrengthOfMyEnemyBuff)
            .SetDisplayName(Loc.Str("PW.StrengthOfMyEnemy.MainBuff.Name", "Strength of My Enemy", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.StrengthOfMyEnemy.MainBuff.Desc",
                "Your weapon is imbued with strength-draining psionic energy. Each successful melee hit drains 2 Strength from your foe and grants you +1 enhancement Strength (max +6).",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New()
                    .Conditional(
                        conditions: ConditionsBuilder.New().BuffRank(
                            buff: Guids.PowerStrengthOfMyEnemyCasterBuff,
                            rankValue: ContextValues.Constant(6),
                            negate: true),
                        ifTrue: ActionsBuilder.New()
                            .ApplyBuff(
                                buff: casterBuff,
                                durationValue: ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds),
                                toCaster: true)
                            .ApplyBuff(
                                buff: enemyDebuff,
                                durationValue: ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds))),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Melee)
            .Configure();

        AbilityConfigurator.New("PWStrengthOfMyEnemy", Guids.PowerStrengthOfMyEnemy)
            .SetDisplayName(Loc.Str("PW.StrengthOfMyEnemy.Name", "Strength of My Enemy", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.StrengthOfMyEnemy.Desc",
                "Imbue your weapon with strength-draining psionic energy. For 1 round per manifester level, each successful melee hit drains 2 Strength from your foe and grants you a +1 enhancement bonus to Strength (maximum +6).",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(2, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(
                    mainBuff,
                    ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Divination)
            .Configure();
    }
}
