using System;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

[Serializable]
public class AdvancedDervishDamageBonus : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
    IRulebookHandler<RuleCalculateWeaponStats>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _pwClass;
    private static BlueprintBuff _tranceBuff;

    public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
    {
        _tranceBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.DervishTranceBuff);
        if (!Owner.Buffs.HasFact(_tranceBuff)) return;

        // Same dual-wield gate as DervishTranceAttackBonus
        var body = Owner.Body;
        var secondary = body.SecondaryHand;
        if (!secondary.HasWeapon || secondary.HasShield) return;
        var secondaryBp = secondary.MaybeWeapon?.Blueprint;
        if (secondaryBp == null || secondaryBp.IsNatural || secondaryBp.Category == WeaponCategory.UnarmedStrike)
            return;
        if (body.CurrentHandsEquipmentSet.GripType == GripType.TwoHanded) return;

        _pwClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.PsychicWarriorClass);
        int level = Owner.Progression.GetClassLevel(_pwClass);
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
        evt.AddDamageModifier(bonus, Fact, ModifierDescriptor.Competence);
    }

    public void OnEventDidTrigger(RuleCalculateWeaponStats evt) { }
}
