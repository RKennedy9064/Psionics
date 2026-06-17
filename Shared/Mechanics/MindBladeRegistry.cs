using System.Collections.Generic;
using Kingmaker.Blueprints.Items.Weapons;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Central registry of every mind blade item blueprint (per-category primaries + off-hands, plus the
/// legacy fixed-form blueprints kept for save compatibility). This is the single source of truth for
/// "is this weapon a mind blade?", replacing the old approaches that compared against a buff's
/// WeaponRef or a hardcoded GUID list. Populated by <c>MindBlade.Configure</c> at init.
/// </summary>
public static class MindBladeRegistry
{
    private static readonly HashSet<BlueprintItemWeapon> Items = [];

    public static void Register(BlueprintItemWeapon bp)
    {
        if (bp != null) Items.Add(bp);
    }

    public static bool IsMindBlade(BlueprintItemWeapon bp) => bp != null && Items.Contains(bp);
}
