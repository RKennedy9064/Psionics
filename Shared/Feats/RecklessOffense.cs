using System.Linq;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Psionics.HarmonyPatches;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class RecklessOffense
{
    public static void Configure()
    {
        PsionicProficiencyPatch.RegisterPsionicFeat(Guids.RecklessOffenseFeat);

        var buff = BuffConfigurator.New("RecklessOffenseBuff", Guids.RecklessOffenseBuff)
            .SetDisplayName(Loc.Str("PW.RecklessOffense.BuffName", "Reckless Offense"))
            .SetDescription(Loc.Str("PW.RecklessOffense.BuffDesc",
                "You are attacking recklessly. +2 to attack rolls, –4 to AC."))
            .SetIcon(FeatureRefs.PowerAttackFeature.Reference.Get().Icon)
            .AddStatBonus(descriptor: ModifierDescriptor.UntypedStackable, stat: StatType.AdditionalAttackBonus, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.UntypedStackable, stat: StatType.AC, value: -4)
            .Configure();

        ActivatableAbilityConfigurator.New("RecklessOffenseActivatable", Guids.RecklessOffenseActivatable)
            .SetDisplayName(Loc.Str("PW.RecklessOffense.Name", "Reckless Offense"))
            .SetDescription(Loc.Str("PW.RecklessOffense.Desc",
                "Toggle. While active, you attack recklessly, gaining +2 to melee attack rolls but suffering –4 to AC until the start of your next turn."))
            .SetIcon(FeatureRefs.PowerAttackFeature.Reference.Get().Icon)
            .SetBuff(buff)
            .SetActivationType(AbilityActivationType.Immediately)
            .SetDeactivateImmediately(true)
            .SetGroup(ActivatableAbilityGroup.None)
            .Configure();

        FeatureConfigurator.New("RecklessOffenseFeat", Guids.RecklessOffenseFeat)
            .SetDisplayName(Loc.Str("PW.RecklessOffense.Name", "Reckless Offense"))
            .SetDescription(Loc.Str("PW.RecklessOffense.FeatDesc",
                "You can throw caution to the wind in melee. When this stance is active, you gain a +2 bonus to melee attack rolls but take a –4 penalty to AC. Requires BAB +1."))
            .SetIcon(FeatureRefs.PowerAttackFeature.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
            .AddFacts([Guids.RecklessOffenseActivatable])
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.RecklessOffenseFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.RecklessOffenseFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.RecklessOffenseFeat);
    }

    private static void SafeAddFeatToSelection(string selectionGuid, string featGuid)
    {
        FeatureSelectionConfigurator.For(selectionGuid)
            .OnConfigure(bp =>
            {
                var featRef = BlueprintTool.GetRef<BlueprintFeatureReference>(featGuid);
                bp.m_AllFeatures = [.. bp.m_AllFeatures.Where(f => f.Guid != featRef.Guid), featRef];
            })
            .Configure();
    }
}
