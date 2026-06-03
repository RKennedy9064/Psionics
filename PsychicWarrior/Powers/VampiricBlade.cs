using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

/// <summary>
/// Vampiric Blade (Psychometabolism â†' Transmutation)  -  RAW Psionic Unleashed.
///
/// You imbue your weapon with soul-stealing energy. While active, each successful melee weapon hit
/// deals an extra 2d6 negative-energy damage to the target, and you gain temporary hit points equal
/// to one-half the damage dealt.
///
/// Implementation chain (on each melee hit):
///   1. Deal 2d6 negative-energy damage; result stored in <c>AbilitySharedValue.Damage</c>
///   2. <c>ChangeSharedValueDivideBy2(Damage)</c> halves the stored value in place
///   3. Apply a stackable temp-HP buff to the caster that reads <c>AbilitySharedValue.Damage</c>
///      via <c>TemporaryHitPointsFromAbilityValue</c>; the buff stacks so multiple hits accumulate.
/// </summary>
public static class VampiricBlade
{
    public static void Configure()
    {
        var icon = AbilityRefs.VampiricTouch.Reference.Get().Icon;

        // Temp HP buff applied to caster on each hit. Stacks so consecutive hits accumulate temp HP.
        var tempHpBuff = BuffConfigurator.New("PWVampiricBladeTempHPBuff", Guids.PowerVampiricBladeTempHPBuff)
            .SetDisplayName(Loc.Str("PW.VampiricBladeTempHP.Name", "Vampiric Blade (Temp HP)", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.VampiricBladeTempHP.Desc",
                "Temporary hit points drained from a foe by your vampiric weapon.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetStacking(StackingType.Stack)
            .AddTemporaryHitPointsFromAbilityValue(
                descriptor: ModifierDescriptor.UntypedStackable,
                value: ContextValues.Shared(AbilitySharedValue.Damage))
            .Configure();

        // Main buff with the on-hit trigger
        var buff = BuffConfigurator.New("PWVampiricBladeBuff", Guids.PowerVampiricBladeBuff)
            .SetDisplayName(Loc.Str("PW.VampiricBlade.BuffName", "Vampiric Blade", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.VampiricBlade.BuffDesc",
                "Your weapon drinks the life essence of those you strike: each melee hit deals +2d6 negative-energy damage and grants you temporary hit points equal to half the damage dealt.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New()
                    .DealDamage(
                        damageType: DamageTypes.Energy(DamageEnergyType.NegativeEnergy),
                        value: ContextDice.Value(DiceType.D6, 2),
                        resultSharedValue: AbilitySharedValue.Damage)
                    .ChangeSharedValueDivideBy2(AbilitySharedValue.Damage)
                    .ApplyBuff(
                        buff: tempHpBuff,
                        durationValue: ContextDuration.Fixed(1, DurationRate.Hours),
                        toCaster: true),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Melee)
            .Configure();

        AbilityConfigurator.New("PWVampiricBlade", Guids.PowerVampiricBlade)
            .SetDisplayName(Loc.Str("PW.VampiricBlade.Name", "Vampiric Blade", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.VampiricBlade.Desc",
                "You imbue your weapon with soul-stealing psionic energy. For 1 round per manifester level, your melee attacks deal an extra 2d6 negative-energy damage and you gain temporary hit points equal to half the damage dealt by these strikes.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(3, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(
                    buff,
                    ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
