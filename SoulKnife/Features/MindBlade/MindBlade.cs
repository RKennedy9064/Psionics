using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.MindBlade;

public static class MindBlade
{
    private static readonly LogWrapper Log = LogWrapper.Get("Psionics");

    private static readonly HashSet<WeaponCategory> Excluded =
    [
        WeaponCategory.UnarmedStrike,
        WeaponCategory.KineticBlast,
        WeaponCategory.Touch,
        WeaponCategory.Ray,
        WeaponCategory.Bomb,
        // Shields are not valid mind blade forms
        WeaponCategory.WeaponLightShield,
        WeaponCategory.WeaponHeavyShield,
        WeaponCategory.SpikedLightShield,
        WeaponCategory.SpikedHeavyShield,
    ];

    public static void Configure()
    {
        var icon = AbilityRefs.MagicWeapon.Reference.Get().Icon;

        // Legacy fixed-form blueprints. No longer equipped (each chosen weapon gets its own
        // dedicated immutable blueprint below), but kept defined + registered so a save that has an
        // old-style mind blade equipped still resolves and is recognized until the buff re-equips.
        CreateWeaponBlueprints();
        foreach (var g in new[]
                 {
                     Guids.MindBladeLightWeapon, Guids.MindBladeOneHandedWeapon,
                     Guids.MindBladeTwoHandedWeapon, Guids.MindBladeDoublePrimaryWeapon,
                     Guids.MindBladeDoubleOffhandWeapon,
                 })
            MindBladeRegistry.Register(BlueprintTool.Get<BlueprintItemWeapon>(g));

        // Legacy form toggles (one per handedness), kept defined + registered for save compatibility
        // with characters created before per-weapon toggles existed. New picks below grant their own
        // per-category toggle carrying the chosen weapon's icon and name.
        CreateFormToggle("Light",     Guids.MindBladeLightBuff,     Guids.MindBladeLightToggle,     icon);
        CreateFormToggle("OneHanded", Guids.MindBladeOneHandedBuff, Guids.MindBladeOneHandedToggle, icon);
        CreateFormToggle("TwoHanded", Guids.MindBladeTwoHandedBuff, Guids.MindBladeTwoHandedToggle, icon);
        CreateFormToggle("Double",    Guids.MindBladeDoubleBuff,    Guids.MindBladeDoubleToggle,    icon);

        // Crit-normalizing references (every mind blade form is 19-20/×2 regardless of the weapon
        // it emulates), taken from the standard weapon of each form.
        var lightNorm  = ItemWeaponRefs.StandardShortsword.Reference.Get()?.m_Type?.Get();
        var oneNorm    = ItemWeaponRefs.StandardLongsword.Reference.Get()?.m_Type?.Get();
        var twoNorm    = ItemWeaponRefs.StandardGreatsword.Reference.Get()?.m_Type?.Get();
        var doubleNorm = ItemWeaponRefs.StandardDoubleSword.Reference.Get()?.m_Type?.Get();

        // Single combined selection: every non-shield weapon. The form (and thus damage/crit) is
        // derived from each weapon's own handedness; a dagger summons a 1d6 light blade, a longsword
        // a 1d8 one-handed blade, a greatsword a 2d6 two-handed blade.
        var conf = FeatureSelectionConfigurator.New("SKMindBladeFormSelection", Guids.MindBladeFeature)
            .SetDisplayName(Loc.Str("SK.MB.Selection.Name", "Form Mind Blade"))
            .SetDescription(Loc.Str("SK.MB.Selection.Desc",
                "Choose the weapon your mind blade manifests as. Its damage and critical range are set by " +
                "the weapon's form: light (1d6), one-handed (1d8), or two-handed (2d6), all 19-20/×2. " +
                "The mind blade is treated as a magic weapon, and you are always proficient with it."))
            .SetIcon(icon)
            .SetIsClassFeature(true);

        foreach (WeaponSubCategory sub in new[] { WeaponSubCategory.Simple, WeaponSubCategory.Martial, WeaponSubCategory.Exotic })
        {
            foreach (var cat in WeaponCategoryExtension.GetAllCategories(sub))
            {
                if (Excluded.Contains(cat)) continue;
                var weaponRef = FindWeaponRef(cat);
                if (weaponRef == null) continue;

                var srcWeapon = weaponRef.Get();
                var wtype = srcWeapon?.m_Type?.Get();
                if (wtype == null) continue;

                // Map handedness → form toggle + normalized damage/crit reference.
                // Double weapons (two ends) are detected first; they become a double mind blade.
                // NOTE: classify light by m_IsLight, NOT one-handed by m_IsOneHanded. In WotR's vanilla
                // weapon blueprints m_IsOneHanded is left false on nearly every weapon (only the Rapier
                // sets it), and m_IsLight is what actually distinguishes light from one-handed.
                string formLabel;
                DiceFormula formDice;
                BlueprintWeaponType norm;
                bool isDouble = false;
                if (srcWeapon.Double)
                {
                    formDice   = new DiceFormula(1, DiceType.D8);
                    norm       = doubleNorm;
                    formLabel  = "double (1d8/1d8)";
                    isDouble   = true;
                }
                else if (wtype.m_IsTwoHanded)
                {
                    formDice   = new DiceFormula(2, DiceType.D6);
                    norm       = twoNorm;
                    formLabel  = "two-handed (2d6)";
                }
                else if (wtype.m_IsLight)
                {
                    formDice   = new DiceFormula(1, DiceType.D6);
                    norm       = lightNorm;
                    formLabel  = "light (1d6)";
                }
                else
                {
                    formDice   = new DiceFormula(1, DiceType.D8);
                    norm       = oneNorm;
                    formLabel  = "one-handed (1d8)";
                }

                // Each chosen weapon gets its own dedicated, immutable type + item blueprint with the
                // chosen weapon's look/reach/group baked in and the form's normalized damage/crit.
                var itemRef = BuildPerCategory(cat, srcWeapon, formDice, norm, isDouble);

                // Per-character form toggle: its own buff + activatable carrying the chosen weapon's
                // icon and name, so the action bar button and tooltip show exactly what was picked.
                var name       = FormatName(cat);
                var togIcon    = srcWeapon.Icon ?? icon;
                var toggleGuid = CreatePerCategoryToggle(cat.ToString(), name, formLabel, togIcon);

                conf = conf.AddToAllFeatures(MakeWeaponEntry(cat, toggleGuid, itemRef, formLabel, togIcon));
            }
        }

        conf.Configure();
    }

