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

public static class AdvancedArcherPath
{
    public static void Configure()
    {
        FeatureConfigurator.New("AdvancedArcherPathFeat", Guids.AdvancedArcherPathFeat)
            .SetDisplayName(Loc.Str("PW.AdvancedArcherPath.Name", "Advanced Archer Path"))
            .SetDescription(Loc.Str("PW.AdvancedArcherPath.Desc",
                "Your mastery of the Archer path deepens. Half the competence bonus granted by your Archer Trance also applies to damage rolls with ranged and thrown weapons (natural weapons excluded)."))
            .SetIcon(FeatureRefs.RapidShotFeature.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.ArcherTrance)
            .AddPrerequisiteFeature(FeatureRefs.PointBlankShot.ToString())
            .AddPrerequisiteFeature(FeatureRefs.PreciseShot.ToString())
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteClassLevel(Guids.PsychicWarriorClass, 10)
            .AddComponent(new AdvancedArcherDamageBonus())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.AdvancedArcherPathFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.AdvancedArcherPathFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.AdvancedArcherPathFeat);
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
