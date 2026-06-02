using System.Collections.Generic;
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
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features.Paths;

/// <summary>
/// Phase 7 — Twisting Paths (lvl 11) and Pathweaving (lvl 15) mechanics.
///
/// Twisting Paths: swift-action ability granting a 1-round combat buff (no focus cost).
/// Represents the psychic warrior's mental agility from training in two paths.
///
/// Pathweaving: free-action per-day ability granting a strong 5-minute buff. Uses scale
/// with PW level: 1 at level 15, +1 per 3 levels past 15.
/// </summary>
public static class TwistingPathsPathweaving
{
    public static void Configure()
    {
        ConfigureTwistingPaths();
        ConfigurePathweaving();
    }

    private static void ConfigureTwistingPaths()
    {
        var icon = AbilityRefs.SpellResistance.Reference.Get().Icon;

        // All 12 trance activatables — Twisting Paths reduces their toggle cost from Standard to Swift
        var trances = new List<Blueprint<BlueprintActivatableAbilityReference>>
        {
            Guids.WeaponmasterTranceActivatable,
            Guids.BrawlerTranceActivatable,
            Guids.ArcherTranceActivatable,
            Guids.AsceticTranceActivatable,
            Guids.AssassinsTranceActivatable,
            Guids.DervishTranceActivatable,
            Guids.FeralWarriorTranceActivatable,
            Guids.GladiatorTranceActivatable,
            Guids.InfiltratorTranceActivatable,
            Guids.InterceptorTranceActivatable,
            Guids.MindKnightTranceActivatable,
            Guids.SurvivorTranceActivatable,
        };

        FeatureConfigurator.New("TwistingPaths", Guids.TwistingPaths)
            .SetDisplayName(Loc.Str("PW.TwistingPaths.Name", "Twisting Paths", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.TwistingPaths.Desc",
                "At 12th level, your training lets you slip between trances effortlessly. The action cost to toggle any of your path trances is reduced from a standard action to a swift action, letting you switch between your two paths' trances mid-combat.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddChangeActivatableAbilitiesCommandType(
                activatableAbilities: trances,
                newCommandType: UnitCommand.CommandType.Swift)
            .Configure();
    }

    private static void ConfigurePathweaving()
    {
        var icon = AbilityRefs.BullsStrength.Reference.Get().Icon;

        // Per-day resource: 1 at level 15, +1 per 3 PW levels past 15
        AbilityResourceConfigurator.New("PathweavingResource", Guids.PathweavingResource)
            .SetLocalizedName(Loc.Str("PW.PathweavingResource.Name", "Pathweaving Uses", tagEncyclopediaEntries: false))
            .SetLocalizedDescription(Loc.Str("PW.PathweavingResource.Desc",
                "Daily uses of Pathweaving. Recovers after a full rest.", tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetMaxAmount(
                ResourceAmountBuilder.New(0)
                    .IncreaseByLevelStartPlusDivStep(
                        classes: [Guids.PsychicWarriorClass],
                        startingLevel: 15,
                        startingBonus: 1,
                        levelsPerStep: 3,
                        bonusPerStep: 1))
            .Configure();

        // Pathweaving buff: combined trance benefits for 5 minutes
        var buff = BuffConfigurator.New("PathweavingBuff", Guids.PathweavingBuff)
            .SetDisplayName(Loc.Str("PW.PathweavingBuff.Name", "Pathweaving", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.PathweavingBuff.Desc",
                "Your trances merge: +2 competence attack and damage, +2 dodge AC, +2 to all saves.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalAttackBonus, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.AdditionalDamage, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.AC, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SaveFortitude, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SaveReflex, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: StatType.SaveWill, value: 2)
            .Configure();

        // Ability: free action, consumes one PathweavingResource, applies the buff for 5 minutes
        AbilityConfigurator.New("PathweavingAbility", Guids.PathweavingAbility)
            .SetDisplayName(Loc.Str("PW.PathweavingAbility.Name", "Pathweaving", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.PathweavingAbility.Desc",
                "Free Action. Weave both your trances together for 5 minutes, gaining +2 competence to attack and damage, +2 dodge AC, and +2 to all saves. Usable a number of times per day equal to 1 + your Psychic Warrior level above 15 divided by 3.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Free)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityResourceLogic(amount: 1, isSpendResource: true, requiredResource: Guids.PathweavingResource)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(50)))  // 50 rounds = 5 minutes
            .Configure();

        // Replace the placeholder Pathweaving feature
        FeatureConfigurator.New("Pathweaving", Guids.Pathweaving)
            .SetDisplayName(Loc.Str("PW.Pathweaving.Name", "Pathweaving", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.Pathweaving.Desc",
                "At 15th level, you can weave both your paths' trances together. As a free action a number of times per day " +
                "(1 at level 15, +1 every 3 Psychic Warrior levels thereafter), you gain +2 competence to attack and damage, " +
                "+2 dodge AC, and +2 to all saves for 5 minutes.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFacts([Guids.PathweavingAbility])
            .AddAbilityResources(resource: Guids.PathweavingResource, restoreAmount: true)
            .Configure();
    }
}
