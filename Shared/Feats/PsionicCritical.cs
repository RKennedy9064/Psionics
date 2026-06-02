using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Psionics.HarmonyPatches;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class PsionicCritical
{
    public static void Configure()
    {
        PsionicProficiencyPatch.RegisterPsionicFeat(Guids.PsionicCriticalFeat);

        FeatureConfigurator.New("PsionicCriticalFeat", Guids.PsionicCriticalFeat)
            .SetDisplayName(Loc.Str("PW.PsionicCritical.Name", "Psionic Critical"))
            .SetDescription(Loc.Str("PW.PsionicCritical.Desc",
                "While psionically focused, your critical hits deal an additional 1d8 points of damage."))
            .SetIcon(FeatureRefs.VitalStrikeFeature.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddComponent(new PsionicCriticalDamage())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.PsionicCriticalFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.PsionicCriticalFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.PsionicCriticalFeat);
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
