using System.Collections.Generic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

/// <summary>
/// Animal Affinity (Transmutation)  -  Gain +4 enhancement bonus to one ability score of your
/// choice (Strength, Dexterity, Constitution, Intelligence, Wisdom, or Charisma)/level.
///
/// Implemented as a parent ability with six variants (alchemist-mutagen pattern). The parent is
/// what shows up in the spellbook and action bar; clicking expands to show the six stat options.
/// </summary>
public static class AnimalAffinity
{
    public static void Configure()
    {
        var iconStr = AbilityRefs.BullsStrength.Reference.Get().Icon;
        var iconDex = AbilityRefs.CatsGrace.Reference.Get().Icon;
        var iconCon = AbilityRefs.BearsEndurance.Reference.Get().Icon;
        var iconInt = AbilityRefs.FoxsCunning.Reference.Get().Icon;
        var iconWis = AbilityRefs.OwlsWisdom.Reference.Get().Icon;
        var iconCha = AbilityRefs.EaglesSplendor.Reference.Get().Icon;

        BuildVariant(
            "PWAnimalAffinityStrength", Guids.PowerAnimalAffinityStrength,
            "PWAnimalAffinityStrengthBuff", Guids.PowerAnimalAffinityStrengthBuff,
            "Animal Affinity  -  Strength", "Boar",
            StatType.Strength, iconStr);

        BuildVariant(
            "PWAnimalAffinityDexterity", Guids.PowerAnimalAffinityDexterity,
            "PWAnimalAffinityDexterityBuff", Guids.PowerAnimalAffinityDexterityBuff,
            "Animal Affinity  -  Dexterity", "Cat",
            StatType.Dexterity, iconDex);

        BuildVariant(
            "PWAnimalAffinityConstitution", Guids.PowerAnimalAffinityConstitution,
            "PWAnimalAffinityConstitutionBuff", Guids.PowerAnimalAffinityConstitutionBuff,
            "Animal Affinity  -  Constitution", "Bear",
            StatType.Constitution, iconCon);

        BuildVariant(
            "PWAnimalAffinityIntelligence", Guids.PowerAnimalAffinityIntelligence,
            "PWAnimalAffinityIntelligenceBuff", Guids.PowerAnimalAffinityIntelligenceBuff,
            "Animal Affinity  -  Intelligence", "Fox",
            StatType.Intelligence, iconInt);

        BuildVariant(
            "PWAnimalAffinityWisdom", Guids.PowerAnimalAffinityWisdom,
            "PWAnimalAffinityWisdomBuff", Guids.PowerAnimalAffinityWisdomBuff,
            "Animal Affinity  -  Wisdom", "Owl",
            StatType.Wisdom, iconWis);

        BuildVariant(
            "PWAnimalAffinityCharisma", Guids.PowerAnimalAffinityCharisma,
            "PWAnimalAffinityCharismaBuff", Guids.PowerAnimalAffinityCharismaBuff,
            "Animal Affinity  -  Charisma", "Eagle",
            StatType.Charisma, iconCha);

        // Parent ability  -  the entry in the spellbook/action bar. Clicking expands the variant menu.
        AbilityConfigurator.New("PWAnimalAffinity", Guids.PowerAnimalAffinity)
            .SetDisplayName(Loc.Str("PW.AnimalAffinity.Name", "Animal Affinity", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.AnimalAffinity.Desc",
                "Channel the spirit of a great beast: gain a +4 enhancement bonus to one ability score of your choice (Strength, Dexterity, Constitution, Intelligence, Wisdom, or Charisma).",
                tagEncyclopediaEntries: false))
            .SetIcon(iconStr)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(2, Guids.SpellList)
            .AddAbilityVariants(variants:
            [
                Guids.PowerAnimalAffinityStrength,
                Guids.PowerAnimalAffinityDexterity,
                Guids.PowerAnimalAffinityConstitution,
                Guids.PowerAnimalAffinityIntelligence,
                Guids.PowerAnimalAffinityWisdom,
                Guids.PowerAnimalAffinityCharisma,
            ])
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }

    private static BlueprintAbility BuildVariant(
        string abilityName, string abilityGuid,
        string buffName, string buffGuid,
        string displayName, string beastName,
        StatType stat, UnityEngine.Sprite icon)
    {
        var buff = BuffConfigurator.New(buffName, buffGuid)
            .SetDisplayName(Loc.Str($"PW.{abilityName}.BuffName", displayName, tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str($"PW.{abilityName}.BuffDesc",
                $"You channel the spirit of the {beastName}, gaining a +4 enhancement bonus to {stat}.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: stat, value: 4)
            .Configure();

        return AbilityConfigurator.New(abilityName, abilityGuid)
            .SetDisplayName(Loc.Str($"PW.{abilityName}.Name", displayName, tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str($"PW.{abilityName}.Desc",
                $"Channel the spirit of the {beastName}, gaining +4 enhancement bonus to {stat}.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(
                    buff,
                    ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
