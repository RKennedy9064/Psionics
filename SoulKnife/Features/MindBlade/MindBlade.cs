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

        // Create the three fixed weapon type + item blueprints
        CreateWeaponBlueprints();

        // Form toggles (buff + activatable), one per handedness
        var (_, lightToggle) = CreateFormToggle("Light",     Guids.MindBladeLightBuff,      Guids.MindBladeLightToggle,
                                   Guids.MindBladeLightWeapon,     WeaponCategory.Shortsword, icon);
        var (_, oneToggle)   = CreateFormToggle("OneHanded", Guids.MindBladeOneHandedBuff,   Guids.MindBladeOneHandedToggle,
                                   Guids.MindBladeOneHandedWeapon, WeaponCategory.Longsword,  icon);
        var (_, twoToggle)   = CreateFormToggle("TwoHanded", Guids.MindBladeTwoHandedBuff,   Guids.MindBladeTwoHandedToggle,
                                   Guids.MindBladeTwoHandedWeapon, WeaponCategory.Greatsword, icon);
        var (_, doubleToggle) = CreateFormToggle("Double",   Guids.MindBladeDoubleBuff,      Guids.MindBladeDoubleToggle,
                                   Guids.MindBladeDoublePrimaryWeapon, WeaponCategory.DoubleSword, icon);

        // Single combined selection: every non-shield weapon. The form (and thus damage/crit)
        // is derived automatically from each weapon's own handedness, so a dagger summons a
        // 1d6 light blade, a longsword a 1d8 one-handed blade, a greatsword a 2d6 two-handed blade.
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

                // Map handedness → form toggle + custom weapon type + damage label.
                // Double weapons (two ends) are detected first; they become a double mind blade.
                string toggleGuid, typeGuid, formLabel;
                if (srcWeapon.Double)
                {
                    toggleGuid = doubleToggle.AssetGuid.ToString();
                    typeGuid   = Guids.MindBladeDoubleWeaponType;
                    formLabel  = "double (1d8/1d8)";
                }
                else if (wtype.m_IsTwoHanded)
                {
                    toggleGuid = twoToggle.AssetGuid.ToString();
                    typeGuid   = Guids.MindBladeTwoHandedWeaponType;
                    formLabel  = "two-handed (2d6)";
                }
                else if (wtype.m_IsOneHanded)
                {
                    toggleGuid = oneToggle.AssetGuid.ToString();
                    typeGuid   = Guids.MindBladeOneHandedWeaponType;
                    formLabel  = "one-handed (1d8)";
                }
                else
                {
                    toggleGuid = lightToggle.AssetGuid.ToString();
                    typeGuid   = Guids.MindBladeLightWeaponType;
                    formLabel  = "light (1d6)";
                }

                conf = conf.AddToAllFeatures(MakeWeaponEntry(cat, weaponRef, toggleGuid, typeGuid, formLabel, icon));
            }
        }

        conf.Configure();
    }

    // ── Blueprint creation ─────────────────────────────────────────────────────
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

    // Double mind blade: a two-ended weapon. Each end deals one-handed form damage (1d8, 19-20/×2).
    // The primary item carries Double=true and links the off-hand half via m_SecondWeapon, so the
    // engine grants main- and off-hand attacks exactly like a real two-bladed sword.
    private static void CreateDoubleWeaponBlueprints()
    {
        var doubleSword = ItemWeaponRefs.StandardDoubleSword.Reference.Get();

        CreateWeaponTypeBP("SKMindBladeDoubleType", Guids.MindBladeDoubleWeaponType,
            new DiceFormula(1, DiceType.D8), WeaponCategory.DoubleSword, doubleSword);

        // Off-hand half first (the primary references it).
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

        var made = BlueprintTool.Get<BlueprintItemWeapon>(Guids.MindBladeDoublePrimaryWeapon);
        Log.Info($"[MB] ItemWeapon SKMindBladeDoublePrimary: double={made?.Double} second={made?.m_SecondWeapon?.deserializedGuid} " +
                 $"computedBaseDamage={made?.BaseDamage}");
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
                // Clone every stat/display field from the reference weapon type so the synthetic
                // type is fully initialized (a from-scratch type is missing m_DamageType etc.,
                // which makes the inventory tooltip mis-compute damage as 1-1).
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

        var made = BlueprintTool.Get<BlueprintWeaponType>(guid);
        Log.Info($"[MB] WeaponType {name}: baseDamage={made?.m_BaseDamage.Rolls}d{(int?)made?.m_BaseDamage.Dice} " +
                 $"crit={made?.m_CriticalRollEdge} x{made?.m_CriticalModifier} 2h={made?.m_IsOneHanded == false}");
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
                // Item-level visual drives the equipped 3D model; seed it from the reference weapon.
                bp.m_VisualParameters = sourceItem.m_VisualParameters;
            })
            .Configure();

        var made = BlueprintTool.Get<BlueprintItemWeapon>(guid);
        Log.Info($"[MB] ItemWeapon {name}: size={made?.m_Size} icon={(made?.m_Icon != null)} " +
                 $"computedBaseDamage={made?.BaseDamage}");
    }

    // ── Form toggle ────────────────────────────────────────────────────────────
    private static (Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff,
                    Kingmaker.UnitLogic.ActivatableAbilities.BlueprintActivatableAbility)
        CreateFormToggle(string formKey, string buffGuid, string toggleGuid,
                         string weaponGuid, WeaponCategory profCategory,
                         UnityEngine.Sprite icon)
    {
        var weaponRef = BlueprintTool.GetRef<BlueprintItemWeaponReference>(weaponGuid);

        var buff = BuffConfigurator.New($"SKMBBuff{formKey}", buffGuid)
            .SetDisplayName(Loc.Str($"SK.MB.{formKey}.BN", "Mind Blade"))
            .SetDescription(Loc.Str($"SK.MB.{formKey}.BD",
                "Your mind blade is manifested. It is bound to your primary hand and cannot be unequipped while active."))
            .SetIcon(icon)
            .AddComponent(new MindBladeComponent { Category = profCategory, WeaponRef = weaponRef })
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

    // ── Weapon entry ───────────────────────────────────────────────────────────
    // One entry per weapon category. Grants the form toggle matching the weapon's handedness
    // and applies the weapon's visual to that form's custom weapon type blueprint.
    private static Kingmaker.Blueprints.Classes.BlueprintFeature
        MakeWeaponEntry(WeaponCategory cat, BlueprintItemWeaponReference weaponRef,
                        string toggleGuid, string targetWeaponTypeGuid, string formLabel,
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
            // (two-bladed sword, elven curved blade, etc.) can be equipped. Granted at level 1
            // with the selection, so it is present long before the blade is ever manifested.
            .AddComponent(new AddProficiencies { WeaponProficiencies = [cat] })
            .AddComponent(new MindBladeVisualComponent
            {
                SourceWeaponRef      = weaponRef,
                TargetWeaponTypeGuid = targetWeaponTypeGuid,
            })
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
