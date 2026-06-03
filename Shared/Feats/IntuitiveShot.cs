using System.Linq;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

/// <summary>
/// Intuitive Shot [Psionic]: while psionically focused, your ranged attacks deal extra damage equal to your
/// Wisdom modifier. (Following the mod's convention — as with Psionic Shot — the rider applies to every
/// ranged hit while focused, which already covers what tabletop "Greater Intuitive Shot" would add, so
/// Greater is intentionally not implemented.) Precision-immune creatures are unaffected.
/// </summary>
public static class IntuitiveShot
{
    public static void Configure()
    {
        FeatureConfigurator.New("IntuitiveShotFeat", Guids.IntuitiveShotFeat)
            .SetDisplayName(Loc.Str("PW.IntuitiveShot.Name", "Intuitive Shot"))
            .SetDescription(Loc.Str("PW.IntuitiveShot.Desc",
                "While psionically focused, your ranged attacks deal additional damage equal to your Wisdom modifier."))
            .SetIcon(FeatureRefs.PointBlankShot.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.PsionicShotFeat)
            .AddPrerequisiteStatValue(StatType.Wisdom, 13)
            .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom))
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().DealDamage(
                        DamageTypes.Physical(),
                        ContextDice.Value(DiceType.Zero, 0, ContextValues.Rank()))),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Ranged)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.IntuitiveShotFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.IntuitiveShotFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.IntuitiveShotFeat);
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
