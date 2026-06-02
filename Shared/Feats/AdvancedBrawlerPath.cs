using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class AdvancedBrawlerPath
{
    public static void Configure()
    {
        FeatureConfigurator.New("AdvancedBrawlerPathFeat", Guids.AdvancedBrawlerPathFeat)
            .SetDisplayName(Loc.Str("PW.AdvancedBrawlerPath.Name", "Advanced Brawler Path"))
            .SetDescription(Loc.Str("PW.AdvancedBrawlerPath.Desc",
                "Your mastery of the Brawler path deepens. While in Brawler Trance, you gain a competence bonus to grapple checks equal to +1 per 3 psychic warrior levels. Additionally, when using the Brawler Maneuver you may deal lethal damage instead of non-lethal (this is already the default in this implementation)."))
            .SetIcon(FeatureRefs.ImprovedUnarmedStrike.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.BrawlerTrance)
            .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteClassLevel(Guids.PsychicWarriorClass, 10)
            .AddComponent(new AdvancedBrawlerCMBBonus())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.AdvancedBrawlerPathFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.AdvancedBrawlerPathFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.AdvancedBrawlerPathFeat);
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
