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
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class SpeedOfThought
{
    public static void Configure()
    {
        // Intermediate buff — source label shows "Speed of Thought" in the speed tooltip.
        BuffConfigurator.New("SpeedOfThoughtBuff", Guids.SpeedOfThoughtBuff)
            .SetDisplayName(Loc.Str("PW.SpeedOfThought.Name", "Speed of Thought"))
            .SetDescription(Loc.Str("PW.SpeedOfThought.Desc",
                "While psionically focused, you gain a +10 ft enhancement bonus to your land speed."))
            .SetIcon(FeatureRefs.Improved_Initiative.Reference.Get().Icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: StatType.Speed, value: 10)
            .Configure();

        FeatureConfigurator.New("SpeedOfThoughtFeat", Guids.SpeedOfThoughtFeat)
            .SetDisplayName(Loc.Str("PW.SpeedOfThought.Name", "Speed of Thought"))
            .SetDescription(Loc.Str("PW.SpeedOfThought.Desc",
                "While psionically focused, you gain a +10 ft enhancement bonus to your land speed."))
            .SetIcon(FeatureRefs.Improved_Initiative.Reference.Get().Icon)
            .SetGroups(FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .AddFactContextActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.SpeedOfThoughtBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.SpeedOfThoughtBuff))
            .Configure();

        // When focus is gained, apply the speed buff if the unit has this feat.
        // When focus is lost, the speed buff is removed alongside it.
        BuffConfigurator.For(Guids.PsionicFocusBuff)
            .AddBuffActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.SpeedOfThoughtFeat),
                    ifTrue: ActionsBuilder.New()
                        .Add(new ContextActionLogStat { Stat = StatType.Speed, Label = "[SpeedOfThought] before Speed" })
                        .ApplyBuffPermanent(Guids.SpeedOfThoughtBuff)
                        .Add(new ContextActionLogStat { Stat = StatType.Speed, Label = "[SpeedOfThought] after  Speed (expect +10)" })),
                deactivated: ActionsBuilder.New()
                    .Add(new ContextActionLogStat { Stat = StatType.Speed, Label = "[SpeedOfThought] focus lost — Speed (pre-remove)" })
                    .RemoveBuff(Guids.SpeedOfThoughtBuff))
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.SpeedOfThoughtFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.SpeedOfThoughtFeat);
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
