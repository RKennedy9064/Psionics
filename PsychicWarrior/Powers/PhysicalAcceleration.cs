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

public static class PhysicalAcceleration
{
    public static void Configure()
    {
        var icon = AbilityRefs.Haste.Reference.Get().Icon;

        AbilityConfigurator.New("PWPhysicalAcceleration", Guids.PowerPhysicalAcceleration)
            .SetDisplayName(Loc.Str("PW.PhysicalAcceleration.Name", "Physical Acceleration"))
            .SetDescription(Loc.Str("PW.PhysicalAcceleration.Desc",
                "You psionically accelerate your body. You gain the benefits of haste: +1 bonus on attack rolls, +1 dodge bonus to AC and Reflex saves, +30 ft. enhancement to speed, and one extra attack at your highest base attack bonus."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[PhysicalAcceleration] applying haste (1 round/ML)", LogRank = true })
                    .ApplyBuff(BuffRefs.HasteBuff.ToString(), ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(3, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
