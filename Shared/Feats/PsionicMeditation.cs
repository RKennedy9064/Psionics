using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class PsionicMeditation
{
    public static void Configure()
    {
        FeatureConfigurator.New("PsionicMeditationFeat", Guids.PsionicMeditationFeat)
            .SetDisplayName(Loc.Str("PW.PsionicMeditation.Name", "Psionic Meditation"))
            .SetDescription(Loc.Str("PW.PsionicMeditation.Desc", "You can take a move action to gain psionic focus, rather than a standard action."))
            .SetIcon(AbilityRefs.RestorationLesser.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddFacts([Guids.GainPsionicFocusMoveAbility])
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.PsionicMeditationFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.PsionicMeditationFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.PsionicMeditationFeat);
    }

    private static void SafeAddFeatToSelection(string selectionGuid, string featGuid)
    {
        FeatureSelectionConfigurator.For(selectionGuid)
            .OnConfigure(bp =>
            {
                var featRef = BlueprintTool.GetRef<BlueprintFeatureReference>(featGuid);

                bp.m_AllFeatures = [.. bp.m_AllFeatures.Where(f => f.Guid != featRef.Guid)];

                bp.m_AllFeatures = [.. bp.m_AllFeatures, featRef];
            })
            .Configure();
    }
}