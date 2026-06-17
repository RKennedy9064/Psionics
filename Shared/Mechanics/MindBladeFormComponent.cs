using System;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Data carrier placed on each mind blade weapon-entry feature (one per chosen weapon category).
/// Records the dedicated, immutable per-category mind blade item that the player's selection maps to.
/// <see cref="MindBladeComponent"/> reads this off the owner's chosen feature at manifest time and
/// equips that exact blueprint — so nothing is mutated at runtime and characters never share state.
/// </summary>
[Serializable]
public class MindBladeFormComponent : UnitFactComponentDelegate
{
    public BlueprintItemWeaponReference ItemRef;
}
