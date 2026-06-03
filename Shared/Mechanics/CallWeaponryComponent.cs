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

namespace Psionics.Shared.Mechanics;

[Serializable]
public class CallWeaponryComponent : UnitFactComponentDelegate, IUnitLevelUpHandler
{
    public WeaponCategory Category;
    public BlueprintItemWeaponReference WeaponRef;

    [NonSerialized]
    private int _appliedEnhancementLevel;

    private static readonly Dictionary<WeaponCategory, BlueprintItemWeapon> s_Cache = [];
    private static readonly LogWrapper Log = LogWrapper.Get("Psionics");

    private BlueprintItemWeapon FindWeapon()
    {
        if (s_Cache.TryGetValue(Category, out var cached))
            return cached;

        var weapon = WeaponRef?.Get();
        if (weapon != null)
        {
            s_Cache[Category] = weapon;
            Log.Info($"[CW] FindWeapon({Category}): loaded {weapon.name}");
        }
        else
        {
            Log.Warn($"[CW] FindWeapon({Category}): WeaponRef is null or Get() returned null (ref guid={WeaponRef?.deserializedGuid})");
        }
        return weapon;
    }

    public override void OnActivate()
    {
        Log.Info($"[CW] OnActivate({Category}) owner={Owner?.CharacterName}");

        foreach (var buff in Owner.Descriptor.Buffs.Enumerable.ToList())
        {
            if (buff != Fact && buff.Blueprint.ComponentsArray.OfType<CallWeaponryComponent>().Any())
            {
                Log.Info($"[CW] Removing conflicting buff: {buff.Blueprint.name}");
                buff.Remove();
                break;
            }
        }

        var weapon = FindWeapon();
        if (weapon == null)
        {
            Log.Warn($"[CW] OnActivate({Category}): no weapon blueprint, aborting");
            return;
        }

        var slot = Owner.Descriptor.Body.PrimaryHand;
        Log.Info($"[CW] slot.HasItem={slot.HasItem} slot.Item={(slot.HasItem ? slot.Item?.Blueprint?.name : "empty")}");

        if (slot.HasItem && slot.Item?.Blueprint == weapon) return;

        var item = new ItemEntityWeapon(weapon)
        {
            IsIdentified = true
        };
        Owner.Inventory.Add(item);
        Log.Info($"[CW] After Inventory.Add: collection={item.Collection}");
        slot.InsertItem(item);
        Log.Info($"[CW] After InsertItem: slot.HasItem={slot.HasItem} slot.Item={(slot.HasItem ? slot.Item?.Blueprint?.name : "empty")}");
        slot.Lock.Retain();

        var visualEnch = BlueprintTool.Get<BlueprintWeaponEnchantment>(Psionics.Utils.Guids.CallWeaponryVisualEnchantment);
        if (visualEnch != null)
            item.AddEnchantment(visualEnch, null);

        var pwClass = BlueprintTool.Get<BlueprintCharacterClass>(Psionics.Utils.Guids.PsychicWarriorClass);
        var classLevel = Owner.Descriptor.Progression.GetClassLevel(pwClass);
        _appliedEnhancementLevel = ApplyEnchantment(item, 0, classLevel);

        EventBus.Subscribe(this);
    }

    public override void OnDeactivate()
    {
        Log.Info($"[CW] OnDeactivate({Category}) owner={Owner?.CharacterName}");

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
            Log.Info($"[CW] Weapon removed from slot and inventory");
        }
        else
        {
            Log.Warn($"[CW] OnDeactivate: slot mismatch — expected {weapon.name}, found {(slot.HasItem ? slot.Item?.Blueprint?.name : "empty")}");
        }

        _appliedEnhancementLevel = 0;
    }

    // IUnitLevelUpHandler
    public void HandleUnitBeforeLevelUp(UnitEntityData unit) { }

    public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller)
    {
        if (unit != Owner) return;

        var slot = Owner.Descriptor.Body.PrimaryHand;
        if (!slot.HasItem) return;

        var weapon = FindWeapon();
        if (weapon == null || slot.Item?.Blueprint != weapon) return;

        if (slot.Item is not ItemEntityWeapon item) return;

        var pwClass = BlueprintTool.Get<BlueprintCharacterClass>(Psionics.Utils.Guids.PsychicWarriorClass);
        var classLevel = Owner.Descriptor.Progression.GetClassLevel(pwClass);
        _appliedEnhancementLevel = ApplyEnchantment(item, _appliedEnhancementLevel, classLevel);
    }

    private int ApplyEnchantment(ItemEntityWeapon item, int currentLevel, int classLevel)
    {
        var newLevel = GetEnhancementLevel(classLevel);

        if (currentLevel > 0)
        {
            var oldBlueprint = GetEnhancementEnchantment(currentLevel);
            if (oldBlueprint != null)
            {
                var existing = item.GetEnchantment(oldBlueprint);
                if (existing != null) item.RemoveEnchantment(existing);
            }
        }

        if (newLevel > 0)
        {
            var ench = GetEnhancementEnchantment(newLevel);
            if (ench != null) item.AddEnchantment(ench, null);
        }

        Log.Info($"[CW] Enchantment: +{currentLevel} → +{newLevel} for {Category} at class level {classLevel}");
        return newLevel;
    }

    private static int GetEnhancementLevel(int classLevel)
    {
        if (classLevel >= 19) return 5;
        if (classLevel >= 15) return 4;
        if (classLevel >= 11) return 3;
        if (classLevel >= 7) return 2;
        if (classLevel >= 3) return 1;
        return 0;
    }

    private static BlueprintWeaponEnchantment GetEnhancementEnchantment(int level)
    {
        return level switch
        {
            1 => WeaponEnchantmentRefs.Enhancement1.Reference.Get(),
            2 => WeaponEnchantmentRefs.Enhancement2.Reference.Get(),
            3 => WeaponEnchantmentRefs.Enhancement3.Reference.Get(),
            4 => WeaponEnchantmentRefs.Enhancement4.Reference.Get(),
            5 => WeaponEnchantmentRefs.Enhancement5.Reference.Get(),
            _ => null
        };
    }
}
