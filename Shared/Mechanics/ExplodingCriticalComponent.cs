using System;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Blade skill: Exploding Critical. On a confirmed critical hit with the mind blade, if the
/// soulknife is psionically focused, she expends her focus to deal psychic strike damage — even
/// if she had no psychic strike charged. Independent of <see cref="PsychicStrikeDamage"/> (which
/// requires an existing charge).
/// </summary>
[Serializable]
public class ExplodingCriticalComponent : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleAttackWithWeapon>,
    IRulebookHandler<RuleAttackWithWeapon>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _skClass;
    private static BlueprintBuff _focusBuff;

    public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) { }

    public void OnEventDidTrigger(RuleAttackWithWeapon evt)
    {
        if (evt.AttackRoll == null || !evt.AttackRoll.IsCriticalConfirmed) return;

        _focusBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.PsionicFocusBuff);
        var focus = Owner.Buffs.GetBuff(_focusBuff);
        if (focus == null) return;

        _skClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.SoulKnifeClass);
        int level = Owner.Progression.GetClassLevel(_skClass);
        int numDice = level switch
        {
            >= 19 => 5,
            >= 15 => 4,
            >= 11 => 3,
            >= 7  => 2,
            >= 3  => 1,
            _     => 0
        };
        if (numDice <= 0) return;

        focus.Remove();

        var raw = WeaponInheritedDamage.Build(evt.Weapon, new DiceFormula(numDice, DiceType.D8), alignmentBypassAll: false);
        Rulebook.Trigger(new RuleDealDamage(evt.Initiator, evt.Target, new DamageBundle(raw))
        {
            Reason = new RuleReason(Fact),
            ForceUseReasonAsSource = true,
        });
    }
}
