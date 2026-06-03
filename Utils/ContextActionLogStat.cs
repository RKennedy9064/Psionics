using System;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace Psionics.Utils;

/// <summary>
/// Reads a single stat off the caster and emits it to Player.log.
/// Use two instances (before + after ApplyBuff) to capture the delta.
/// </summary>
[Serializable]
public class ContextActionLogStat : ContextAction
{
    public StatType Stat;
    public string Label = "";

    public override string GetCaption() => "PW Log Stat";

    public override void RunAction()
    {
        if (!ContextActionLog.Enabled) return;
        try
        {
            var unit = Context?.MaybeCaster;
            if (unit == null) return;
            var stat = unit.Stats.GetStat(Stat);
            int val = stat?.ModifiedValue ?? -999;
            string name = unit.CharacterName ?? "?";
            UnityEngine.Debug.Log($"[PW Debug] {Label}: {Stat}={val} | caster={name}");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log($"[PW Debug] {Label}: stat-err {e.Message}");
        }
    }
}
