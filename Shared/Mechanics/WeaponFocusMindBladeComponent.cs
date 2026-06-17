using System;
using System.Linq;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace Psionics.Shared.Mechanics;

[Serializable]
public class WeaponFocusMindBladeComponent : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
    {
        if (evt.Weapon?.Blueprint == null) return;
        if (MindBladeRegistry.IsMindBlade(evt.Weapon.Blueprint))
            evt.AddModifier(1, Fact, ModifierDescriptor.None);
    }

    public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) { }
}
