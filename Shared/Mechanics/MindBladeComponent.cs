using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

[Serializable]
public class MindBladeComponent : UnitFactComponentDelegate, IUnitLevelUpHandler
{
    public WeaponCategory Category;
    public BlueprintItemWeaponReference WeaponRef;

    private static readonly Dictionary<WeaponCategory, BlueprintItemWeapon> s_Cache = [];
    private static readonly LogWrapper Log = LogWrapper.Get("Psionics");

    private BlueprintItemWeapon FindWeapon()
    {
        if (s_Cache.TryGetValue(Category, out var cached))
            return cached;

        var weapon = WeaponRef?.Get();
        if (weapon != null)
            s_Cache[Category] = weapon;
        else
            Log.Warn($"[MB] FindWeapon({Category}): WeaponRef is null or Get() returned null");
        return weapon;
    }

    public override void OnActivate()
    {
        foreach (var buff in Owner.Descriptor.Buffs.Enumerable.ToList())
        {
            if (buff != Fact && buff.Blueprint.ComponentsArray.OfType<MindBladeComponent>().Any())
            {
                buff.Remove();
                break;
            }
        }

        var weapon = FindWeapon();
        if (weapon == null) return;

        var slot = Owner.Descriptor.Body.PrimaryHand;
        var secondary = Owner.Descriptor.Body.SecondaryHand;
        Log.Info($"[MB] OnActivate ({weapon.name}): double={weapon.Double} " +
                 $"primaryHasItem={slot.HasItem} primaryItem={(slot.HasItem ? slot.Item?.Blueprint?.name : "empty")} " +
                 $"secHasItem={secondary?.HasItem} secLocked={secondary?.Lock.Value}");

        if (slot.HasItem && slot.Item?.Blueprint == weapon)
        {
            Log.Info($"[MB] OnActivate ({weapon.name}): early-return, already equipped");
            return;
        }

        // Apply the player's chosen appearance to this form's weapon type *before* equipping,
        // so the new item picks up the right model. (The visual feature also applies on load,
        // but doing it here guarantees correct ordering when the blade is first manifested.)
        ApplyChosenVisual(weapon);

        // Blade skill: Mind Blade Finesse — force the mind blade to use Dexterity for attack rolls
        // (works for all forms, including two-handed). ApplyChosenVisual resets these fields from the
        // source weapon each equip, so we (re)apply the override here based on the wielder.
        ApplyMindBladeFinesse(weapon);

        var item = new ItemEntityWeapon(weapon) { IsIdentified = true };
        Log.Info($"[MB] pre-equip ({weapon.name}): canInsertPrimary={slot.CanInsertItem(item)} primaryLocked={slot.Lock.Value} " +
                 $"| possible={slot.IsPossibleInsertItems()} supported={slot.IsItemSupported(item)} " +
                 $"canEquip={item.CanBeEquippedBy(Owner.Descriptor)} canTakeOneHand={item.CanTakeOneHand(Owner)}");

        Owner.Inventory.Add(item);
        slot.InsertItem(item);
        slot.Lock.Retain();

        Log.Info($"[MB] post-insert ({weapon.name}): primaryHasItem={slot.HasItem} primaryItem={(slot.HasItem ? slot.Item?.Blueprint?.name : "empty")} " +
                 $"itemWielder={(item.Wielder != null)} secHasItem={secondary?.HasItem} secItem={(secondary != null && secondary.HasItem ? secondary.Item?.Blueprint?.name : "empty")}");

        var visualEnch = BlueprintTool.Get<BlueprintWeaponEnchantment>(Guids.CallWeaponryVisualEnchantment);
        if (visualEnch != null)
            item.AddEnchantment(visualEnch, null);

        // Enhanced Mind Blade: apply the player's current enhancement-pool allocation to the new item.
        MindBladeEnchantments.Recompute(Owner);

        EventBus.Subscribe(this);
    }

