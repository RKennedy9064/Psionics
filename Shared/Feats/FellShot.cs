using System.Linq;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Psionics.HarmonyPatches;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class FellShot
{
    public static void Configure()
    {
        PsionicProficiencyPatch.RegisterPsionicFeat(Guids.FellShotFeat);

        BuffConfigurator.New("FellShotDebuff", Guids.FellShotDebuff)
            .SetDisplayName(Loc.Str("PW.FellShot.DebuffName", "Fell Shot"))
            .SetDescription(Loc.Str("PW.FellShot.DebuffDesc",
                "This target has been struck with psionic force and loses their Dexterity bonus to AC."))
            .SetIcon(FeatureRefs.PreciseShot.Reference.Get().Icon)
            .AddCondition(UnitCondition.LoseDexterityToAC)
            .Configure();

        FeatureConfigurator.New("FellShotFeat", Guids.FellShotFeat)
            .SetDisplayName(Loc.Str("PW.FellShot.Name", "Fell Shot"))
            .SetDescription(Loc.Str("PW.FellShot.Desc",
                "While psionically focused, your ranged attacks treat the target as flat-footed for 1 round."))
            .SetIcon(FeatureRefs.PreciseShot.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddPrerequisiteFeature(Guids.PsionicShotFeat)
            .AddPrerequisiteStatValue(StatType.Dexterity, 13)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteFeature(FeatureRefs.PointBlankShot.ToString())
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New()
                        .Add(new ContextActionLog { Message = "[FellShot] ranged hit while focused — applying flat-footed 1r" })
                        .ApplyBuff(Guids.FellShotDebuff, ContextDuration.Fixed(1))),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Ranged)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.FellShotFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.FellShotFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.FellShotFeat);
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
