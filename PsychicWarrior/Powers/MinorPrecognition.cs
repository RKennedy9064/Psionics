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

public static class MinorPrecognition
{
    public static void Configure()
    {
        var icon = AbilityRefs.Guidance.Reference.Get().Icon;

        AbilityConfigurator.New("PWMinorPrecognition", Guids.PowerMinorPrecognition)
            .SetDisplayName(Loc.Str("PW.MinorPrecognition.Name", "Minor Precognition"))
            .SetDescription(Loc.Str("PW.MinorPrecognition.Desc",
                "Your psionic foresight grants a +1 competence bonus on your next attack roll, saving throw, or skill check."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1Min", "1 minute"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[MinorPrecognition] applying +1 competence guidance buff" })
                    .ApplyBuff(BuffRefs.GuidanceBuff.ToString(), ContextDuration.Fixed(1, DurationRate.Minutes)))
            .AddSpellListComponent(1, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Divination)
            .Configure();
    }
}
