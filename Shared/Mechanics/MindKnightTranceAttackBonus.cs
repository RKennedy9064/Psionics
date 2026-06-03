using System;
using System.Linq;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Competence bonus to attack rolls while in Mind Knight Trance, but only when attacking with
/// the currently called weapon (primary hand weapon locked by CallWeaponryComponent).
/// Secondary hand attacks and attacks made without a called weapon active are not affected.
/// </summary>
[Serializable]
public class MindKnightTranceAttackBonus : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _pwClass;

    public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
    {
        if (evt.Weapon == null) return;

        // Only applies to primary hand attacks
        var primaryWeapon = evt.Initiator.Body.PrimaryHand.MaybeWeapon;
        if (primaryWeapon == null || evt.Weapon != primaryWeapon) return;

        // Only applies when a called weapon buff is active
        var hasCW = false;
        foreach (var buff in evt.Initiator.Descriptor.Buffs.Enumerable)
        {
            if (buff.Blueprint.ComponentsArray.OfType<CallWeaponryComponent>().Any())
            {
                hasCW = true;
                break;
            }
        }
        if (!hasCW) return;

        _pwClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.PsychicWarriorClass);

        int level = evt.Initiator.Progression.GetClassLevel(_pwClass);
        int bonus = level switch
        {
            >= 19 => 5,
            >= 15 => 4,
            >= 11 => 3,
            >= 7 => 2,
            >= 3 => 1,
            _ => 0
        };

        if (bonus <= 0) return;
        evt.AddModifier(bonus, Fact, ModifierDescriptor.Competence);
    }

    public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) { }
}
