using System;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Blade skill: Mark of the Challenger. Carried by the debuff buff applied to a marked foe. While
/// marked, the creature takes a −2 penalty on its attack rolls. (Adaptation: the tabletop penalty
/// only applies to attacks against creatures other than the soulknife; WotR has no clean
/// attacker-vs-specific-target hook, so it is applied to all of the marked creature's attacks.)
/// </summary>
[Serializable]
public class MarkOfTheChallengerComponent : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
    {
        evt.AddModifier(-2, Fact, ModifierDescriptor.Penalty);
    }

    public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) { }
}
