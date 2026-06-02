using System;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

[Serializable]
public class AdvancedBrawlerCMBBonus : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCombatManeuver>,
    IRulebookHandler<RuleCombatManeuver>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    private static BlueprintCharacterClass _pwClass;
    private static BlueprintBuff _tranceBuff;

    public void OnEventAboutToTrigger(RuleCombatManeuver evt)
    {
        if (evt.Type != CombatManeuver.Grapple) return;

        _tranceBuff ??= BlueprintTool.Get<BlueprintBuff>(Guids.BrawlerTranceBuff);
        if (!Owner.Buffs.HasFact(_tranceBuff)) return;

        _pwClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.PsychicWarriorClass);
        int level = Owner.Progression.GetClassLevel(_pwClass);
        int bonus = level / 3;
        if (bonus <= 0) return;

        evt.AdditionalBonus += bonus;
    }

    public void OnEventDidTrigger(RuleCombatManeuver evt) { }
}
