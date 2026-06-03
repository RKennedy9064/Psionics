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

public static class DeepImpact
{
    public static void Configure()
    {
        PsionicProficiencyPatch.RegisterPsionicFeat(Guids.DeepImpactFeat);

        BuffConfigurator.New("DeepImpactDebuff", Guids.DeepImpactDebuff)
            .SetDisplayName(Loc.Str("PW.DeepImpact.DebuffName", "Deep Impact"))
            .SetDescription(Loc.Str("PW.DeepImpact.DebuffDesc",
                "This target has been struck with psionic force and loses their Dexterity bonus to AC."))
            .SetIcon(AbilityRefs.DisruptingWeapon.Reference.Get().Icon)
            .AddCondition(UnitCondition.LoseDexterityToAC)
            .Configure();

        FeatureConfigurator.New("DeepImpactFeat", Guids.DeepImpactFeat)
            .SetDisplayName(Loc.Str("PW.DeepImpact.Name", "Deep Impact"))
            .SetDescription(Loc.Str("PW.DeepImpact.Desc",
                "While psionically focused, your melee attacks treat the target as flat-footed for 1 round."))
            .SetIcon(AbilityRefs.DisruptingWeapon.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New()
                        .Add(new ContextActionLog { Message = "[DeepImpact] melee hit while focused — applying flat-footed 1r" })
                        .ApplyBuff(Guids.DeepImpactDebuff, ContextDuration.Fixed(1))),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Melee)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.DeepImpactFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.DeepImpactFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.DeepImpactFeat);
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
