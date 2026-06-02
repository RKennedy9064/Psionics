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

public static class AdvancedDervishPath
{
    public static void Configure()
    {
        FeatureConfigurator.New("AdvancedDervishPathFeat", Guids.AdvancedDervishPathFeat)
            .SetDisplayName(Loc.Str("PW.AdvancedDervishPath.Name", "Advanced Dervish Path"))
            .SetDescription(Loc.Str("PW.AdvancedDervishPath.Desc",
                "Your mastery of the Dervish path deepens. While in Dervish Trance and dual-wielding, the trance competence bonus also applies to your damage rolls (same conditions as the attack bonus)."))
            .SetIcon(FeatureRefs.TwoWeaponFighting.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.DervishTrance)
            .AddPrerequisiteFeature(FeatureRefs.DoubleSlice.ToString())
            .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteStatValue(StatType.Dexterity, 15)
            .AddPrerequisiteClassLevel(Guids.PsychicWarriorClass, 10)
            .AddComponent(new AdvancedDervishDamageBonus())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.AdvancedDervishPathFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.AdvancedDervishPathFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.AdvancedDervishPathFeat);
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
