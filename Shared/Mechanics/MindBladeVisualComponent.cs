using System;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.UnitLogic;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Carried by a mind blade weapon-entry feature. Records which real weapon the player chose and
/// which custom mind blade weapon type it maps to. The chosen weapon's full type (reach, fighter
/// group, handedness, name, visual, etc.) is copied onto the target form type, with the form's
/// normalized damage dice and critical range (1d6/1d8/2d6, 19-20/×2) preserved. The weapon's
/// category is deliberately NOT copied so the soulknife stays proficient with her mind blade.
/// Applied on fact activation (game load) and, for correct ordering, by <see cref="MindBladeComponent"/>
/// immediately before the blade is equipped.
/// </summary>
[Serializable]
public class MindBladeVisualComponent : UnitFactComponentDelegate
{
    public BlueprintItemWeaponReference SourceWeaponRef;
    public string TargetWeaponTypeGuid;

    public override void OnActivate() => Apply();

    public override void OnDeactivate() { }

    /// <summary>Copies the chosen weapon's type onto the target mind blade form type.</summary>
    public void Apply()
    {
        var sourceType = SourceWeaponRef?.Get()?.m_Type?.Get();
        if (sourceType == null) return;

        var target = BlueprintTool.Get<BlueprintWeaponType>(TargetWeaponTypeGuid);
        if (target == null) return;

        // Preserve the form's normalized damage + critical range.
        var formDamage  = target.m_BaseDamage;
        var formEdge    = target.m_CriticalRollEdge;
        var formCritMod = target.m_CriticalModifier;

        // Inherit the chosen weapon's mechanical + display properties, including its category so
        // category-dependent traits (finesse eligibility, Weapon Focus, fighter weapon groups)
        // match the real weapon. Proficiency for this category is granted by the weapon-entry
        // feature the player selected at 1st level.
        target.Category                  = sourceType.Category;
        target.m_DamageType              = sourceType.m_DamageType;
        target.m_AttackType              = sourceType.m_AttackType;
        target.m_AttackRange             = sourceType.m_AttackRange;   // reach
        target.m_FighterGroupFlags       = sourceType.m_FighterGroupFlags;
        target.m_IsTwoHanded             = sourceType.m_IsTwoHanded;
        target.m_IsOneHanded             = sourceType.m_IsOneHanded;
        target.m_IsLight                 = sourceType.m_IsLight;
        target.m_IsMonk                  = sourceType.m_IsMonk;
        target.m_IsNatural               = sourceType.m_IsNatural;
        target.m_IsUnarmed               = sourceType.m_IsUnarmed;
        target.m_OverrideAttackBonusStat = sourceType.m_OverrideAttackBonusStat;
        target.m_AttackBonusStatOverride = sourceType.m_AttackBonusStatOverride;
        target.m_Weight                  = sourceType.m_Weight;
        target.m_VisualParameters        = sourceType.m_VisualParameters;
        target.m_Icon                    = sourceType.m_Icon;
        target.m_TypeNameText            = sourceType.m_TypeNameText;
        target.m_DefaultNameText         = sourceType.m_DefaultNameText;
        target.m_DescriptionText         = sourceType.m_DescriptionText;

        // Restore the normalized form stats.
        target.m_BaseDamage       = formDamage;
        target.m_CriticalRollEdge = formEdge;
        target.m_CriticalModifier = formCritMod;
    }
}
