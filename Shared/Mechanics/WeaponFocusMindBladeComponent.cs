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
        if (evt.Weapon == null) return;

        foreach (var buff in Owner.Descriptor.Buffs.Enumerable)
        {
            var mbComp = buff.Blueprint.ComponentsArray.OfType<MindBladeComponent>().FirstOrDefault();
            if (mbComp?.WeaponRef?.Get() == evt.Weapon.Blueprint)
            {
                evt.AddModifier(1, Fact, ModifierDescriptor.None);
                return;
            }
        }
    }

    public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) { }
}
