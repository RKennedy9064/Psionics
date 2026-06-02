using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class SteadfastPerception
{
    public static void Configure()
    {
        var icon = AbilityRefs.SeeInvisibility.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWSteadfastPerceptionBuff", Guids.PowerSteadfastPerceptionBuff)
            .SetDisplayName(Loc.Str("PW.SteadfastPerception.BuffName", "Steadfast Perception", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.SteadfastPerception.BuffDesc",
                "Your psionic senses pierce deception: +6 competence bonus to Perception, and you are immune to blindness and deafness.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SkillPerception, value: 6)
            .AddConditionImmunity(UnitCondition.Blindness)
            .Configure();

        AbilityConfigurator.New("PWSteadfastPerception", Guids.PowerSteadfastPerception)
            .SetDisplayName(Loc.Str("PW.SteadfastPerception.Name", "Steadfast Perception", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.SteadfastPerception.Desc",
                "Your psionic senses become impossibly sharp. You gain a +6 competence bonus to Perception checks and become immune to blindness and deafness.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(4, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(
                    buff,
                    ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Divination)
            .Configure();
    }
}