    // ── Per-category blueprint creation ─────────────────────────────────────────
    // Builds the dedicated weapon type + item for one chosen weapon category and registers them.
    // Returns the primary item reference the feature points at.
    private static BlueprintItemWeaponReference BuildPerCategory(
        WeaponCategory cat, BlueprintItemWeapon src, DiceFormula formDice,
        BlueprintWeaponType norm, bool isDouble)
    {
        var catStr   = cat.ToString();
        var typeGuid = DeterministicGuid($"SK.MB.Type.{catStr}");
        var itemGuid = DeterministicGuid($"SK.MB.Item.{catStr}");

        CreatePerCategoryType($"SKMBType_{catStr}", typeGuid, cat, src, formDice, norm);

        if (isDouble)
        {
            var offGuid = DeterministicGuid($"SK.MB.Offhand.{catStr}");
            CreatePerCategoryItem($"SKMBOff_{catStr}", offGuid, typeGuid, src, makeDouble: false, secondRef: null);
            var offRef = BlueprintTool.GetRef<BlueprintItemWeaponReference>(offGuid);
            CreatePerCategoryItem($"SKMBItem_{catStr}", itemGuid, typeGuid, src, makeDouble: true, secondRef: offRef);
            MindBladeRegistry.Register(BlueprintTool.Get<BlueprintItemWeapon>(offGuid));
        }
        else
        {
            CreatePerCategoryItem($"SKMBItem_{catStr}", itemGuid, typeGuid, src, makeDouble: false, secondRef: null);
        }

        MindBladeRegistry.Register(BlueprintTool.Get<BlueprintItemWeapon>(itemGuid));
        return BlueprintTool.GetRef<BlueprintItemWeaponReference>(itemGuid);
    }

