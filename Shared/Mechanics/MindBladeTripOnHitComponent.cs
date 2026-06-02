using System;
using System.Linq;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Blade skill: Weapon Special (trip). On a successful hit with the mind blade, makes a free trip
/// combat maneuver against the target — modeled on the wolf companion's bite-trip and the
/// kineticist blast trip-on-hit (which both fire a <see cref="RuleCombatManeuver"/> on hit).
/// Matches the mind blade by blueprint identity (via the active <see cref="MindBladeComponent"/>),
/// so it is category-agnostic and works for whatever weapon the mind blade currently emulates.
/// </summary>
[Serializable]
public class MindBladeTripOnHitComponent : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleAttackWithWeapon>,
    IRulebookHandler<RuleAttackWithWeapon>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

    public void OnEventDidTrigger(RuleAttackWithWeapon evt)
    {
        if (evt.AttackRoll == null || !evt.AttackRoll.IsHit) return;
        if (evt.Target == null) return;
        if (!IsMindBlade(evt.Weapon)) return;

        var maneuver = new RuleCombatManeuver(evt.Initiator, evt.Target, CombatManeuver.Trip, evt.AttackRoll.AttackBonusRule)
        {
            Reason = new RuleReason(Fact),
        };
        Rulebook.Trigger(maneuver);
    }

    private bool IsMindBlade(ItemEntityWeapon weapon)
    {
        if (weapon == null) return false;
        foreach (var buff in Owner.Descriptor.Buffs.Enumerable)
        {
            var mb = buff.Blueprint.ComponentsArray.OfType<MindBladeComponent>().FirstOrDefault();
            if (mb?.WeaponRef?.Get() == weapon.Blueprint) return true;
        }
        return false;
    }
}