    public override void OnDeactivate()
    {
        EventBus.Unsubscribe(this);

        var weapon = FindWeapon();
        if (weapon == null) return;

        var slot = Owner.Descriptor.Body.PrimaryHand;
        if (slot.HasItem && slot.Item?.Blueprint == weapon)
        {
            slot.Lock.Release();
            var item = slot.Item;
            slot.RemoveItem();
            item?.Collection?.Remove(item);
            Log.Info($"[MB] Mind blade dismissed for {Owner.CharacterName}");
        }
    }

    // Finds the visual feature the player selected for this form and applies the chosen weapon's
    // appearance (3D model on the type) and inventory icon (on the item blueprint) before the
    // item is created.
    private void ApplyChosenVisual(BlueprintItemWeapon weapon)
    {
        var weaponType = weapon.m_Type?.Get();
        if (weaponType == null)
        {
            Log.Warn($"[MB] ApplyChosenVisual: weapon {weapon.name} has no resolvable type");
            return;
        }

        int scanned = 0;
        foreach (var feature in Owner.Progression.Features.Enumerable)
        {
            var vc = feature.Blueprint.GetComponent<MindBladeVisualComponent>();
            if (vc == null) continue;
            scanned++;

            // Compare resolved blueprints by identity — string GUID formats differ (dashes vs none).
            var vcTargetType = BlueprintTool.Get<BlueprintWeaponType>(vc.TargetWeaponTypeGuid);
            if (vcTargetType != weaponType) continue;

            var sourceWeapon = vc.SourceWeaponRef?.Get();
            vc.Apply(); // copies the chosen weapon's type onto the form type
            if (sourceWeapon != null)
            {
                // Item-level fields drive the equipped 3D model, name, and inventory icon.
                weapon.m_VisualParameters = sourceWeapon.m_VisualParameters;
                weapon.m_DisplayNameText  = sourceWeapon.m_DisplayNameText;
                if (sourceWeapon.Icon != null) weapon.m_Icon = sourceWeapon.Icon;
            }
            var reach = weaponType.m_AttackRange;
            Log.Info($"[MB] ApplyChosenVisual: matched '{feature.Blueprint.name}', source={sourceWeapon?.name ?? "null"}, " +
                     $"attackRange={reach} name={sourceWeapon?.m_DisplayNameText}");
            return;
        }

        Log.Warn($"[MB] ApplyChosenVisual: no MindBladeVisualComponent matched type {weaponType.name} " +
                 $"(scanned {scanned} visual features on {Owner.CharacterName})");
    }

    private static BlueprintFeature _mindBladeFinesse;

    // Blade skill: Mind Blade Finesse. Sets the equipped form's weapon type to use Dexterity for
    // attack rolls when the wielder has the skill (otherwise leaves the source weapon's setting).
    private void ApplyMindBladeFinesse(BlueprintItemWeapon weapon)
    {
        var weaponType = weapon.m_Type?.Get();
        if (weaponType == null) return;

        _mindBladeFinesse ??= BlueprintTool.Get<BlueprintFeature>(Guids.BladeSkillMindBladeFinesse);
        bool hasFinesse = _mindBladeFinesse != null && Owner.Descriptor.HasFact(_mindBladeFinesse);

        weaponType.m_OverrideAttackBonusStat = hasFinesse;
        if (hasFinesse)
            weaponType.m_AttackBonusStatOverride = Kingmaker.EntitySystem.Stats.StatType.Dexterity;
    }

    public void HandleUnitBeforeLevelUp(UnitEntityData unit) { }

    public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller)
    {
        // During char-gen/level-up preview this fires on component instances whose runtime is
        // not attached; accessing Owner then throws "ComponentRuntime is unavailable". Guard it.
        UnitEntityData owner;
        try { owner = Owner; }
        catch { return; }

        if (unit != owner) return;

        // Enhanced Mind Blade pool may have grown — reapply the current allocation.
        MindBladeEnchantments.Recompute(owner);
    }
}
