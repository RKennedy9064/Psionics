using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.MindBlade;

/// <summary>
/// Mind-blade versions of the weapon-type combat feats (Greater Weapon Focus, Improved Critical, Weapon
/// Specialization, Greater Weapon Specialization). The mind blade uses custom weapon types, so the stock
/// feats can't target it; these apply the equivalent bonuses to all four mind-blade weapon types via the
/// game's WeaponType* fact components (the same approach the Deadly Blow blade skill uses).
///
/// All gate on Weapon Focus (Mind Blade) so they only surface for mind-blade users. Fighter-level
/// prerequisites from the tabletop feats are relaxed to base-attack-bonus thresholds so the mind-blade
/// classes (which aren't fighters) can actually take them.
/// </summary>
public static class MindBladeWeaponFeats
{
    private static readonly string[] WeaponTypes =
    [
        Guids.MindBladeLightWeaponType, Guids.MindBladeOneHandedWeaponType,
        Guids.MindBladeTwoHandedWeaponType, Guids.MindBladeDoubleWeaponType,
    ];

    public static void Configure()
    {
        // Greater Weapon Focus (Mind Blade): +1 attack (stacks with Weapon Focus for +2).
        var greaterFocus = FeatureConfigurator.New("SKGreaterWeaponFocusMindBlade", Guids.GreaterWeaponFocusMindBlade)
            .SetDisplayName(Loc.Str("SK.GreaterWeaponFocusMB.Name", "Greater Weapon Focus (Mind Blade)"))
            .SetDescription(Loc.Str("SK.GreaterWeaponFocusMB.Desc",
                "You gain an additional +1 bonus on attack rolls with your mind blade (+2 total with Weapon Focus)."))
            .SetIcon(FeatureRefs.WeaponFocusGreatsword.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.WeaponFocusMindBlade)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 4)
            .AddComponent<WeaponFocusMindBladeComponent>()
            .Configure();

        // Improved Critical (Mind Blade): doubles the mind blade's threat range (does not stack with Keen).
        var improvedCrit = FeatureConfigurator.New("SKImprovedCriticalMindBlade", Guids.ImprovedCriticalMindBlade)
            .SetDisplayName(Loc.Str("SK.ImprovedCriticalMB.Name", "Improved Critical (Mind Blade)"))
            .SetDescription(Loc.Str("SK.ImprovedCriticalMB.Desc",
                "Your mind blade's threat range is doubled (19-20 becomes 17-20). This does not stack with the Keen weapon property."))
            .SetIcon(AbilityRefs.Disintegrate.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.WeaponFocusMindBlade)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 8);
        foreach (var wt in WeaponTypes)
            improvedCrit = improvedCrit.AddComponent(new WeaponTypeCriticalEdgeIncrease
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(wt) });
        improvedCrit.Configure();

        // Weapon Specialization (Mind Blade): +2 damage.
        var spec = FeatureConfigurator.New("SKWeaponSpecializationMindBlade", Guids.WeaponSpecializationMindBlade)
            .SetDisplayName(Loc.Str("SK.WeaponSpecMB.Name", "Weapon Specialization (Mind Blade)"))
            .SetDescription(Loc.Str("SK.WeaponSpecMB.Desc",
                "You deal an additional +2 damage with your mind blade."))
            .SetIcon(FeatureRefs.WeaponSpecializationGreatsword.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.WeaponFocusMindBlade)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 4);
        foreach (var wt in WeaponTypes)
            spec = spec.AddComponent(new WeaponTypeDamageBonus
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(wt), DamageBonus = 2 });
        spec.Configure();

        // Greater Weapon Specialization (Mind Blade): +2 more damage (+4 total).
        var greaterSpec = FeatureConfigurator.New("SKGreaterWeaponSpecializationMindBlade", Guids.GreaterWeaponSpecializationMindBlade)
            .SetDisplayName(Loc.Str("SK.GreaterWeaponSpecMB.Name", "Greater Weapon Specialization (Mind Blade)"))
            .SetDescription(Loc.Str("SK.GreaterWeaponSpecMB.Desc",
                "You deal an additional +2 damage with your mind blade (+4 total with Weapon Specialization)."))
            .SetIcon(FeatureRefs.WeaponSpecializationGreatsword.Reference.Get().Icon)
            .SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat)
            .AddPrerequisiteFeature(Guids.WeaponSpecializationMindBlade)
            .AddPrerequisiteFeature(Guids.GreaterWeaponFocusMindBlade)
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 12);
        foreach (var wt in WeaponTypes)
            greaterSpec = greaterSpec.AddComponent(new WeaponTypeDamageBonus
                { m_WeaponType = BlueprintTool.GetRef<BlueprintWeaponTypeReference>(wt), DamageBonus = 2 });
        greaterSpec.Configure();

        foreach (var g in new[]
        {
            Guids.GreaterWeaponFocusMindBlade, Guids.ImprovedCriticalMindBlade,
            Guids.WeaponSpecializationMindBlade, Guids.GreaterWeaponSpecializationMindBlade,
        })
        {
            AddToSelection(FeatureSelectionRefs.BasicFeatSelection.ToString(), g);
            AddToSelection(FeatureSelectionRefs.FighterFeatSelection.ToString(), g);
            AddToSelection(Guids.BonusFeatSelection, g);
        }
    }

    private static void AddToSelection(string selectionGuid, string featGuid)
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
