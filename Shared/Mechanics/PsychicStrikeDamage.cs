using System;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

[Serializable]
public class PsychicStrikeDamage : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleAttackWithWeapon>,
    IRulebookHandler<RuleAttackWithWeapon>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _skClass;
    private static BlueprintBuff _chargeBuff;
    private static BlueprintFeature _powerfulStrikes;
    private static BlueprintFeature _vampiricBlade;
    private static BlueprintFeature _reapersBlade;
    private static BlueprintFeature _knifeToTheSoul;
    private static BlueprintFeature _devastatingBlade;

    public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

    public void OnEventDidTrigger(RuleAttackWithWeapon evt)
    {
        if (!evt.AttackRoll.IsHit) return;

        _chargeBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.PsychicStrikeChargeBuff);
        var buff = Owner.Buffs.GetBuff(_chargeBuff);
        if (buff == null) return;

        _skClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.SoulKnifeClass);
        int level = Owner.Progression.GetClassLevel(_skClass);
        int numDice = level switch
        {
            >= 19 => 5,
            >= 15 => 4,
            >= 11 => 3,
            >= 7  => 2,
            >= 3  => 1,
            _     => 0
        };
        if (numDice <= 0) return;

        // Blade skill: Powerful Strikes — +1d8 to psychic strike.
        _powerfulStrikes ??= BlueprintTool.Get<BlueprintFeature>(Guids.BladeSkillPowerfulStrikes);
        if (_powerfulStrikes != null && Owner.Descriptor.HasFact(_powerfulStrikes))
            numDice += 1;

        buff.Remove();

        // Force damage (matches Psionic Weapon): fits the psychic theme and bypasses physical DR.
        var raw = new EnergyDamage(new DiceFormula(numDice, DiceType.D8), DamageEnergyType.Magic);
        var damageRule = Rulebook.Trigger(new RuleDealDamage(evt.Initiator, evt.Target, new DamageBundle(raw))
        {
            Reason = new RuleReason(Fact),
            ForceUseReasonAsSource = true,
        });

        // Blade skill: Vampiric Blade — heal the soulknife for half the psychic strike damage dealt.
        _vampiricBlade ??= BlueprintTool.Get<BlueprintFeature>(Guids.BladeSkillVampiricBlade);
        if (_vampiricBlade != null && Owner.Descriptor.HasFact(_vampiricBlade))
        {
            int healed = damageRule.Result / 2;
            if (healed > 0)
                Rulebook.Trigger(new RuleHealDamage(evt.Initiator, evt.Initiator, healed));
        }

        // Blade skill: Knife to the Soul / Devastating Blade — also deal Intelligence damage.
        // (Adaptation: applied as a rider in addition to the HP damage, rather than the tabletop
        // "substitute for HP damage" choice.) Devastating Blade increases the rate.
        _devastatingBlade ??= BlueprintTool.Get<BlueprintFeature>(Guids.BladeSkillDevastatingBlade);
        _knifeToTheSoul ??= BlueprintTool.Get<BlueprintFeature>(Guids.BladeSkillKnifeToTheSoul);
        bool hasDevastating = _devastatingBlade != null && Owner.Descriptor.HasFact(_devastatingBlade);
        bool hasKnife = _knifeToTheSoul != null && Owner.Descriptor.HasFact(_knifeToTheSoul);
        if (hasKnife || hasDevastating)
        {
            int abilityDamage = hasDevastating ? numDice : System.Math.Max(1, numDice / 2);
            Rulebook.Trigger(new RuleDealStatDamage(
                evt.Initiator, evt.Target, StatType.Intelligence, DiceFormula.Zero, abilityDamage));
        }

        // Blade skill: Reaper's Blade — if the strike drops the target, recharge psychic strike.
        _reapersBlade ??= BlueprintTool.Get<BlueprintFeature>(Guids.BladeSkillReapersBlade);
        if (_reapersBlade != null && Owner.Descriptor.HasFact(_reapersBlade)
            && evt.Target?.Descriptor?.State?.IsDead == true)
        {
            Owner.AddBuff(_chargeBuff, Owner);
        }
    }
}
