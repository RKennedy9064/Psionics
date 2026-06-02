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

public static class AdvancedWeaponmasterPath
{
    public static void Configure()
    {
        FeatureConfigurator.New("AdvancedWeaponmasterPathFeat", Guids.AdvancedWeaponmasterPathFeat)
            .SetDisplayName(Loc.Str("PW.AdvancedWeaponmasterPath.Name", "Advanced Weaponmaster Path"))
            .SetDescription(Loc.Str("PW.AdvancedWeaponmasterPath.Desc",
                "Your mastery of the Weaponmaster path deepens. The competence bonus granted by your Weaponmaster Trance now also applies to weapon damage rolls (excluding natural attacks)."))
            .SetIcon(AbilityRefs.TrueStrike.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.WeaponmasterTrance)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteClassLevel(Guids.PsychicWarriorClass, 10)
            .AddComponent(new AdvancedWeaponmasterDamageBonus())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.AdvancedWeaponmasterPathFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.AdvancedWeaponmasterPathFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.AdvancedWeaponmasterPathFeat);
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
