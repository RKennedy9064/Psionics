using System;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Shared helper for building a PhysicalDamage roll that inherits the triggering weapon's
/// material and enhancement bonus, used by PsionicCritical and AlignedAttack.
/// </summary>
public static class WeaponInheritedDamage
{
    public static BaseDamage Build(ItemEntityWeapon weapon, DiceFormula formula, bool alignmentBypassAll)
    {
        var physData = new DamageTypeDescription.PhysicalData();
        if (weapon != null)
        {
            physData.Form = weapon.Blueprint.DamageType.Physical.Form;
            physData.Material = weapon.Blueprint.DamageType.Physical.Material;
            physData.Enhancement = alignmentBypassAll
                ? Math.Max(1, weapon.EnchantmentValue)
                : weapon.EnchantmentValue;
        }
        else
        {
            physData.Form = PhysicalDamageForm.Bludgeoning | PhysicalDamageForm.Piercing | PhysicalDamageForm.Slashing;
            physData.Enhancement = alignmentBypassAll ? 1 : 0;
        }

        var dmgType = new DamageTypeDescription
        {
            Type = DamageType.Physical,
            Physical = physData
        };

        var raw = dmgType.CreateDamage(formula, 0);
        if (alignmentBypassAll && raw is PhysicalDamage phd)
            phd.AddAlignment(DamageAlignment.Good | DamageAlignment.Evil | DamageAlignment.Lawful | DamageAlignment.Chaotic);

        return raw;
    }
}
