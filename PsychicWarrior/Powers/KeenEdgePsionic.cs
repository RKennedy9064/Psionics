using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class KeenEdgePsionic
{
    public static void Configure()
    {
        var icon = AbilityRefs.KeenEdge.Reference.Get().Icon;

        AbilityConfigurator.New("PWKeenEdge", Guids.PowerKeenEdgePsionic)
            .SetDisplayName(Loc.Str("PW.KeenEdgePsionic.Name", "Keen Edge, Psionic", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.KeenEdgePsionic.Desc",
                "Psionic energy hones the edge of your weapon, doubling its threat range. The effect does not stack with other effects that increase threat range."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.10MinPerML", "10 minutes per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[KeenEdgePsionic] applying keen edge primary weapon (10 min/ML)", LogRank = true })
                    .ApplyBuff(BuffRefs.KeenEdgePrimaryBuff.ToString(), ContextDuration.Variable(ContextValues.Rank(), DurationRate.TenMinutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(3, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
