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

public static class UbiquitousVision
{
    public static void Configure()
    {
        var icon = AbilityRefs.Foresight.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWUbiquitousVisionBuff", Guids.PowerUbiquitousVisionBuff)
            .SetDisplayName(Loc.Str("PW.UbiquitousVision.BuffName", "Ubiquitous Vision"))
            .SetDescription(Loc.Str("PW.UbiquitousVision.BuffDesc",
                "360-degree psionic vision. You gain +4 insight bonus to Perception and cannot be flanked."))
            .SetIcon(icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Insight, stat: StatType.SkillPerception, value: 4)
            .AddFortification(50)
            .Configure();

        AbilityConfigurator.New("PWUbiquitousVision", Guids.PowerUbiquitousVision)
            .SetDisplayName(Loc.Str("PW.UbiquitousVision.Name", "Ubiquitous Vision"))
            .SetDescription(Loc.Str("PW.UbiquitousVision.Desc",
                "Your psionic senses expand to perceive all directions simultaneously. You gain a +4 insight bonus to Perception checks and a 50% chance to negate flanking and sneak attacks."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.10MinPerML", "10 minutes per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[UbiquitousVision] applying +4 Perception + fortification", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.TenMinutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(3, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Divination)
            .Configure();
    }
}
