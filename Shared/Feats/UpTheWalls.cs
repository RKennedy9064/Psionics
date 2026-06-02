using System.Linq;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class UpTheWalls
{
    public static void Configure()
    {
        BuffConfigurator.New("UpTheWallsBuff", Guids.UpTheWallsBuff)
            .SetDisplayName(Loc.Str("PW.UpTheWalls.Name", "Up the Walls"))
            .SetDescription(Loc.Str("PW.UpTheWalls.Desc",
                "While psionically focused, you do not provoke attacks of opportunity when moving."))
            .SetIcon(FeatureRefs.Mobility.Reference.Get().Icon)
            .AddCondition(UnitCondition.ImmuneToAttackOfOpportunity)
            .Configure();

        FeatureConfigurator.New("UpTheWallsFeat", Guids.UpTheWallsFeat)
            .SetDisplayName(Loc.Str("PW.UpTheWalls.Name", "Up the Walls"))
            .SetDescription(Loc.Str("PW.UpTheWalls.Desc",
                "While psionically focused, you do not provoke attacks of opportunity when moving."))
            .SetIcon(FeatureRefs.Mobility.Reference.Get().Icon)
            .SetGroups(FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .AddFactContextActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.UpTheWallsBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.UpTheWallsBuff))
            .Configure();

        BuffConfigurator.For(Guids.PsionicFocusBuff)
            .AddBuffActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.UpTheWallsFeat),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.UpTheWallsBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.UpTheWallsBuff))
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.UpTheWallsFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.UpTheWallsFeat);
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
