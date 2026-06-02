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

/// <summary>
/// Concealing Amorpha, Greater (Metacreativity â†' Conjuration)  -  Surround yourself with quasi-real
/// psionic matter, gaining displacement (50% miss chance).
/// Reuses WoTR's DisplacementBuff for the actual miss-chance mechanic.
/// </summary>
public static class ConcealingAmorphaGreater
{
    public static void Configure()
    {
        var icon = AbilityRefs.Displacement.Reference.Get().Icon;

        AbilityConfigurator.New("PWConcealingAmorphaGreater", Guids.PowerConcealingAmorphaGreater)
            .SetDisplayName(Loc.Str("PW.ConcealingAmorphaGreater.Name", "Concealing Amorpha, Greater", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.ConcealingAmorphaGreater.Desc",
                "You manifest a layer of quasi-real psionic matter that obscures your true location. Attacks against you have a 50% miss chance.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[ConcealingAmorphaGreater] applying 50% miss chance (1 round/ML)", LogRank = true })
                    .ApplyBuff(BuffRefs.DisplacementBuff.ToString(), ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(3, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Conjuration)
            .Configure();
    }
}
