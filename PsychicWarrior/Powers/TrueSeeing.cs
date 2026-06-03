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

public static class TrueSeeing
{
    public static void Configure()
    {
        var icon = AbilityRefs.TrueSeeing.Reference.Get().Icon;

        AbilityConfigurator.New("PWTrueSeeing", Guids.PowerTrueSeeing)
            .SetDisplayName(Loc.Str("PW.TrueSeeing.Name", "True Seeing, Psionic", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.TrueSeeing.Desc",
                "You see all things as they actually are. You see through normal and magical darkness, see exact locations of creatures under blur or displacement, and see invisible creatures."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[TrueSeeing] applying true seeing (1 min/ML)", LogRank = true })
                    .ApplyBuff(BuffRefs.TrueSeeingBuff.ToString(), ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(5, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Divination)
            .Configure();
    }
}
