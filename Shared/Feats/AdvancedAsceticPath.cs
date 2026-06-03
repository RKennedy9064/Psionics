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

public static class AdvancedAsceticPath
{
    public static void Configure()
    {
        FeatureConfigurator.New("AdvancedAsceticPathFeat", Guids.AdvancedAsceticPathFeat)
            .SetDisplayName(Loc.Str("PW.AdvancedAsceticPath.Name", "Advanced Ascetic Path"))
            .SetDescription(Loc.Str("PW.AdvancedAsceticPath.Desc",
                "Your mastery of the Ascetic path deepens. The competence bonus granted by your Ascetic Trance now also applies to all saving throws."))
            .SetIcon(AbilityRefs.TrueStrike.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.AsceticTrance)
            .AddPrerequisiteFeature(Guids.PsionicDodgeFeat)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteClassLevel(Guids.PsychicWarriorClass, 10)
            .AddComponent(new AdvancedAsceticSavingThrowBonus())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.AdvancedAsceticPathFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.AdvancedAsceticPathFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.AdvancedAsceticPathFeat);
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
