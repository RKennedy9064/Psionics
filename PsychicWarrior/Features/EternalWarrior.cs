using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features;

/// <summary>
/// Level 20 capstone. Once per day as a free action while psionically focused, for 5 minutes:
/// adds Wisdom modifier as an insight bonus to attack rolls, damage rolls, AC, all saves,
/// initiative, and all skill checks. Stacks with active trances and Pathweaving.
/// </summary>
public static class EternalWarrior
{
    public static void Configure()
    {
        var icon = AbilityRefs.OwlsWisdom.Reference.Get().Icon;

        AbilityResourceConfigurator.New("EternalWarriorResource", Guids.EternalWarriorResource)
            .SetLocalizedName(Loc.Str("PW.EternalWarrior.ResourceName", "Eternal Warrior Uses", tagEncyclopediaEntries: false))
            .SetLocalizedDescription(Loc.Str("PW.EternalWarrior.ResourceDesc", "Daily uses of Eternal Warrior.", tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetMaxAmount(ResourceAmountBuilder.New(1))
            .Configure();

        var buff = BuffConfigurator.New("EternalWarriorBuff", Guids.EternalWarriorBuff)
            .SetDisplayName(Loc.Str("PW.EternalWarrior.BuffName", "Eternal Warrior", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.EternalWarrior.BuffDesc",
                "Your Wisdom modifier is added as an insight bonus to attack rolls, damage rolls, AC, all saving throws, initiative, and all skill checks.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddContextStatBonus(stat: StatType.AdditionalAttackBonus, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.AdditionalDamage, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.AC, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SaveFortitude, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SaveReflex, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SaveWill, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.Initiative, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillAthletics, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillMobility, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillPerception, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillStealth, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillLoreNature, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillLoreReligion, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillKnowledgeArcana, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillKnowledgeWorld, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillPersuasion, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillUseMagicDevice, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextStatBonus(stat: StatType.SkillThievery, descriptor: ModifierDescriptor.Insight, value: ContextValues.Rank())
            .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom))
            .Configure();

        AbilityConfigurator.New("EternalWarriorAbility", Guids.EternalWarriorAbility)
            .SetDisplayName(Loc.Str("PW.EternalWarrior.AbilityName", "Eternal Warrior", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.EternalWarrior.AbilityDesc",
                "Free Action, once per day while psionically focused. For 5 minutes, add your Wisdom modifier as an insight bonus to attack rolls, damage rolls, AC, all saving throws, initiative, and all skill checks. Stacks with active path trances and Pathweaving.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Free)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityResourceLogic(amount: 1, isSpendResource: true, requiredResource: Guids.EternalWarriorResource)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(50)))
            .Configure();

        FeatureConfigurator.New("EternalWarrior", Guids.EternalWarriorFeature)
            .SetDisplayName(Loc.Str("PW.EternalWarrior.Name", "Eternal Warrior", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.EternalWarrior.Desc",
                "At 20th level, once per day as a free action while psionically focused, you may activate Eternal Warrior for 5 minutes. " +
                "During this time, you add your Wisdom modifier as an insight bonus to attack rolls, damage rolls, AC, all saving throws, initiative, and all skill checks. " +
                "This stacks with your active path trances and Pathweaving.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFacts([Guids.EternalWarriorAbility])
            .AddAbilityResources(resource: Guids.EternalWarriorResource, restoreAmount: true)
            .Configure();
    }
}
