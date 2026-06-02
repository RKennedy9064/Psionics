using System;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

[Serializable]
public class AssassinSneakAttack : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleAttackWithWeapon>,
    IRulebookHandler<RuleAttackWithWeapon>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _pwClass;
    private static BlueprintBuff _tranceBuff;

    public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

    public void OnEventDidTrigger(RuleAttackWithWeapon evt)
    {
        if (!evt.AttackRoll.IsHit) return;

        _tranceBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.AssassinsTranceBuff);
        if (!Owner.Buffs.HasFact(_tranceBuff)) return;

        var target = evt.Target;
        bool eligible = target.Descriptor.State.HasCondition(UnitCondition.LoseDexterityToAC)
                     || evt.AttackRoll.TargetIsFlanked;
        if (!eligible) return;

        if (evt.AttackRoll.ImmuneToSneakAttack || evt.AttackRoll.FortificationNegatesSneakAttack) return;

        _pwClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.PsychicWarriorClass);
        int level = Owner.Progression.GetClassLevel(_pwClass);
        int numDice = level switch
        {
            >= 18 => 5,
            >= 14 => 4,
            >= 10 => 3,
            >= 6 => 2,
            >= 2 => 1,
            _ => 0
        };
        if (numDice <= 0) return;

        var raw = WeaponInheritedDamage.Build(evt.Weapon, new DiceFormula(numDice, DiceType.D6), alignmentBypassAll: false);
        raw.Sneak = true;
        Rulebook.Trigger(new RuleDealDamage(evt.Initiator, evt.Target, new DamageBundle(raw)));
    }
}
