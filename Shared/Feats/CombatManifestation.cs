using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class CombatManifestation
{
    public static void Configure()
    {
        FeatureConfigurator.New("CombatManifestationFeat", Guids.CombatManifestationFeat)
            .SetDisplayName(Loc.Str("PW.CombatManifestation.Name", "Combat Manifestation"))
            .SetDescription(Loc.Str("PW.CombatManifestation.Desc",
                "You get a +4 bonus on concentration checks made to manifest a power while on the defensive or while grappling or pinned."))
            .SetIcon(FeatureRefs.CombatCasting.Reference.Get().Icon)
            .SetGroups(FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddConcentrationBonus(value: ContextValues.Constant(4))
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.CombatManifestationFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.CombatManifestationFeat);
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