    // A per-category weapon type: chosen weapon's mechanics + look, the form's normalized damage,
    // and 19-20/×2 critical (from the form's standard reference, not the chosen weapon).
    private static void CreatePerCategoryType(
        string name, string guid, WeaponCategory cat, BlueprintItemWeapon src,
        DiceFormula formDice, BlueprintWeaponType norm)
    {
        var srcType = src?.m_Type?.Get();

        WeaponTypeConfigurator.New(name, guid)
            .SetCategory(cat)
            .SetBaseDamage(formDice)
            .OnConfigure(bp =>
            {
                if (srcType != null)
                {
                    bp.m_DamageType              = srcType.m_DamageType;
                    bp.m_AttackType              = srcType.m_AttackType;
                    bp.m_AttackRange            = srcType.m_AttackRange;   // reach / range
                    bp.m_FighterGroupFlags      = srcType.m_FighterGroupFlags;
                    bp.m_IsTwoHanded            = srcType.m_IsTwoHanded;
                    bp.m_IsOneHanded            = srcType.m_IsOneHanded;
                    bp.m_IsLight                = srcType.m_IsLight;
                    bp.m_IsMonk                 = srcType.m_IsMonk;
                    bp.m_IsNatural              = srcType.m_IsNatural;
                    bp.m_IsUnarmed              = srcType.m_IsUnarmed;
                    bp.m_OverrideAttackBonusStat = srcType.m_OverrideAttackBonusStat;
                    bp.m_AttackBonusStatOverride = srcType.m_AttackBonusStatOverride;
                    bp.m_Weight                 = srcType.m_Weight;
                    bp.m_VisualParameters       = srcType.m_VisualParameters;
                    bp.m_Icon                   = srcType.m_Icon;
                    bp.m_TypeNameText           = srcType.m_TypeNameText;
                    bp.m_DefaultNameText        = srcType.m_DefaultNameText;
                    bp.m_DescriptionText        = srcType.m_DescriptionText;
                }
                // Normalize critical to the form's 19-20/×2 regardless of the emulated weapon.
                if (norm != null)
                {
                    bp.m_CriticalRollEdge = norm.m_CriticalRollEdge;
                    bp.m_CriticalModifier = norm.m_CriticalModifier;
                }
            })
            .Configure();
    }

    private static void CreatePerCategoryItem(
        string name, string guid, string weaponTypeGuid, BlueprintItemWeapon src,
        bool makeDouble, BlueprintItemWeaponReference secondRef)
    {
        ItemWeaponConfigurator.New(name, guid)
            .SetType(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(weaponTypeGuid))
            .SetSize(Size.Medium)
            .OnConfigure(bp =>
            {
                bp.m_OverrideDamageDice = false;
                // Item-level fields drive the equipped 3D model, name, and inventory icon.
                bp.m_VisualParameters = src.m_VisualParameters;
                bp.m_DisplayNameText  = src.m_DisplayNameText;
                if (src.Icon != null) bp.m_Icon = src.Icon;
                if (makeDouble)
                {
                    bp.Double         = true;
                    bp.CountAsDouble  = true;
                    bp.m_SecondWeapon = secondRef;
                }
            })
            .Configure();
    }

