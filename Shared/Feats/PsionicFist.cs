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
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class PsionicFist
{
    public static void Configure()
    {
        FeatureConfigurator.New("PsionicFistFeat", Guids.PsionicFistFeat)
            .SetDisplayName(Loc.Str("PW.PsionicFist.Name", "Psionic Fist"))
            .SetDescription(Loc.Str("PW.PsionicFist.Desc",
                "While psionically focused, your unarmed strikes deal additional damage scaling with manifester level (1d6 at ML 1, 2d6 at ML 2, 3d6 at ML 4)."))
            .SetIcon(FeatureRefs.ImprovedUnarmedStrike.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
            .AddContextRankConfig(ContextRankConfigs.CasterLevel().WithCustomProgression((1, 1), (3, 2), (20, 3)))
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New()
                        .Add(new ContextActionLog { Message = "[PsionicFist] unarmed hit while focused", LogRank = true })
                        .DealDamage(
                            DamageTypes.Physical(),
                            ContextDice.Value(DiceType.D6, ContextValues.Rank(), 0))),
                onlyHit: true,
                category: WeaponCategory.UnarmedStrike)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.PsionicFistFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.PsionicFistFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.PsionicFistFeat);
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
