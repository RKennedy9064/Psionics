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

public static class DimensionSlide
{
    public static void Configure()
    {
        AbilityConfigurator.New("PWDimensionSlide", Guids.PowerDimensionSlide)
            .SetDisplayName(Loc.Str("PW.DimensionSlide.Name", "Dimension Slide"))
            .SetDescription(Loc.Str("PW.DimensionSlide.Desc",
                "You teleport to any visible location within close range (25 ft. + 5 ft./2 levels). You do not provoke attacks of opportunity from this movement."))
            .SetIcon(AbilityRefs.Blink.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Close)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(3, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
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
