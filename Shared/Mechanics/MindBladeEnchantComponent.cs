using System;
using System.Collections.Generic;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Enhanced Mind Blade. Carried by the buff behind each enhancement/ability toggle. Toggling one on
/// or off recomputes the full set of enchantments applied to the equipped mind blade, enforcing the
/// level-scaled point pool. The "enhancement" toggle (IsEnhancement) eats all remaining points up to
/// the max direct bonus; ability toggles each cost <see cref="Cost"/> points.
/// </summary>
[Serializable]
public class MindBladeEnchantComponent : UnitFactComponentDelegate
{
    public int Cost;
    public BlueprintWeaponEnchantmentReference Enchantment; // ability toggles
    public bool IsEnhancement;                              // the eat-all direct-bonus toggle

    // Budget + the on-icon point count are handled by the game's ActivatableAbilityResourceLogic against
    // the pool resource (see EnhancedMindBlade + MindBladeEnchantPoolPatch), so this component only
    // (re)applies the current selection of enchantments to the equipped mind blade.
    public override void OnActivate() => MindBladeEnchantments.Recompute(Owner);

    public override void OnDeactivate() => MindBladeEnchantments.Recompute(Owner);
}

/// <summary>
/// Static engine for the Enhanced Mind Blade pool: progression math + applying the current toggle
/// selection to the equipped mind blade item.
/// </summary>
public static class MindBladeEnchantments
{
    private static BlueprintCharacterClass _skClass;
    private static BlueprintFeature _improvedEnhancement;
    private static BlueprintAbilityResource _poolResource;

    // All enchantments this system manages (Enhancement1-5 + every ability); registered by
    // EnhancedMindBlade.Configure so Recompute can clear them before re-applying.
    public static readonly List<BlueprintWeaponEnchantmentReference> ManagedEnchantments = [];

    // Ability-toggle blueprint GUID (dashed) -> pool cost (+N). Populated by EnhancedMindBlade.Configure;
    // read by MindBladeEnchantPoolPatch to charge/darken the correct amount per toggle.
    public static readonly Dictionary<string, int> AbilityToggleCosts =
        new(StringComparer.OrdinalIgnoreCase);

    private static int SkLevel(UnitEntityData owner)
    {
        _skClass ??= BlueprintTool.Get<BlueprintCharacterClass>(Guids.SoulKnifeClass);
        return owner.Progression.GetClassLevel(_skClass);
    }

    public static int GetPool(UnitEntityData owner)
    {
        int lvl = SkLevel(owner);
        int pool = lvl < 3 ? 0 : Math.Min(9, (lvl - 1) / 2);
        if (pool > 0 && HasImprovedEnhancement(owner)) pool += 1;
        return pool;
    }

    public static int GetMaxDirect(UnitEntityData owner)
    {
        int lvl = SkLevel(owner);
        int max =
            lvl >= 15 ? 5 :
            lvl >= 13 ? 4 :
            lvl >= 9  ? 3 :
            lvl >= 7  ? 2 :
            lvl >= 3  ? 1 : 0;
        if (max > 0 && HasImprovedEnhancement(owner)) max = Math.Min(5, max + 1);
        return max;
    }

    private static bool HasImprovedEnhancement(UnitEntityData owner)
    {
        _improvedEnhancement ??= BlueprintTool.Get<BlueprintFeature>(Guids.BladeSkillImprovedEnhancement);
        return _improvedEnhancement != null && owner.Descriptor.HasFact(_improvedEnhancement);
    }

    public static int ActiveAbilityCost(UnitEntityData owner)
    {
        int cost = 0;
        foreach (var buff in owner.Descriptor.Buffs.Enumerable)
        {
            var c = buff.Blueprint.GetComponent<MindBladeEnchantComponent>();
            if (c != null && !c.IsEnhancement) cost += c.Cost;
        }
        return cost;
    }

    /// <summary>Reapplies the current toggle selection to the equipped mind blade.</summary>
    public static void Recompute(UnitEntityData owner)
    {
        var item = FindMindBlade(owner);
        if (item == null) return;

        foreach (var managed in ManagedEnchantments)
        {
            var bp = managed?.Get();
            if (bp == null) continue;
            var existing = item.GetEnchantment(bp);
            if (existing != null) item.RemoveEnchantment(existing);
        }

        bool enhanceToggle = false;
        int abilityCost = 0;
        var abilityEnchants = new List<BlueprintWeaponEnchantment>();
        foreach (var buff in owner.Descriptor.Buffs.Enumerable)
        {
            var c = buff.Blueprint.GetComponent<MindBladeEnchantComponent>();
            if (c == null) continue;
            if (c.IsEnhancement) { enhanceToggle = true; continue; }
            var ench = c.Enchantment?.Get();
            if (ench != null) { abilityEnchants.Add(ench); abilityCost += c.Cost; }
        }

        foreach (var ench in abilityEnchants)
            item.AddEnchantment(ench, null);

        // The enhance "eat-all" toggle reserves (and grants) up to the level's max direct bonus from
        // whatever the ability toggles leave; it does NOT yield those points back to abilities while on.
        int pool = GetPool(owner);
        int enhanceReserved = enhanceToggle ? Math.Max(0, Math.Min(GetMaxDirect(owner), pool - abilityCost)) : 0;
        if (enhanceReserved > 0)
        {
            var ench = EnhancementEnchant(enhanceReserved);
            if (ench != null) item.AddEnchantment(ench, null);
        }

        // Single source of truth for the on-icon pool count: points left after ability + enhance
        // reservations. Setting it here (on every toggle on/off, equip, and level-up) gives correct
        // live display, darkening, and — crucially — refunds when a toggle is switched off.
        SyncPoolDisplay(owner, Math.Max(0, pool - abilityCost - enhanceReserved));
    }

    private static void SyncPoolDisplay(UnitEntityData owner, int remaining)
    {
        _poolResource ??= BlueprintTool.Get<BlueprintAbilityResource>(Guids.EnhancedMindBladePoolResource);
        if (_poolResource == null) return;

        var res = owner.Resources;
        int now = res.GetResourceAmount(_poolResource);
        if (now < remaining) res.Restore(_poolResource, remaining - now);
        else if (now > remaining) res.Spend(_poolResource, now - remaining);
    }

    private static BlueprintWeaponEnchantment EnhancementEnchant(int level) => level switch
    {
        1 => WeaponEnchantmentRefs.Enhancement1.Reference.Get(),
        2 => WeaponEnchantmentRefs.Enhancement2.Reference.Get(),
        3 => WeaponEnchantmentRefs.Enhancement3.Reference.Get(),
        4 => WeaponEnchantmentRefs.Enhancement4.Reference.Get(),
        5 => WeaponEnchantmentRefs.Enhancement5.Reference.Get(),
        _ => null,
    };

    private static readonly string[] MindBladeWeaponGuids =
    [
        Guids.MindBladeLightWeapon, Guids.MindBladeOneHandedWeapon,
        Guids.MindBladeTwoHandedWeapon, Guids.MindBladeDoublePrimaryWeapon,
    ];

    private static ItemEntityWeapon FindMindBlade(UnitEntityData owner)
    {
        var slot = owner.Descriptor.Body?.PrimaryHand;
        if (slot == null || !slot.HasItem) return null;
        if (slot.Item is not ItemEntityWeapon weapon || weapon.Blueprint == null) return null;
        foreach (var guid in MindBladeWeaponGuids)
            if (weapon.Blueprint == BlueprintTool.Get<BlueprintItemWeapon>(guid)) return weapon;
        return null;
    }
}
