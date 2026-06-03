using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class DimensionDoor
{
    public static void Configure()
    {
        AbilityConfigurator.New("PWDimensionDoor", Guids.PowerDimensionDoor)
            .SetDisplayName(Loc.Str("PW.DimensionDoor.Name", "Dimension Door, Psionic", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.DimensionDoor.Desc",
                "You instantly transfer yourself from your current location to any other spot within range. You can travel up to 400 feet + 40 feet per manifester level beyond 8th."))
            .SetIcon(AbilityRefs.DimensionDoor.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Long)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(4, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Conjuration)
            .OnConfigure(bp =>
            {
                var src = AbilityRefs.DimensionDoor.Reference.Get();
                var teleport = src.ComponentsArray.OfType<AbilityCustomDimensionDoor>().FirstOrDefault();
                if (teleport != null)
                    bp.ComponentsArray = [.. bp.ComponentsArray, teleport];
            })
            .Configure();
    }
}
