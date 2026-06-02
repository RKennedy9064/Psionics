using System;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

// While the Mind Knight Maneuver buff is active, adds a competence bonus to attack rolls
// equal to +1 per 4 psychic warrior levels. Fires on RuleCalculateAttackBonusWithoutTarget
// which runs before the attack roll (and before AddInitiatorAttackRollTrigger removes the buff),
// so the buff check is reliable without caching.
[Serializable]
public class AdvancedMindKnightAttackBonus : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _pwClass;
    private static BlueprintBuff _maneuverBuff;

    public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
    {
        _maneuverBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.MindKnightManeuverBuff);
        if (!Owner.Buffs.HasFact(_maneuverBuff)) return;

        _pwClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.PsychicWarriorClass);
        int level = Owner.Progression.GetClassLevel(_pwClass);
        int bonus = level / 4;
        if (bonus <= 0) return;

        evt.AddModifier(bonus, Fact, ModifierDescriptor.Competence);
    }

    public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) { }
}
