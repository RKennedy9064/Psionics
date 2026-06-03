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

public static class UnavoidableStrike
{
    public static void Configure()
    {
        PsionicProficiencyPatch.RegisterPsionicFeat(Guids.UnavoidableStrikeFeat);

        BuffConfigurator.New("UnavoidableStrikeDebuff", Guids.UnavoidableStrikeDebuff)
            .SetDisplayName(Loc.Str("PW.UnavoidableStrike.DebuffName", "Unavoidable Strike"))
            .SetDescription(Loc.Str("PW.UnavoidableStrike.DebuffDesc",
                "This target has been struck with psionic force and loses their Dexterity bonus to AC."))
            .SetIcon(FeatureRefs.WeaponFocusUnarmed.Reference.Get().Icon)
            .AddCondition(UnitCondition.LoseDexterityToAC)
            .Configure();

        FeatureConfigurator.New("UnavoidableStrikeFeat", Guids.UnavoidableStrikeFeat)
            .SetDisplayName(Loc.Str("PW.UnavoidableStrike.Name", "Unavoidable Strike"))
            .SetDescription(Loc.Str("PW.UnavoidableStrike.Desc",
                "While psionically focused, your unarmed and natural weapon attacks treat the target as flat-footed for 1 round."))
            .SetIcon(FeatureRefs.WeaponFocusUnarmed.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddPrerequisiteFeature(Guids.PsionicFistFeat)
            .AddPrerequisiteStatValue(StatType.Strength, 13)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New()
                        .Add(new ContextActionLog { Message = "[UnavoidableStrike] melee hit while focused — applying flat-footed 1r" })
                        .ApplyBuff(Guids.UnavoidableStrikeDebuff, ContextDuration.Fixed(1))),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Melee)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.UnavoidableStrikeFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.UnavoidableStrikeFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.UnavoidableStrikeFeat);
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
