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
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Psionics.HarmonyPatches;
using Psionics.Utils;

namespace Psionics.Shared.Feats;

public static class GreaterPsionicWeapon
{
    public static void Configure()
    {
        PsionicProficiencyPatch.RegisterPsionicFeat(Guids.GreaterPsionicWeaponFeat);

        BuffConfigurator.New("GreaterPsionicWeaponBuff", Guids.GreaterPsionicWeaponBuff)
            .SetDisplayName(Loc.Str("PW.GreaterPsionicWeapon.Name", "Greater Psionic Weapon"))
            .SetFlags(BlueprintBuff.Flags.HiddenInUi)
            .Configure();

        FeatureConfigurator.New("GreaterPsionicWeaponFeat", Guids.GreaterPsionicWeaponFeat)
            .SetDisplayName(Loc.Str("PW.GreaterPsionicWeapon.Name", "Greater Psionic Weapon"))
            .SetDescription(Loc.Str("PW.GreaterPsionicWeapon.Desc",
                "While psionically focused, your melee attacks deal an additional 1d6 force damage at ML 1, 2d6 at ML 6, 3d6 at ML 11, 4d6 at ML 16. Stacks with Psionic Weapon for a total up to 8d6 force at ML 16."))
            .SetIcon(FeatureRefs.VitalStrikeFeatureImproved.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.PsionicWeaponFeat)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().Conditional(
                    ConditionsBuilder.New().CasterHasFact(Guids.PsionicFocusBuff),
                    ifTrue: ActionsBuilder.New()
                        .ChangeSharedValueTo(ContextValues.Constant(1), AbilitySharedValue.Damage)
                        .Conditional(
                            ConditionsBuilder.New().CharacterClass(checkCaster: true, clazz: Guids.PsychicWarriorClass, minLevel: 6),
                            ifTrue: ActionsBuilder.New().ChangeSharedValueAddTo(ContextValues.Constant(1), AbilitySharedValue.Damage))
                        .Conditional(
                            ConditionsBuilder.New().CharacterClass(checkCaster: true, clazz: Guids.PsychicWarriorClass, minLevel: 11),
                            ifTrue: ActionsBuilder.New().ChangeSharedValueAddTo(ContextValues.Constant(1), AbilitySharedValue.Damage))
                        .Conditional(
                            ConditionsBuilder.New().CharacterClass(checkCaster: true, clazz: Guids.PsychicWarriorClass, minLevel: 16),
                            ifTrue: ActionsBuilder.New().ChangeSharedValueAddTo(ContextValues.Constant(1), AbilitySharedValue.Damage))
                        .DealDamage(DamageTypes.Energy(DamageEnergyType.Magic),
                            ContextDice.Value(DiceType.D6, ContextValues.Shared(AbilitySharedValue.Damage), ContextValues.Constant(0)))),
                onlyHit: true, checkWeaponRangeType: true, rangeType: WeaponRangeType.Melee)
            .AddRecommendedClass(Guids.PsychicWarriorClass)
            .Configure();

        SafeAddFeatToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), Guids.GreaterPsionicWeaponFeat);
        SafeAddFeatToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), Guids.GreaterPsionicWeaponFeat);
        SafeAddFeatToSelection(Guids.BonusFeatSelection, Guids.GreaterPsionicWeaponFeat);
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
