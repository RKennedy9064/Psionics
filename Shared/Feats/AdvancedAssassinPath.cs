using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class AdvancedAssassinPath
{
    public static void Configure()
    {
        BuffConfigurator.New("AdvancedAssassinStaggerBuff", Guids.AdvancedAssassinStaggerBuff)
            .SetDisplayName(Loc.Str("PW.AdvancedAssassin.StaggerBuffName", "Psychic Stagger"))
            .SetDescription(Loc.Str("PW.AdvancedAssassin.StaggerBuffDesc",
                "You are staggered by a focused psionic strike."))
            .SetIcon(AbilityRefs.TrueStrike.Reference.Get().Icon)
            .AddCondition(UnitCondition.Staggered)
            .Configure();

        FeatureConfigurator.New("AdvancedAssassinPathFeat", Guids.AdvancedAssassinPathFeat)
            .SetDisplayName(Loc.Str("PW.AdvancedAssassinPath.Name", "Advanced Assassin Path"))
            .SetDescription(Loc.Str("PW.AdvancedAssassinPath.Desc",
                "Your mastery of the Assassin's path deepens. While in Assassin's Trance, attacks against flat-footed or flanked targets deal bonus precision damage as a rogue of half your psychic warrior level (1d6 at 2nd, +1d6 per 4 levels). Additionally, when your Assassin's Maneuver buff is active, successful hits force the target to make a Fortitude save (DC 10 + BAB) or be staggered for a number of rounds equal to your Wisdom modifier."))
            .SetIcon(AbilityRefs.VampiricTouch.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.AssassinsTrance)
            .AddPrerequisiteFeature(Guids.DeepImpactFeat)
            .AddPrerequisiteFeature(Guids.PsionicWeaponFeat)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteClassLevel(Guids.PsychicWarriorClass, 10)
            .AddComponent(new AssassinSneakAttack())
            .AddComponent(new AssassinManeuverStagger())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.AdvancedAssassinPathFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.AdvancedAssassinPathFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.AdvancedAssassinPathFeat);
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
