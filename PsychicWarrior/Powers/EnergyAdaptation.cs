using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class EnergyAdaptation
{
    public static void Configure()
    {
        BuildVariant("Fire", Guids.PowerEnergyAdaptationFire, Guids.PowerEnergyAdaptationFireBuff, DamageEnergyType.Fire, AbilityRefs.ResistFire.Reference.Get().Icon);
        BuildVariant("Cold", Guids.PowerEnergyAdaptationCold, Guids.PowerEnergyAdaptationColdBuff, DamageEnergyType.Cold, AbilityRefs.ResistCold.Reference.Get().Icon);
        BuildVariant("Electricity", Guids.PowerEnergyAdaptationElec, Guids.PowerEnergyAdaptationElecBuff, DamageEnergyType.Electricity, AbilityRefs.ResistElectricity.Reference.Get().Icon);
        BuildVariant("Acid", Guids.PowerEnergyAdaptationAcid, Guids.PowerEnergyAdaptationAcidBuff, DamageEnergyType.Acid, AbilityRefs.ResistAcid.Reference.Get().Icon);
        BuildVariant("Sonic", Guids.PowerEnergyAdaptationSonic, Guids.PowerEnergyAdaptationSonicBuff, DamageEnergyType.Sonic, AbilityRefs.ResistSonic.Reference.Get().Icon);

        AbilityConfigurator.New("PWEnergyAdaptation", Guids.PowerEnergyAdaptation)
            .SetDisplayName(Loc.Str("PW.EnergyAdaptation.Name", "Energy Adaptation"))
            .SetDescription(Loc.Str("PW.EnergyAdaptation.Desc",
                "You adapt your body to resist a chosen energy type. Choose fire, cold, electricity, acid, or sonic - you gain resistance 10 to that energy type."))
            .SetIcon(AbilityRefs.ResistFire.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.10MinPerML", "10 minutes per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(4, Guids.SpellList)
            .AddAbilityVariants(variants:
            [
                Guids.PowerEnergyAdaptationFire,
                Guids.PowerEnergyAdaptationCold,
                Guids.PowerEnergyAdaptationElec,
                Guids.PowerEnergyAdaptationAcid,
                Guids.PowerEnergyAdaptationSonic,
            ])
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }

    private static void BuildVariant(
        string energyName, string abilityGuid, string buffGuid,
        DamageEnergyType energy, UnityEngine.Sprite icon)
    {
        var buff = BuffConfigurator.New($"PWEnergyAdaptation{energyName}Buff", buffGuid)
            .SetDisplayName(Loc.Str($"PW.EnergyAdaptation{energyName}.BuffName", $"Energy Adaptation ({energyName})"))
            .SetDescription(Loc.Str($"PW.EnergyAdaptation{energyName}.BuffDesc",
                $"You have resistance 10 to {energyName.ToLower()} damage."))
            .SetIcon(icon)
            .AddComponent(new AddDamageResistanceEnergy { Type = energy, Value = 10 })
            .Configure();

        AbilityConfigurator.New($"PWEnergyAdaptation{energyName}", abilityGuid)
            .SetDisplayName(Loc.Str($"PW.EnergyAdaptation{energyName}.Name", $"Energy Adaptation: {energyName}"))
            .SetDescription(Loc.Str($"PW.EnergyAdaptation{energyName}.Desc",
                $"You gain resistance 10 to {energyName.ToLower()} damage."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.10MinPerML", "10 minutes per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = $"[EnergyAdaptation] applying resist 10 {energyName}", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.TenMinutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .Configure();
    }
}
