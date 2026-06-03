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

public static class AdvancedFeralWarriorPath
{
    public static void Configure()
    {
        FeatureConfigurator.New("AdvancedFeralWarriorPathFeat", Guids.AdvancedFeralWarriorPathFeat)
            .SetDisplayName(Loc.Str("PW.AdvancedFeralWarriorPath.Name", "Advanced Feral Path"))
            .SetDescription(Loc.Str("PW.AdvancedFeralWarriorPath.Desc",
                "Your mastery of the Feral path deepens. While in Feral Trance, the trance competence bonus also applies to damage rolls with natural attacks and unarmed strikes."))
            .SetIcon(AbilityRefs.Rage.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.FeralWarriorTrance)
            .AddPrerequisiteFeature(Guids.PsionicFistFeat)
            .AddPrerequisiteFeature(Guids.UnavoidableStrikeFeat)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteClassLevel(Guids.PsychicWarriorClass, 10)
            .AddComponent(new AdvancedFeralWarriorDamageBonus())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.AdvancedFeralWarriorPathFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.AdvancedFeralWarriorPathFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.AdvancedFeralWarriorPathFeat);
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