    // ── Legacy blueprint creation (kept for save compatibility) ─────────────────
    private static void CreateWeaponBlueprints()
    {
        CreateWeaponTypeBP("SKMindBladeLightType",     Guids.MindBladeLightWeaponType,
            new DiceFormula(1, DiceType.D6), WeaponCategory.Shortsword,
            ItemWeaponRefs.StandardShortsword.Reference.Get());

        CreateWeaponTypeBP("SKMindBladeOneHandedType", Guids.MindBladeOneHandedWeaponType,
            new DiceFormula(1, DiceType.D8), WeaponCategory.Longsword,
            ItemWeaponRefs.StandardLongsword.Reference.Get());

        CreateWeaponTypeBP("SKMindBladeTwoHandedType", Guids.MindBladeTwoHandedWeaponType,
            new DiceFormula(2, DiceType.D6), WeaponCategory.Greatsword,
            ItemWeaponRefs.StandardGreatsword.Reference.Get());

        CreateItemWeaponBP("SKMindBladeLightWeapon",     Guids.MindBladeLightWeapon,     Guids.MindBladeLightWeaponType,
            ItemWeaponRefs.StandardShortsword.Reference.Get());
        CreateItemWeaponBP("SKMindBladeOneHandedWeapon", Guids.MindBladeOneHandedWeapon, Guids.MindBladeOneHandedWeaponType,
            ItemWeaponRefs.StandardLongsword.Reference.Get());
        CreateItemWeaponBP("SKMindBladeTwoHandedWeapon", Guids.MindBladeTwoHandedWeapon, Guids.MindBladeTwoHandedWeaponType,
            ItemWeaponRefs.StandardGreatsword.Reference.Get());

        CreateDoubleWeaponBlueprints();
    }

    private static void CreateDoubleWeaponBlueprints()
    {
        var doubleSword = ItemWeaponRefs.StandardDoubleSword.Reference.Get();

        CreateWeaponTypeBP("SKMindBladeDoubleType", Guids.MindBladeDoubleWeaponType,
            new DiceFormula(1, DiceType.D8), WeaponCategory.DoubleSword, doubleSword);

        CreateItemWeaponBP("SKMindBladeDoubleOffhand", Guids.MindBladeDoubleOffhandWeapon,
            Guids.MindBladeDoubleWeaponType, doubleSword);

        var offhandRef = BlueprintTool.GetRef<BlueprintItemWeaponReference>(Guids.MindBladeDoubleOffhandWeapon);

        ItemWeaponConfigurator.New("SKMindBladeDoublePrimary", Guids.MindBladeDoublePrimaryWeapon)
            .SetType(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(Guids.MindBladeDoubleWeaponType))
            .SetSize(Size.Medium)
            .OnConfigure(bp =>
            {
                bp.m_OverrideDamageDice = false;
                bp.Double          = true;
                bp.CountAsDouble   = true;
                bp.m_SecondWeapon  = offhandRef;
                if (doubleSword?.Icon != null) bp.m_Icon = doubleSword.Icon;
                bp.m_VisualParameters = doubleSword.m_VisualParameters;
            })
            .Configure();
    }

    private static void CreateWeaponTypeBP(
        string name, string guid, DiceFormula dice, WeaponCategory category,
        BlueprintItemWeapon sourceItem)
    {
        var sourceType = sourceItem?.m_Type?.Get();

        WeaponTypeConfigurator.New(name, guid)
            .SetCategory(category)
            .SetBaseDamage(dice)
            .OnConfigure(bp =>
            {
                if (sourceType == null) return;
                bp.m_DamageType            = sourceType.m_DamageType;
                bp.m_CriticalRollEdge      = sourceType.m_CriticalRollEdge;
                bp.m_CriticalModifier      = sourceType.m_CriticalModifier;
                bp.m_AttackType            = sourceType.m_AttackType;
                bp.m_AttackRange           = sourceType.m_AttackRange;
                bp.m_FighterGroupFlags     = sourceType.m_FighterGroupFlags;
                bp.m_IsTwoHanded           = sourceType.m_IsTwoHanded;
                bp.m_IsOneHanded           = sourceType.m_IsOneHanded;
                bp.m_IsLight               = sourceType.m_IsLight;
                bp.m_IsMonk                = sourceType.m_IsMonk;
                bp.m_IsNatural             = sourceType.m_IsNatural;
                bp.m_IsUnarmed             = sourceType.m_IsUnarmed;
                bp.m_OverrideAttackBonusStat = sourceType.m_OverrideAttackBonusStat;
                bp.m_AttackBonusStatOverride = sourceType.m_AttackBonusStatOverride;
                bp.m_Weight                = sourceType.m_Weight;
                bp.m_VisualParameters      = sourceType.m_VisualParameters;
                bp.m_Icon                  = sourceType.m_Icon;
                bp.m_TypeNameText          = sourceType.m_TypeNameText;
                bp.m_DefaultNameText       = sourceType.m_DefaultNameText;
                bp.m_DescriptionText       = sourceType.m_DescriptionText;
                bp.m_MasterworkDescriptionText = sourceType.m_MasterworkDescriptionText;
                bp.m_MagicDescriptionText  = sourceType.m_MagicDescriptionText;
            })
            .Configure();
    }

