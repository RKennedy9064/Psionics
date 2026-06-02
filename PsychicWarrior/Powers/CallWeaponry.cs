using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class CallWeaponry
{
    // Categories that make no sense to "call" as a weapon
    private static readonly HashSet<WeaponCategory> Excluded =
    [
        WeaponCategory.UnarmedStrike,
        WeaponCategory.KineticBlast,
        WeaponCategory.Touch,
        WeaponCategory.Ray,
        WeaponCategory.Bomb,
    ];

    // Populated during Configure(); used by MindKnightPath to share features with its selection.
    public static readonly List<BlueprintFeature> WeaponFeatureList = [];

    private static readonly LogWrapper Log = LogWrapper.Get("Psionics");

    public static void Configure()
    {
        WeaponFeatureList.Clear();

        // Cosmetic enchantment: borrows BrilliantEnergy's weapon FX for the astral glow visual.
        // No mechanical components — purely a visual effect on the called weapon.
        WeaponEnchantmentConfigurator.New("CWAstralGlow", Guids.CallWeaponryVisualEnchantment)
            .OnConfigure(bp =>
            {
                var source = WeaponEnchantmentRefs.BrilliantEnergy.Reference.Get();
                bp.WeaponFxPrefab = source.WeaponFxPrefab;
            })
            .Configure();

        var icon = AbilityRefs.MagicWeapon.Reference.Get().Icon;

        var categories = new List<WeaponCategory>();
        foreach (WeaponSubCategory sub in new[] { WeaponSubCategory.Simple, WeaponSubCategory.Martial, WeaponSubCategory.Exotic })
        {
            foreach (var cat in WeaponCategoryExtension.GetAllCategories(sub))
            {
                if (!Excluded.Contains(cat) && !categories.Contains(cat))
                    categories.Add(cat);
            }
        }

        var conf = FeatureSelectionConfigurator.New("CallWeaponrySelection", Guids.CallWeaponrySelection)
            .SetDisplayName(Loc.Str("PW.CW.Selection.Name", "Call Weaponry"))
            .SetDescription(Loc.Str("PW.CW.Selection.Desc",
                "Choose a weapon to add to your Call Weaponry repertoire. You may psionically call this weapon to your primary hand at will. Only weapons you are proficient with appear in this list. You gain a new weapon choice at 3rd, 7th, 11th, 15th, and 19th level (five additional choices)."))
            .SetIcon(icon)
            .SetIsClassFeature(true)
            .SetIgnorePrerequisites(false);

        foreach (var cat in categories)
        {
            var f = MakeWeapon(cat, icon);
            conf = conf.AddToAllFeatures(f);
            WeaponFeatureList.Add(f);
        }

        conf.Configure();
    }

    private static string Guid(string key)
    {
        using var md5 = MD5.Create();
        return new System.Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(key))).ToString();
    }

    private static string FormatName(WeaponCategory cat) =>
        Regex.Replace(cat.ToString(), @"(?<!^)([A-Z])", " $1");

    // Finds a BlueprintItemWeaponReference for the given category. Prefers the standard
    // (unenchanted) base variant so all enhancement comes from the scaled buff. Falls back
    // to the Plus1 variant for categories without a Standard entry.
    private static BlueprintItemWeaponReference FindWeaponRef(WeaponCategory cat)
    {
        var catName = cat.ToString();
        var flags = BindingFlags.Public | BindingFlags.Static;
        var refsType = typeof(ItemWeaponRefs);

        var allNames = new List<string>();
        foreach (var f in refsType.GetFields(flags)) allNames.Add(f.Name);
        foreach (var p in refsType.GetProperties(flags)) allNames.Add(p.Name);

        // Primary: Standard{Category} (exact, then case-insensitive suffix match)
        var result = TryExtractRef(refsType, $"Standard{catName}", flags);
        if (result != null) return result;

        foreach (var memberName in allNames)
        {
            if (!memberName.StartsWith("Standard", StringComparison.OrdinalIgnoreCase)) continue;
            if (!memberName.EndsWith(catName, StringComparison.OrdinalIgnoreCase)) continue;
            result = TryExtractRef(refsType, memberName, flags);
            if (result != null) return result;
        }

        // Fallback: {Category}Plus1 (exact, then case-insensitive)
        result = TryExtractRef(refsType, $"{catName}Plus1", flags);
        if (result != null) return result;

        foreach (var memberName in allNames)
        {
            if (!memberName.StartsWith(catName, StringComparison.OrdinalIgnoreCase)) continue;
            if (!memberName.EndsWith("Plus1", StringComparison.Ordinal)) continue;
            result = TryExtractRef(refsType, memberName, flags);
            if (result != null) return result;
        }

        Log.Warn($"[CW] No weapon ref found in ItemWeaponRefs for category {catName}");
        return null;
    }

    private static BlueprintItemWeaponReference TryExtractRef(Type refsType, string memberName, BindingFlags flags)
    {
        try
        {
            object value = null;
            var field = refsType.GetField(memberName, flags);
            if (field != null)
                value = field.GetValue(null);
            else
            {
                var prop = refsType.GetProperty(memberName, flags);
                if (prop != null)
                    value = prop.GetValue(null);
            }
            if (value == null) return null;
            // Blueprint<T>.Reference is a public instance field holding a BlueprintReference<T>
            var refFieldInfo = value.GetType().GetField("Reference", BindingFlags.Public | BindingFlags.Instance);
            if (refFieldInfo == null) return null;
            if (refFieldInfo.GetValue(value) is not BlueprintReferenceBase refBase) return null;
            var guid = refBase.deserializedGuid.ToString();
            if (string.IsNullOrEmpty(guid) || guid == "00000000-0000-0000-0000-000000000000") return null;
            return BlueprintTool.GetRef<BlueprintItemWeaponReference>(guid);
        }
        catch (Exception ex)
        {
            Log.Warn($"[CW] Exception extracting ref for {memberName}: {ex.Message}");
            return null;
        }
    }

    private static BlueprintFeature MakeWeapon(WeaponCategory cat, UnityEngine.Sprite icon)
    {
        var name = FormatName(cat);
        var catStr = cat.ToString();
        var weaponRef = FindWeaponRef(cat);

        var buff = BuffConfigurator.New($"CWBuff{catStr}", Guid($"PW.CW.Buff.{catStr}"))
            .SetDisplayName(Loc.Str($"PW.CW.{catStr}.BN", $"Called {name}"))
            .SetDescription(Loc.Str($"PW.CW.{catStr}.BD",
                $"A psionically-called {name.ToLower()} is bound to your primary hand. Enhancement bonus scales with psychic warrior level: +1 at 3rd, +2 at 7th, +3 at 11th, +4 at 15th, +5 at 19th."))
            .SetIcon(icon)
            .AddComponent(new CallWeaponryComponent { Category = cat, WeaponRef = weaponRef })
            .Configure();

        var activatable = ActivatableAbilityConfigurator.New($"CWToggle{catStr}", Guid($"PW.CW.Activatable.{catStr}"))
            .SetDisplayName(Loc.Str($"PW.CW.{catStr}.TN", $"Call {name}"))
            .SetDescription(Loc.Str($"PW.CW.{catStr}.TD",
                $"Toggle. Psionically call a {name.ToLower()} to your primary hand. The weapon cannot be unequipped while active, and gains a scaling enhancement bonus. Only one called weapon may be active at a time."))
            .SetIcon(icon)
            .SetBuff(buff)
            .SetActivationType(AbilityActivationType.Immediately)
            .SetGroup(ActivatableAbilityGroup.None)
            .Configure();

        return FeatureConfigurator.New($"CWFeature{catStr}", Guid($"PW.CW.Feature.{catStr}"))
            .SetDisplayName(Loc.Str($"PW.CW.{catStr}.FN", $"Call {name}"))
            .SetDescription(Loc.Str($"PW.CW.{catStr}.FD",
                $"You can psionically call a {name.ToLower()} to your primary hand. Toggle the ability on your action bar to summon or dismiss the weapon."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFacts([activatable.ToString()])
            .AddPrerequisiteProficiency(
                armorProficiencies: [],
                weaponProficiencies: [cat])
            .Configure();
    }
}
