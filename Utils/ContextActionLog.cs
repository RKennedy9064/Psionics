using System;
using System.Text;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace Psionics.Utils;

/// <summary>
/// Drop into any ActionsBuilder chain to emit a debug line to Player.log.
/// Set Enabled = false at runtime to silence all PW debug output.
/// </summary>
[Serializable]
public class ContextActionLog : ContextAction
{
    public static bool Enabled = true;

    public string Message = "";
    // When true, appends Context[Default] (resolved rank — ML, Wis mod, etc.)
    public bool LogRank = false;

    public override string GetCaption() => "PW Log";

    public override void RunAction()
    {
        if (!Enabled) return;
        try
        {
            var sb = new StringBuilder("[PW Debug] ").Append(Message);

            if (LogRank)
            {
                try { sb.Append(" rank=").Append(Context[AbilityRankType.Default]); }
                catch { sb.Append(" rank=?"); }
            }

            var caster = TryGetName(Context?.MaybeCaster);
            sb.Append(" | caster=").Append(caster);

            var target = TryGetName(Context?.MainTarget?.Unit);
            if (target != "?") sb.Append(" target=").Append(target);

            UnityEngine.Debug.Log(sb.ToString());
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log($"[PW Debug] {Message} (log-err: {e.Message})");
        }
    }

    private static string TryGetName(Kingmaker.EntitySystem.Entities.UnitEntityData unit)
    {
        if (unit == null) return "?";
        try { return unit.CharacterName ?? unit.Blueprint?.name ?? "?"; }
        catch { return "?"; }
    }
}