    private static void CreateItemWeaponBP(string name, string guid, string weaponTypeGuid, BlueprintItemWeapon sourceItem)
    {
        ItemWeaponConfigurator.New(name, guid)
            .SetType(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(weaponTypeGuid))
            .SetSize(Size.Medium)
            .OnConfigure(bp =>
            {
                bp.m_OverrideDamageDice = false;
                if (sourceItem?.Icon != null) bp.m_Icon = sourceItem.Icon;
                bp.m_VisualParameters = sourceItem.m_VisualParameters;
            })
            .Configure();
    }

    // ── Form toggle ────────────────────────────────────────────────────────────
    private static (Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff,
                    Kingmaker.UnitLogic.ActivatableAbilities.BlueprintActivatableAbility)
        CreateFormToggle(string formKey, string buffGuid, string toggleGuid, UnityEngine.Sprite icon)
    {
        var buff = BuffConfigurator.New($"SKMBBuff{formKey}", buffGuid)
            .SetDisplayName(Loc.Str($"SK.MB.{formKey}.BN", "Mind Blade"))
            .SetDescription(Loc.Str($"SK.MB.{formKey}.BD",
                "Your mind blade is manifested. It is bound to your primary hand and cannot be unequipped while active."))
            .SetIcon(icon)
            .AddComponent(new MindBladeComponent())
            .Configure();

        var toggle = ActivatableAbilityConfigurator.New($"SKMBToggle{formKey}", toggleGuid)
            .SetDisplayName(Loc.Str($"SK.MB.{formKey}.TN", "Form Mind Blade"))
            .SetDescription(Loc.Str($"SK.MB.{formKey}.TD",
                "Toggle. Manifest or dismiss your mind blade. The blade cannot be unequipped while active."))
            .SetIcon(icon)
            .SetBuff(buff)
            .SetActivationType(AbilityActivationType.Immediately)
            // Instant dismiss (like the kineticist's kinetic blade): without this, DeactivateImmediately
            // defaults to false and the blade lingers until the round ticks over.
            .SetDeactivateImmediately(true)
            .SetGroup(ActivatableAbilityGroup.None)
            .Configure();

        return (buff, toggle);
    }

    // Per-category form toggle: a dedicated buff + activatable for one chosen weapon, carrying that
    // weapon's icon and name so the action bar and tooltip reflect the exact pick. Returns the toggle's
    // GUID for the weapon entry to grant.
    private static string CreatePerCategoryToggle(
        string catStr, string weaponName, string formLabel, UnityEngine.Sprite icon)
    {
        var buffGuid   = DeterministicGuid($"SK.MB.Buff.{catStr}");
        var toggleGuid = DeterministicGuid($"SK.MB.Toggle.{catStr}");

        var buff = BuffConfigurator.New($"SKMBBuff_{catStr}", buffGuid)
            .SetDisplayName(Loc.Str($"SK.MB.Buff.{catStr}.BN", "Mind Blade"))
            .SetDescription(Loc.Str($"SK.MB.Buff.{catStr}.BD",
                $"Your mind blade is manifested as a {weaponName.ToLower()}. It is bound to your primary " +
                "hand and cannot be unequipped while active."))
            .SetIcon(icon)
            .AddComponent(new MindBladeComponent())
            .Configure();

        ActivatableAbilityConfigurator.New($"SKMBToggle_{catStr}", toggleGuid)
            .SetDisplayName(Loc.Str($"SK.MB.Toggle.{catStr}.TN", "Form Mind Blade"))
            .SetDescription(Loc.Str($"SK.MB.Toggle.{catStr}.TD",
                $"Toggle. Manifest or dismiss your mind blade, which takes the form of a {weaponName.ToLower()} " +
                $"({formLabel}). The blade cannot be unequipped while active."))
            .SetIcon(icon)
            .SetBuff(buff)
            .SetActivationType(AbilityActivationType.Immediately)
            .SetDeactivateImmediately(true)
            .SetGroup(ActivatableAbilityGroup.None)
            .Configure();

        return toggleGuid;
    }

