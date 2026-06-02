using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Psionics.HarmonyPatches;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class AlignedAttack
{
    public static void Configure()
    {
        PsionicProficiencyPatch.RegisterPsionicFeat(Guids.AlignedAttackFeat);

        FeatureConfigurator.New("AlignedAttackFeat", Guids.AlignedAttackFeat)
            .SetDisplayName(Loc.Str("PW.AlignedAttack.Name", "Aligned Attack"))
            .SetDescription(Loc.Str("PW.AlignedAttack.Desc",
                "While psionically focused, your melee attacks deal an additional 2d6 damage, as your strikes are infused with aligned psionic energy that bypasses damage reduction based on alignment."))
            .SetIcon(AbilityRefs.BlessWeapon.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddComponent(new AlignedAttackDamage())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.AlignedAttackFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.AlignedAttackFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.AlignedAttackFeat);
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
