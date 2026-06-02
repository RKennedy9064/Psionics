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

public static class FreedomOfMovement
{
    public static void Configure()
    {
        var icon = AbilityRefs.FreedomOfMovement.Reference.Get().Icon;

        AbilityConfigurator.New("PWFreedomOfMovement", Guids.PowerFreedomOfMovement)
            .SetDisplayName(Loc.Str("PW.FreedomOfMovement.Name", "Freedom of Movement, Psionic", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.FreedomOfMovement.Desc",
                "This power enables you to move and attack normally, even under the influence of magic that usually impedes movement."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[FreedomOfMovement] applying FoM (1 min/ML)", LogRank = true })
                    .ApplyBuff(BuffRefs.FreedomOfMovementBuff.ToString(), ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(4, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Abjuration)
            .Configure();
    }
}
