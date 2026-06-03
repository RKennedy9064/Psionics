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
using Kingmaker.UnitLogic.Mechanics;
using Psionics.HarmonyPatches;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class IntuitiveFighting
{
    public static void Configure()
    {
        PsionicProficiencyPatch.RegisterPsionicFeat(Guids.IntuitiveFightingFeat);

        // Buff applied while focused — adds Wis mod as insight bonus to attack
        BuffConfigurator.New("IntuitiveFightingBuff", Guids.IntuitiveFightingBuff)
            .SetDisplayName(Loc.Str("PW.IntuitiveFighting.BuffName", "Intuitive Fighting"))
            .SetDescription(Loc.Str("PW.IntuitiveFighting.BuffDesc",
                "Your instincts guide your strikes. You gain your Wisdom modifier as an insight bonus to melee attack rolls."))
            .SetIcon(FeatureRefs.BlindFight.Reference.Get().Icon)
            .AddContextStatBonus(
                stat: StatType.AdditionalAttackBonus,
                descriptor: ModifierDescriptor.Insight,
                value: ContextValues.Rank())
            .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom))
            .Configure();

        FeatureConfigurator.New("IntuitiveFightingFeat", Guids.IntuitiveFightingFeat)
            .SetDisplayName(Loc.Str("PW.IntuitiveFighting.Name", "Intuitive Fighting"))
            .SetDescription(Loc.Str("PW.IntuitiveFighting.Desc",
                "While psionically focused, you use your Wisdom modifier instead of Strength on melee attack rolls, adding your Wisdom modifier as an insight bonus to attack rolls."))
            .SetIcon(FeatureRefs.BlindFight.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddPrerequisiteFeature(FeatureRefs.BlindFight.ToString())
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .AddFactContextActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New().ApplyBuffPermanent(Guids.IntuitiveFightingBuff)),
                deactivated: ActionsBuilder.New().RemoveBuff(Guids.IntuitiveFightingBuff))
            .Configure();

        // Inject into PsionicFocusBuff — apply buff while focused, remove when focus lost
        BuffConfigurator.For(Guids.PsionicFocusBuff)
            .AddBuffActions(
                activated: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.IntuitiveFightingFeat),
                    ifTrue: ActionsBuilder.New()
                        .Add(new ContextActionLogStat { Stat = StatType.Wisdom, Label = "[IntuitiveFighting] focus gained — Wisdom (mod=(score-10)/2)" })
                        .Add(new ContextActionLogStat { Stat = StatType.AdditionalAttackBonus, Label = "[IntuitiveFighting] before AdditionalAttackBonus" })
                        .ApplyBuffPermanent(Guids.IntuitiveFightingBuff)
                        .Add(new ContextActionLogStat { Stat = StatType.AdditionalAttackBonus, Label = "[IntuitiveFighting] after  AdditionalAttackBonus" })),
                deactivated: ActionsBuilder.New()
                    .Add(new ContextActionLogStat { Stat = StatType.AdditionalAttackBonus, Label = "[IntuitiveFighting] focus lost  — AdditionalAttackBonus (pre-remove)" })
                    .RemoveBuff(Guids.IntuitiveFightingBuff))
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.IntuitiveFightingFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.IntuitiveFightingFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.IntuitiveFightingFeat);
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
