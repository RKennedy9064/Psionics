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
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class PsionicEndowment
{
    public static void Configure()
    {
        BuffConfigurator.New("PsionicEndowmentBuff", Guids.PsionicEndowmentBuff)
            .SetDisplayName(Loc.Str("PW.PsionicEndowment.Name", "Psionic Endowment"))
            .SetDescription(Loc.Str("PW.PsionicEndowment.Desc",
                "While psionically focused, your manifested powers gain a +1 bonus to their save DCs."))
            .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
            .AddIncreaseSpellDC(spellsOnly: false, value: ContextValues.Constant(1))
            .Configure();

        FeatureConfigurator.New("PsionicEndowmentFeat", Guids.PsionicEndowmentFeat)
            .SetDisplayName(Loc.Str("PW.PsionicEndowment.Name", "Psionic Endowment"))
            .SetDescription(Loc.Str("PW.PsionicEndowment.Desc",
                "While psionically focused, your manifested powers gain a +1 bonus to their save DCs."))
            .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
            .SetGroups(FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .AddFactContextActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.PsionicEndowmentBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.PsionicEndowmentBuff))
            .Configure();

        BuffConfigurator.For(Guids.PsionicFocusBuff)
            .AddBuffActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicEndowmentFeat),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.PsionicEndowmentBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.PsionicEndowmentBuff))
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.PsionicEndowmentFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.PsionicEndowmentFeat);
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
