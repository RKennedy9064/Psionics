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

public static class PsionicDodge
{
    public static void Configure()
    {
        // Intermediate buff — source label shows "Psionic Dodge" in the AC tooltip.
        BuffConfigurator.New("PsionicDodgeBuff", Guids.PsionicDodgeBuff)
            .SetDisplayName(Loc.Str("PW.PsionicDodge.Name", "Psionic Dodge"))
            .SetDescription(Loc.Str("PW.PsionicDodge.Desc",
                "While psionically focused, you gain a +1 dodge bonus to Armor Class."))
            .SetIcon(FeatureRefs.Dodge.Reference.Get().Icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.AC, value: 1)
            .Configure();

        FeatureConfigurator.New("PsionicDodgeFeat", Guids.PsionicDodgeFeat)
            .SetDisplayName(Loc.Str("PW.PsionicDodge.Name", "Psionic Dodge"))
            .SetDescription(Loc.Str("PW.PsionicDodge.Desc",
                "While psionically focused, you gain a +1 dodge bonus to Armor Class."))
            .SetIcon(FeatureRefs.Dodge.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .AddFactContextActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.PsionicDodgeBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.PsionicDodgeBuff))
            .Configure();

        // When focus is gained, apply the bonus buff if the unit has this feat.
        // When focus is lost, the bonus buff is removed alongside it.
        BuffConfigurator.For(Guids.PsionicFocusBuff)
            .AddBuffActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicDodgeFeat),
                    ifTrue: ActionsBuilder.New()
                        .Add(new ContextActionLogStat { Stat = StatType.AC, Label = "[PsionicDodge] before AC" })
                        .ApplyBuffPermanent(Guids.PsionicDodgeBuff)
                        .Add(new ContextActionLogStat { Stat = StatType.AC, Label = "[PsionicDodge] after  AC (expect +1 dodge)" })),
                deactivated: ActionsBuilder.New()
                    .Add(new ContextActionLogStat { Stat = StatType.AC, Label = "[PsionicDodge] focus lost — AC (pre-remove)" })
                    .RemoveBuff(Guids.PsionicDodgeBuff))
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.PsionicDodgeFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.PsionicDodgeFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.PsionicDodgeFeat);
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
