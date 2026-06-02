using System.Linq;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class CriticalRefocus
{
    public static void Configure()
    {
        FeatureConfigurator.New("CriticalRefocusFeat", Guids.CriticalRefocusFeat)
            .SetDisplayName(Loc.Str("PW.CriticalRefocus.Name", "Critical Refocus"))
            .SetDescription(Loc.Str("PW.CriticalRefocus.Desc",
                "When you score a critical hit, you regain your psionic focus."))
            .SetIcon(AbilityRefs.TrueStrike.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff, negate: true),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.PsionicFocusBuff, toCaster: true)),
                criticalHit: true,
                onlyHit: true)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.CriticalRefocusFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.CriticalRefocusFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.CriticalRefocusFeat);
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
