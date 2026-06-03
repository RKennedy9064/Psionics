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
/// On every melee hit while psionically focused, deals 2d6 extra physical damage with all four
/// alignment bypass flags (Good/Evil/Law/Chaos). Uses the exact triggering weapon's material and
/// enhancement for any additional DR bypass the weapon provides. Fires via the rulebook handler
/// so evt.Weapon is the actual weapon that made the attack.
/// </summary>
[Serializable]
public class AlignedAttackDamage : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleAttackWithWeapon>,
    IRulebookHandler<RuleAttackWithWeapon>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintBuff _focusBuff;

    public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

    public void OnEventDidTrigger(RuleAttackWithWeapon evt)
    {
        if (!evt.AttackRoll.IsHit) return;
        if (evt.Weapon?.Blueprint.IsRanged == true) return;

        _focusBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.PsionicFocusBuff);
        if (!evt.Initiator.Buffs.HasFact(_focusBuff)) return;

        var raw = WeaponInheritedDamage.Build(evt.Weapon, new DiceFormula(2, DiceType.D6), alignmentBypassAll: true);
        Rulebook.Trigger(new RuleDealDamage(evt.Initiator, evt.Target, new DamageBundle(raw)));
    }
}
