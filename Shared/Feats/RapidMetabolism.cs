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
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class RapidMetabolism
{
    public static void Configure()
    {
        BuffConfigurator.New("RapidMetabolismBuff", Guids.RapidMetabolismBuff)
            .SetDisplayName(Loc.Str("PW.RapidMetabolism.Name", "Rapid Metabolism"))
            .SetDescription(Loc.Str("PW.RapidMetabolism.Desc",
                "While psionically focused, you gain fast healing scaling with manifester level: FH 1 at ML 1–4, FH 2 at ML 5–9, FH 3 at ML 10–14, FH 4 at ML 15–19, FH 5 at ML 20."))
            .SetIcon(FeatureRefs.Endurance.Reference.Get().Icon)
            .AddBuffActions(
                newRound: ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[RapidMetabolism] fast healing tick", LogRank = true })
                    .HealTarget(ContextDice.Value(DiceType.One, ContextValues.Rank(), 0)))
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (4, 1), (9, 2), (14, 3), (19, 4), (20, 5)))
            .Configure();

        FeatureConfigurator.New("RapidMetabolismFeat", Guids.RapidMetabolismFeat)
            .SetDisplayName(Loc.Str("PW.RapidMetabolism.Name", "Rapid Metabolism"))
            .SetDescription(Loc.Str("PW.RapidMetabolism.Desc",
                "While psionically focused, you gain fast healing scaling with manifester level: FH 1 at ML 1–4, FH 2 at ML 5–9, FH 3 at ML 10–14, FH 4 at ML 15–19, FH 5 at ML 20."))
            .SetIcon(FeatureRefs.Endurance.Reference.Get().Icon)
            .SetGroups(FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .AddFactContextActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.RapidMetabolismBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.RapidMetabolismBuff))
            .Configure();

        BuffConfigurator.For(Guids.PsionicFocusBuff)
            .AddBuffActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.RapidMetabolismFeat),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.RapidMetabolismBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.RapidMetabolismBuff))
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.RapidMetabolismFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.RapidMetabolismFeat);
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
