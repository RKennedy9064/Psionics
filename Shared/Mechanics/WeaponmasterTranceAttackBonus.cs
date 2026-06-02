using System;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Applies a competence bonus to attack rolls while in Weaponmaster Trance for all manufactured
/// weapons. Skips natural weapons and unarmed strikes; everything else qualifies.
/// Fires on RuleCalculateAttackBonusWithoutTarget so the bonus shows in the attack tooltip.
/// Bonus scales with PW class level: +1 at 3, +2 at 7, +3 at 11, +4 at 15, +5 at 19.
/// </summary>
[Serializable]
public class WeaponmasterTranceAttackBonus : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _pwClass;

    public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
    {
        var weaponBp = evt.Weapon?.Blueprint;
        if (weaponBp == null || weaponBp.IsNatural || weaponBp.Category == WeaponCategory.UnarmedStrike)
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
