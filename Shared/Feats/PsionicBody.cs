using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class PsionicBody
{
    // Every psionic feat GUID must be listed here so Psionic Body's count is accurate.
    // Add new feats to this list when they are implemented.
    internal static readonly string[] AllPsionicFeatGuids =
    [
        Guids.PsionicMeditationFeat,
        Guids.PsionicWeaponFeat,
        Guids.PsionicBodyFeat,
        Guids.SpeedOfThoughtFeat,
        Guids.PsionicDodgeFeat,
        Guids.CriticalRefocusFeat,
        Guids.GreaterPsionicWeaponFeat,
        Guids.PsionicFistFeat,
        Guids.GreaterPsionicFistFeat,
        Guids.PsionicShotFeat,
        Guids.GreaterPsionicShotFeat,
        // Tier 2 — add GUIDs here as feats are implemented
        Guids.RapidMetabolismFeat,
        Guids.OverchannelFeat,
        Guids.CombatManifestationFeat,
        Guids.DeepImpactFeat,
        Guids.ExpandedKnowledgeFeat,
        Guids.UpTheWallsFeat,
        Guids.PsionicEndowmentFeat,
        // Tier 1 new feats
        Guids.PsionicCriticalFeat,
        Guids.WoundingAttackFeat,
        Guids.FellShotFeat,
        Guids.UnavoidableStrikeFeat,
        Guids.IntuitiveFightingFeat,
        Guids.RecklessOffenseFeat,
        Guids.AlignedAttackFeat,
    ];

    public static void Configure()
    {
        FeatureConfigurator.New("PsionicBodyFeat", Guids.PsionicBodyFeat)
            .SetDisplayName(Loc.Str("PW.PsionicBody.Name", "Psionic Body"))
            .SetDescription(Loc.Str("PW.PsionicBody.Desc",
                "Your body and mind are in perfect harmony. You gain +2 hit points for each psionic feat you possess, including this one."))
            .SetIcon(FeatureRefs.Toughness.Reference.Get().Icon)
            .SetGroups(FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            // ContextValues.Rank() × 2 where rank = number of psionic feats owned
            .AddContextStatBonus(
                stat: StatType.HitPoints,
                value: ContextValues.Rank(),
                descriptor: ModifierDescriptor.UntypedStackable)
            .AddContextRankConfig(
                ContextRankConfigs.FeatureList(AllPsionicFeatGuids)
                    .WithBonusValueProgression(0, doubleBaseValue: true))
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.PsionicBodyFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.PsionicBodyFeat);
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
