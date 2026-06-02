using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class AdvancedMindKnightPath
{
    public static void Configure()
    {
        FeatureConfigurator.New("AdvancedMindKnightPathFeat", Guids.AdvancedMindKnightPathFeat)
            .SetDisplayName(Loc.Str("PW.AdvancedMindKnightPath.Name", "Advanced Mind Knight Path"))
            .SetDescription(Loc.Str("PW.AdvancedMindKnightPath.Desc",
                "Your mastery of the Mind Knight path deepens. When using the Mind Knight Maneuver, you gain a competence bonus to attack rolls equal to +1 per four psychic warrior levels."))
            .SetIcon(AbilityRefs.MagicWeapon.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.MindKnightTrance)
            .AddPrerequisiteFeature(Guids.PsionicMeditationFeat)
            .AddPrerequisiteFeature(Guids.PsionicWeaponFeat)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteStatValue(StatType.Wisdom, 13)
            .AddPrerequisiteClassLevel(Guids.PsychicWarriorClass, 10)
            .AddComponent(new AdvancedMindKnightAttackBonus())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.AdvancedMindKnightPathFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.AdvancedMindKnightPathFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.AdvancedMindKnightPathFeat);
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
