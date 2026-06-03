using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Localization;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features;

public static class PsychicWarriorBonusFeat
{
    public static void Configure()
    {
        var icon = FeatureRefs.PowerAttackFeature.Reference.Get().Icon;

        FeatureSelectionConfigurator.New("PsychicWarriorBonusFeat", Guids.BonusFeatSelection)
            .CopyFrom(FeatureSelectionRefs.FighterFeatSelection.ToString())
            .SetDisplayName(Loc.Str("PW.BonusFeat.Name", "Psychic Warrior Bonus Feat"))
            .SetDescription(Loc.Str("PW.BonusFeat.Desc", "A psychic warrior gains a bonus combat feat at 1st level, 2nd level, and every three levels thereafter."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .OnConfigure(bp => bp.m_Features = [])
            .Configure();
    }

    // Called after all psionic feats are configured so:
    // 1. m_Features is set to the psionic-feat subset (used by some filter paths)
    // 2. FeatureTag.ClassSpecific is added to each feat (used by the CharGen "class specific" toggle)
    public static void PopulateClassSpecificFeats()
    {
        string[] psionicFeats =
        [
            Guids.PsionicMeditationFeat,
            Guids.PsionicWeaponFeat,
            Guids.PsionicFistFeat,
            Guids.PsionicShotFeat,
            Guids.IntuitiveShotFeat,
            Guids.PsionicBodyFeat,
            Guids.SpeedOfThoughtFeat,
            Guids.PsionicDodgeFeat,
            Guids.CriticalRefocusFeat,
            Guids.GreaterPsionicWeaponFeat,
            Guids.GreaterPsionicFistFeat,
            Guids.GreaterPsionicShotFeat,
            Guids.RapidMetabolismFeat,
            Guids.CombatManifestationFeat,
            Guids.DeepImpactFeat,
            Guids.UpTheWallsFeat,
            Guids.PsionicEndowmentFeat,
            Guids.GreaterPsionicEndowmentFeat,
            Guids.PsionicCriticalFeat,
            Guids.RecklessOffenseFeat,
            Guids.AlignedAttackFeat,
            Guids.WoundingAttackFeat,
            Guids.FellShotFeat,
            Guids.UnavoidableStrikeFeat,
            Guids.IntuitiveFightingFeat,
            Guids.AdvancedWeaponmasterPathFeat,
            Guids.AdvancedAsceticPathFeat,
            Guids.AdvancedArcherPathFeat,
            Guids.AdvancedAssassinPathFeat,
            Guids.AdvancedBrawlerPathFeat,
            Guids.AdvancedDervishPathFeat,
            Guids.AdvancedFeralWarriorPathFeat,
            Guids.AdvancedMindKnightPathFeat,
        ];

        FeatureSelectionConfigurator.For(Guids.BonusFeatSelection)
            .OnConfigure(bp =>
                bp.m_Features = [.. psionicFeats.Select(g => BlueprintTool.GetRef<BlueprintFeatureReference>(g))])
            .Configure();

        // The CharGen "class specific" toggle filters by FeatureTag.ClassSpecific on each feat,
        // not by m_Features. Tag every psionic feat so they appear when the filter is on.
        foreach (var guid in psionicFeats)
        {
            FeatureConfigurator.For(guid)
                .AddFeatureTagsComponent(FeatureTag.ClassSpecific)
                .Configure();
        }
    }
}