    // ── Weapon entry ───────────────────────────────────────────────────────────
    // One entry per weapon category. Grants the form toggle matching the weapon's handedness,
    // proficiency for the chosen weapon, and records the dedicated per-category item to equip.
    private static Kingmaker.Blueprints.Classes.BlueprintFeature
        MakeWeaponEntry(WeaponCategory cat, string toggleGuid,
                        BlueprintItemWeaponReference itemRef, string formLabel,
                        UnityEngine.Sprite icon)
    {
        var name   = FormatName(cat);
        var catStr = cat.ToString();

        return FeatureConfigurator.New($"SKMBWeapon{catStr}", DeterministicGuid($"SK.MB.Weapon.{catStr}"))
            .SetDisplayName(Loc.Str($"SK.MB.Weapon.{catStr}.Name", name))
            .SetDescription(Loc.Str($"SK.MB.Weapon.{catStr}.Desc",
                $"Your mind blade manifests as a {name.ToLower()} — a {formLabel} weapon. " +
                "Toggle Form Mind Blade on your action bar to summon or dismiss it."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFacts([toggleGuid])
            // Grant proficiency for the exact weapon the player chose, so an exotic mind blade
            // (two-bladed sword, elven curved blade, etc.) can be equipped.
            .AddComponent(new AddProficiencies { WeaponProficiencies = [cat] })
            .AddComponent(new MindBladeFormComponent { ItemRef = itemRef })
            .Configure();
    }

    // ── Helpers ────────────────────────────────────────────────────────────────
    private static string DeterministicGuid(string key)
    {
        using var md5 = MD5.Create();
        return new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(key))).ToString();
    }

    private static string FormatName(WeaponCategory cat) =>
        Regex.Replace(cat.ToString(), @"(?<!^)([A-Z])", " $1");

    private static BlueprintItemWeaponReference FindWeaponRef(WeaponCategory cat)
    {
        var catName  = cat.ToString();
        var flags    = BindingFlags.Public | BindingFlags.Static;
        var refsType = typeof(ItemWeaponRefs);

        var allNames = new List<string>();
        foreach (var f in refsType.GetFields(flags)) allNames.Add(f.Name);
        foreach (var p in refsType.GetProperties(flags)) allNames.Add(p.Name);

        var result = TryExtractRef(refsType, $"Standard{catName}", flags);
        if (result != null) return result;

        foreach (var memberName in allNames)
        {
            if (!memberName.StartsWith("Standard", StringComparison.OrdinalIgnoreCase)) continue;
            if (!memberName.EndsWith(catName, StringComparison.OrdinalIgnoreCase)) continue;
            result = TryExtractRef(refsType, memberName, flags);
            if (result != null) return result;
        }

        result = TryExtractRef(refsType, $"{catName}Plus1", flags);
        if (result != null) return result;

        foreach (var memberName in allNames)
        {
            if (!memberName.StartsWith(catName, StringComparison.OrdinalIgnoreCase)) continue;
            if (!memberName.EndsWith("Plus1", StringComparison.Ordinal)) continue;
            result = TryExtractRef(refsType, memberName, flags);
            if (result != null) return result;
        }

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
                if (prop != null) value = prop.GetValue(null);
            }
            if (value == null) return null;
            var refField = value.GetType().GetField("Reference", BindingFlags.Public | BindingFlags.Instance);
            if (refField == null) return null;
            if (refField.GetValue(value) is not BlueprintReferenceBase refBase) return null;
            var guid = refBase.deserializedGuid.ToString();
            if (string.IsNullOrEmpty(guid) || guid == "00000000-0000-0000-0000-000000000000") return null;
            return BlueprintTool.GetRef<BlueprintItemWeaponReference>(guid);
        }
        catch (Exception ex)
        {
            Log.Warn($"[MB] TryExtractRef({memberName}): {ex.Message}");
            return null;
        }
    }
}
