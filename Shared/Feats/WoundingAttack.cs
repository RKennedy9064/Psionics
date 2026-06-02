using System.Linq;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.HarmonyPatches;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class WoundingAttack
{
    public static void Configure()
    {
        PsionicProficiencyPatch.RegisterPsionicFeat(Guids.WoundingAttackFeat);

        // Triggered buff — fires once on next melee hit, then removes itself
        var triggerBuff = BuffConfigurator.New("WoundingAttackBuff", Guids.WoundingAttackBuff)
            .SetDisplayName(Loc.Str("PW.WoundingAttack.BuffName", "Wounding Attack"))
            .SetDescription(Loc.Str("PW.WoundingAttack.BuffDesc",
                "Your next melee attack deals 1d4 additional damage from psychic wounding."))
            .SetIcon(AbilityRefs.InflictModerateWounds.Reference.Get().Icon)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[WoundingAttack] trigger: melee hit — dealing 1d4" })
                    .DealDamage(DamageTypes.Physical(), ContextDice.Value(DiceType.D4, 1, 0))
                    .RemoveBuff(Guids.WoundingAttackBuff),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Melee)
            .Configure();

        // Swift-action ability: expend focus, arm the trigger buff for 1 round
        AbilityConfigurator.New("WoundingAttackAbility", Guids.WoundingAttackAbility)
            .SetDisplayName(Loc.Str("PW.WoundingAttackAb.Name", "Wounding Attack"))
            .SetDescription(Loc.Str("PW.WoundingAttackAb.Desc",
                "Swift Action. Expend psionic focus. Your next melee attack that hits deals an additional 1d4 damage."))
            .SetIcon(AbilityRefs.InflictModerateWounds.Reference.Get().Icon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .RemoveBuff(Guids.PsionicFocusBuff)
                    .ApplyBuff(triggerBuff, ContextDuration.Fixed(1)))
            .Configure();

        FeatureConfigurator.New("WoundingAttackFeat", Guids.WoundingAttackFeat)
            .SetDisplayName(Loc.Str("PW.WoundingAttack.Name", "Wounding Attack"))
            .SetDescription(Loc.Str("PW.WoundingAttack.Desc",
                "As a swift action, expend psionic focus to charge your next melee attack with psychic energy. If the attack hits, it deals an additional 1d4 damage. Requires BAB +8."))
            .SetIcon(AbilityRefs.InflictModerateWounds.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.GainPsionicFocusFeature)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 8)
            .AddFacts([Guids.WoundingAttackAbility])
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.WoundingAttackFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.WoundingAttackFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.WoundingAttackFeat);
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
