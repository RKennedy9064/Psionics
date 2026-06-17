using System;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Blade skill: Mind Blade Finesse. Lets the soulknife use Dexterity instead of Strength on attack
/// rolls with her mind blade, in any form. Implemented per-wielder on the attack-bonus rule (so it
/// cannot bleed across characters), rather than by mutating the shared weapon-type blueprint.
/// Adds the Dex-minus-Str delta on top of the weapon's normal Strength-based attack, yielding a net
/// Dexterity attack. Skipped for ranged forms and for weapons that already use Dexterity (e.g. a
/// rapier mind blade), so nothing is double-counted.
/// </summary>
[Serializable]
public class MindBladeFinesseComponent : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
    {
        var weapon = evt.Weapon;
        if (weapon?.Blueprint == null) return;
        if (!MindBladeRegistry.IsMindBlade(weapon.Blueprint)) return;
        if (weapon.Blueprint.IsRanged) return; // finesse is melee-only

        // Determine the weapon's current attack stat; only replace it when it is Strength.
        var type = weapon.Blueprint.m_Type?.Get();
        var current = (type != null && type.m_OverrideAttackBonusStat)
            ? type.m_AttackBonusStatOverride
            : StatType.Strength;
        if (current == StatType.Dexterity) return; // already finesse — don't double-count

        int delta = Owner.Stats.Dexterity.Bonus - Owner.Stats.Strength.Bonus;
        if (delta != 0) evt.AddModifier(delta, Fact, ModifierDescriptor.None);
    }

    public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) { }
}
