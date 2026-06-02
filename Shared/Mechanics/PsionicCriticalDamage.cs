using System;
using BlueprintCore.Utils;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// On a confirmed critical hit while psionically focused, deals 1d8 extra physical damage using
/// the exact triggering weapon's bypass properties (material, enhancement bonus). Fires via the
/// rulebook handler so evt.Weapon is the actual weapon that made the attack, not an approximation.
/// </summary>
[Serializable]
public class PsionicCriticalDamage : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleAttackWithWeapon>,
    IRulebookHandler<RuleAttackWithWeapon>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintBuff _focusBuff;

    public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

    public void OnEventDidTrigger(RuleAttackWithWeapon evt)
    {
        if (!evt.AttackRoll.IsHit || !evt.AttackRoll.IsCriticalConfirmed) return;

        _focusBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.PsionicFocusBuff);
        if (!evt.Initiator.Buffs.HasFact(_focusBuff)) return;

        var raw = WeaponInheritedDamage.Build(evt.Weapon, new DiceFormula(1, DiceType.D8), alignmentBypassAll: false);
        Rulebook.Trigger(new RuleDealDamage(evt.Initiator, evt.Target, new DamageBundle(raw)));
    }
}
