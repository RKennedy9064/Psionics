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

public static class DisintegratePsionic
{
    public static void Configure()
    {
        AbilityConfigurator.New("PWDisintegrate", Guids.PowerDisintegratePsionic)
            .SetDisplayName(Loc.Str("PW.Disintegrate.Name", "Disintegrate, Psionic", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.Disintegrate.Desc",
                "A thin, green ray springs from your pointing finger. You must make a successful ranged touch attack to hit. Any creature struck by the ray takes 2d6 damage per manifester level (maximum 40d6). Any creature reduced to 0 or fewer hit points by this power is entirely disintegrated."))
            .SetIcon(AbilityRefs.Disintegrate.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Medium)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
            .AddSpellListComponent(6, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .OnConfigure(bp =>
            {
                var src = AbilityRefs.Disintegrate.Reference.Get();
                var runAction = src.ComponentsArray.OfType<AbilityEffectRunAction>().FirstOrDefault();
                if (runAction != null)
                    bp.ComponentsArray = [.. bp.ComponentsArray, runAction];
                var deliver = src.ComponentsArray.OfType<AbilityDeliverProjectile>().FirstOrDefault();
                if (deliver != null)
                    bp.ComponentsArray = [.. bp.ComponentsArray, deliver];
            })
            .Configure();
    }
}
