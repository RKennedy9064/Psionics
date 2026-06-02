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

[Serializable]
public class AdvancedArcherDamageBonus : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
    IRulebookHandler<RuleCalculateWeaponStats>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _pwClass;
    private static BlueprintBuff _tranceBuff;

    public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
    {
        _tranceBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.ArcherTranceBuff);
        if (!Owner.Buffs.HasFact(_tranceBuff)) return;

        var weaponBp = evt.Weapon?.Blueprint;
        if (weaponBp == null || !weaponBp.IsRanged || weaponBp.IsNatural)
            return;

        _pwClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.PsychicWarriorClass);
        int level = Owner.Progression.GetClassLevel(_pwClass);

        // Half the trance competence bonus (floor): +0/+1/+1/+2/+2 at 3/7/11/15/19
        int bonus = level switch
        {
            >= 15 => 2,
            >= 7 => 1,
            _ => 0
        };

        if (bonus <= 0) return;
        evt.AddDamageModifier(bonus, Fact, ModifierDescriptor.Competence);
    }

    public void OnEventDidTrigger(RuleCalculateWeaponStats evt) { }
}
