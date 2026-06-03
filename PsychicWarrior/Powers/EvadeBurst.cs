using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class EvadeBurst
{
    public static void Configure()
    {
        var icon = FeatureRefs.Evasion.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWEvadeBurstBuff", Guids.PowerEvadeBurstBuff)
            .SetDisplayName(Loc.Str("PW.EvadeBurst.BuffName", "Evade Burst"))
            .SetDescription(Loc.Str("PW.EvadeBurst.BuffDesc",
                "You have Evasion: on a successful Reflex save against an area effect, you take no damage instead of half."))
            .SetIcon(icon)
            .AddEvasion(SavingThrowType.Reflex)
            .Configure();

        AbilityConfigurator.New("PWEvadeBurst", Guids.PowerEvadeBurst)
            .SetDisplayName(Loc.Str("PW.EvadeBurst.Name", "Evade Burst"))
            .SetDescription(Loc.Str("PW.EvadeBurst.Desc",
                "Swift Action. You psionically attune your reflexes. You gain Evasion: on a successful Reflex save against an area effect, you take no damage instead of half."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[EvadeBurst] applying Evasion", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(3, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
