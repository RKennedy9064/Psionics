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

/// <summary>
/// Greater Psionic Endowment: improves Psionic Endowment's save-DC bonus from +1 to +2. Implemented as a
/// second focus-gated +1 IncreaseSpellDC buff that stacks with Psionic Endowment's, so the total while
/// focused is +2. Mirrors the dual-hook focus pattern used by Psionic Endowment.
/// </summary>
public static class GreaterPsionicEndowment
{
    public static void Configure()
    {
        BuffConfigurator.New("GreaterPsionicEndowmentBuff", Guids.GreaterPsionicEndowmentBuff)
            .SetDisplayName(Loc.Str("PW.GreaterPsionicEndowment.Name", "Greater Psionic Endowment"))
            .SetDescription(Loc.Str("PW.GreaterPsionicEndowment.Desc",
                "While psionically focused, your manifested powers gain an additional +1 bonus to their save DCs (+2 total with Psionic Endowment)."))
            .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
            .AddIncreaseSpellDC(spellsOnly: false, value: ContextValues.Constant(1))
            .Configure();

        FeatureConfigurator.New("GreaterPsionicEndowmentFeat", Guids.GreaterPsionicEndowmentFeat)
            .SetDisplayName(Loc.Str("PW.GreaterPsionicEndowment.Name", "Greater Psionic Endowment"))
            .SetDescription(Loc.Str("PW.GreaterPsionicEndowment.Desc",
                "While psionically focused, your manifested powers gain an additional +1 bonus to their save DCs (+2 total with Psionic Endowment)."))
            .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
            .SetGroups(FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.PsionicEndowmentFeat)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .AddFactContextActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.GreaterPsionicEndowmentBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.GreaterPsionicEndowmentBuff))
            .Configure();

        BuffConfigurator.For(Guids.PsionicFocusBuff)
            .AddBuffActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.GreaterPsionicEndowmentFeat),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.GreaterPsionicEndowmentBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.GreaterPsionicEndowmentBuff))
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.GreaterPsionicEndowmentFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.GreaterPsionicEndowmentFeat);
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
