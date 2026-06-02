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
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class OakBody
{
    public static void Configure()
    {
        var icon = AbilityRefs.ProtectionFromArrows.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWOakBodyBuff", Guids.PowerOakBodyBuff)
            .SetDisplayName(Loc.Str("PW.OakBody.BuffName", "Oak Body", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.OakBody.BuffDesc",
                "Your body has transformed into living wood. You have DR 10/-, +4 Strength, +4 natural armor, -4 Dexterity, and immunity to mind-affecting effects.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddDamageResistancePhysical(value: ContextValues.Constant(10))
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: StatType.Strength, value: 4)
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: StatType.Dexterity, value: -4)
            .AddStatBonus(descriptor: ModifierDescriptor.NaturalArmor, stat: StatType.AC, value: 4)
            .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.MindAffecting)
            .Configure();

        AbilityConfigurator.New("PWOakBody", Guids.PowerOakBody)
            .SetDisplayName(Loc.Str("PW.OakBody.Name", "Oak Body", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.OakBody.Desc",
                "Your body transforms into living wood. For 1 minute per manifester level you gain DR 10/-, a +4 enhancement bonus to Strength, a +4 natural armor bonus to AC, a âˆ'4 penalty to Dexterity, and immunity to mind-affecting effects.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(6, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(
                    buff,
                    ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
