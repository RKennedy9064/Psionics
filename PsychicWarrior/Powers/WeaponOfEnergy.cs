using System.Collections.Generic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class WeaponOfEnergy
{
    public static void Configure()
    {
        var fireBuff = BuildBuff("PWWeaponOfEnergyFireBuff", Guids.PowerWeaponOfEnergyFireBuff,
            "Weapon of Energy (Fire)", DamageEnergyType.Fire,
            AbilityRefs.BurningHands.Reference.Get().Icon);

        var coldBuff = BuildBuff("PWWeaponOfEnergyColdfBuff", Guids.PowerWeaponOfEnergyColdBuff,
            "Weapon of Energy (Cold)", DamageEnergyType.Cold,
            AbilityRefs.RayOfFrost.Reference.Get().Icon);

        var electricBuff = BuildBuff("PWWeaponOfEnergyElectricBuff", Guids.PowerWeaponOfEnergyElectricBuff,
            "Weapon of Energy (Electric)", DamageEnergyType.Electricity,
            AbilityRefs.CallLightning.Reference.Get().Icon);

        var acidBuff = BuildBuff("PWWeaponOfEnergyAcidBuff", Guids.PowerWeaponOfEnergyAcidBuff,
            "Weapon of Energy (Acid)", DamageEnergyType.Acid,
            AbilityRefs.AcidArrow.Reference.Get().Icon);

        var fireVariant = BuildVariant("PWWeaponOfEnergyFire", Guids.PowerWeaponOfEnergyFire,
            "Weapon of Energy — Fire", "fire", fireBuff,
            AbilityRefs.BurningHands.Reference.Get().Icon);

        var coldVariant = BuildVariant("PWWeaponOfEnergyCold", Guids.PowerWeaponOfEnergyCold,
            "Weapon of Energy — Cold", "cold", coldBuff,
            AbilityRefs.RayOfFrost.Reference.Get().Icon);

        var electricVariant = BuildVariant("PWWeaponOfEnergyElectric", Guids.PowerWeaponOfEnergyElectric,
            "Weapon of Energy — Electric", "electricity", electricBuff,
            AbilityRefs.CallLightning.Reference.Get().Icon);

        var acidVariant = BuildVariant("PWWeaponOfEnergyAcid", Guids.PowerWeaponOfEnergyAcid,
            "Weapon of Energy — Acid", "acid", acidBuff,
            AbilityRefs.AcidArrow.Reference.Get().Icon);

        AbilityConfigurator.New("PWWeaponOfEnergy", Guids.PowerWeaponOfEnergy)
            .SetDisplayName(Loc.Str("PW.WeaponOfEnergy.Name", "Weapon of Energy", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.WeaponOfEnergy.Desc",
                "Your weapon crackles with psionic energy. Choose an energy type (fire, cold, electricity, or acid). For 1 round per manifester level, each successful melee attack deals an extra 2d6 points of the chosen energy type.",
                tagEncyclopediaEntries: false))
            .SetIcon(AbilityRefs.BurningHands.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(4, Guids.SpellList)
            .AddAbilityVariants(variants:
            [
                Guids.PowerWeaponOfEnergyFire,
                Guids.PowerWeaponOfEnergyCold,
                Guids.PowerWeaponOfEnergyElectric,
                Guids.PowerWeaponOfEnergyAcid,
            ])
            .AddSpellComponent(SpellSchool.Evocation)
            .Configure();
    }

    private static BlueprintBuff BuildBuff(string name, string guid, string displayName,
        DamageEnergyType energyType, UnityEngine.Sprite icon)
    {
        return BuffConfigurator.New(name, guid)
            .SetDisplayName(Loc.Str($"PW.{name}.Name", displayName, tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str($"PW.{name}.Desc",
                $"Each successful melee attack deals an extra 2d6 {energyType.ToString().ToLower()} damage.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().DealDamage(
                    damageType: DamageTypes.Energy(energyType),
                    value: ContextDice.Value(DiceType.D6, 2)),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Melee)
            .Configure();
    }

    private static BlueprintAbility BuildVariant(string name, string guid, string displayName,
        string energyName, BlueprintBuff buff, UnityEngine.Sprite icon)
    {
        return AbilityConfigurator.New(name, guid)
            .SetDisplayName(Loc.Str($"PW.{name}.Name", displayName, tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str($"PW.{name}.Desc",
                $"Imbue your weapon with {energyName} energy. For 1 round per manifester level, each melee hit deals +2d6 {energyName} damage.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(
                    buff,
                    ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Evocation)
            .Configure();
    }
}
