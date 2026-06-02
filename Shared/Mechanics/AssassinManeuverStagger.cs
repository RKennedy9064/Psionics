using System;
using BlueprintCore.Utils;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

// On a hit while the Assassin maneuver buff is active, forces a Fortitude save (DC 10 + BAB).
// On failure the target is staggered for a number of rounds equal to the attacker's Wisdom modifier.
// The maneuver buff removes itself on the attack roll (before OnEventDidTrigger), so presence is
// cached in OnEventAboutToTrigger and consumed on the matching OnEventDidTrigger.
[Serializable]
public class AssassinManeuverStagger : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleAttackWithWeapon>,
    IRulebookHandler<RuleAttackWithWeapon>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintBuff _maneuverBuff;
    private static BlueprintBuff _staggerBuff;

    private bool _hasManeuver;

    public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
    {
        _maneuverBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.AssassinsManeuverBuff);
        _hasManeuver = Owner.Buffs.HasFact(_maneuverBuff);
    }

    public void OnEventDidTrigger(RuleAttackWithWeapon evt)
    {
        var hadManeuver = _hasManeuver;
        _hasManeuver = false;

        if (!hadManeuver || !evt.AttackRoll.IsHit) return;

        int dc = 10 + Owner.Stats.BaseAttackBonus.BaseValue;
        var saveRule = new RuleSavingThrow(evt.Target, SavingThrowType.Fortitude, dc);
        Rulebook.Trigger(saveRule);

        if (!saveRule.IsPassed)
        {
            _staggerBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.AdvancedAssassinStaggerBuff);
            int rounds = Math.Max(1, Owner.Stats.Wisdom.Bonus);
            evt.Target.Descriptor.AddBuff(_staggerBuff, Context, TimeSpan.FromSeconds(rounds * 6.0));
        }
    }
}
