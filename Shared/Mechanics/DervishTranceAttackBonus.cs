using System;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Applies a competence bonus to attack rolls while in Dervish Trance, but only when wielding two weapons.
/// Fires on RuleCalculateAttackBonusWithoutTarget (same rule as the TWF penalty) so the bonus shows in
/// the character sheet attack tooltip.
/// Bonus scales with PW class level: +1 at 3, +2 at 7, +3 at 11, +4 at 15, +5 at 19.
/// </summary>
[Serializable]
public class DervishTranceAttackBonus : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _pwClass;

    public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
    {
        var body = evt.Initiator.Body;
        var secondary = body.SecondaryHand;

        // Skip if not dual-wielding: secondary hand must have a weapon (not shield, not empty)
        if (!secondary.HasWeapon || secondary.HasShield)
            return;
        // Skip if secondary hand holds a natural weapon or unarmed strike
        var secondaryBp = secondary.MaybeWeapon?.Blueprint;
        if (secondaryBp == null || secondaryBp.IsNatural || secondaryBp.Category == WeaponCategory.UnarmedStrike)
            return;
        // Skip if primary hand is gripping a two-handed weapon
        if (body.CurrentHandsEquipmentSet.GripType == GripType.TwoHanded)
            return;

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
