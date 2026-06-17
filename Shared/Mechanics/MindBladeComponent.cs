using System;
using System.Linq;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

/// <summary>
/// Placed on each form toggle's buff. When the blade is manifested it resolves the owner's chosen
/// weapon (recorded on their <see cref="MindBladeFormComponent"/> feature) and equips that dedicated,
/// immutable per-category blueprint. Nothing global is mutated, so multiple soulknives never share
/// appearance, reach, or finesse — each gets exactly the weapon they selected.
/// </summary>
[Serializable]
public class MindBladeComponent : UnitFactComponentDelegate, IUnitLevelUpHandler
{
    private static readonly LogWrapper Log = LogWrapper.Get("Psionics");

    // The immutable item this owner's selection maps to (read off their chosen feature).
    private BlueprintItemWeapon ResolveMindBlade()
    {
        foreach (var feature in Owner.Progression.Features.Enumerable)
        {
            var fc = feature.Blueprint.GetComponent<MindBladeFormComponent>();
            var bp = fc?.ItemRef?.Get();
            if (bp != null) return bp;
        }
        return null;
    }

    public override void OnActivate()
    {
        // Dismiss any other active mind blade form so only one is ever manifested.
        foreach (var buff in Owner.Descriptor.Buffs.Enumerable.ToList())
        {
            if (buff != Fact && buff.Blueprint.ComponentsArray.OfType<MindBladeComponent>().Any())
            {
                buff.Remove();
                break;
            }
        }

        var weapon = ResolveMindBlade();
        if (weapon == null)
        {
            Log.Warn($"[MB] OnActivate: no MindBladeFormComponent found on {Owner.CharacterName}");
            return;
        }

        var slot = Owner.Descriptor.Body.PrimaryHand;
        Log.Info($"[MB] OnActivate ({weapon.name}): double={weapon.Double} " +
                 $"primaryHasItem={slot.HasItem} primaryItem={(slot.HasItem ? slot.Item?.Blueprint?.name : "empty")}");

        // If a mind blade is already in the primary hand, keep it if it's the right one, otherwise
        // unlock and remove it so the correct weapon can be equipped.
        if (slot.HasItem && slot.Item is ItemEntityWeapon equipped && MindBladeRegistry.IsMindBlade(equipped.Blueprint))
        {
            if (equipped.Blueprint == weapon) return; // already correct
            slot.Lock.Release();
            var old = slot.Item;
            slot.RemoveItem();
            old?.Collection?.Remove(old);
        }

        var item = new ItemEntityWeapon(weapon) { IsIdentified = true };
        Log.Info($"[MB] pre-equip ({weapon.name}): canInsertPrimary={slot.CanInsertItem(item)} primaryLocked={slot.Lock.Value} " +
                 $"| possible={slot.IsPossibleInsertItems()} supported={slot.IsItemSupported(item)} " +
                 $"canEquip={item.CanBeEquippedBy(Owner.Descriptor)} canTakeOneHand={item.CanTakeOneHand(Owner)}");

        Owner.Inventory.Add(item);
        slot.InsertItem(item);
        slot.Lock.Retain();

        Log.Info($"[MB] post-insert ({weapon.name}): primaryHasItem={slot.HasItem} primaryItem={(slot.HasItem ? slot.Item?.Blueprint?.name : "empty")} " +
                 $"itemWielder={(item.Wielder != null)}");

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

        var slot = Owner.Descriptor.Body.PrimaryHand;
        if (slot.HasItem && slot.Item is ItemEntityWeapon equipped && MindBladeRegistry.IsMindBlade(equipped.Blueprint))
        {
            slot.Lock.Release();
            var item = slot.Item;
            slot.RemoveItem();
            item?.Collection?.Remove(item);
            Log.Info($"[MB] Mind blade dismissed for {Owner.CharacterName}");
        }
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
