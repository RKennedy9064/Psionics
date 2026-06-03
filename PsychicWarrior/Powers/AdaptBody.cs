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
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class AdaptBody
{
    public static void Configure()
    {
        var fireBuff = BuildBuff("PWAdaptBodyFireBuff", Guids.PowerAdaptBodyFireBuff,
            "Adapt Body (Fire)", DamageEnergyType.Fire,
            AbilityRefs.BurningHands.Reference.Get().Icon);

        var coldBuff = BuildBuff("PWAdaptBodyColdBuff", Guids.PowerAdaptBodyColdBuff,
            "Adapt Body (Cold)", DamageEnergyType.Cold,
            AbilityRefs.RayOfFrost.Reference.Get().Icon);

        var electricBuff = BuildBuff("PWAdaptBodyElectricBuff", Guids.PowerAdaptBodyElectricBuff,
            "Adapt Body (Electric)", DamageEnergyType.Electricity,
            AbilityRefs.CallLightning.Reference.Get().Icon);

        var acidBuff = BuildBuff("PWAdaptBodyAcidBuff", Guids.PowerAdaptBodyAcidBuff,
            "Adapt Body (Acid)", DamageEnergyType.Acid,
            AbilityRefs.AcidArrow.Reference.Get().Icon);

        BuildVariant("PWAdaptBodyFire", Guids.PowerAdaptBodyFire,
            "Adapt Body  -  Fire", "fire", fireBuff,
            AbilityRefs.BurningHands.Reference.Get().Icon);

        BuildVariant("PWAdaptBodyCold", Guids.PowerAdaptBodyCold,
            "Adapt Body  -  Cold", "cold", coldBuff,
            AbilityRefs.RayOfFrost.Reference.Get().Icon);

        BuildVariant("PWAdaptBodyElectric", Guids.PowerAdaptBodyElectric,
            "Adapt Body  -  Electric", "electricity", electricBuff,
            AbilityRefs.CallLightning.Reference.Get().Icon);

        BuildVariant("PWAdaptBodyAcid", Guids.PowerAdaptBodyAcid,
            "Adapt Body  -  Acid", "acid", acidBuff,
            AbilityRefs.AcidArrow.Reference.Get().Icon);

        AbilityConfigurator.New("PWAdaptBody", Guids.PowerAdaptBody)
            .SetDisplayName(Loc.Str("PW.AdaptBody.Name", "Adapt Body", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.AdaptBody.Desc",
                "Your body rapidly adapts to hostile environmental conditions. Choose an energy type (fire, cold, electricity, or acid). You gain resistance 30 to that energy type.",
                tagEncyclopediaEntries: false))
            .SetIcon(AbilityRefs.BurningHands.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.10MinPerML", "10 minutes per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(5, Guids.SpellList)
            .AddAbilityVariants(variants:
            [
                Guids.PowerAdaptBodyFire,
                Guids.PowerAdaptBodyCold,
                Guids.PowerAdaptBodyElectric,
                Guids.PowerAdaptBodyAcid,
            ])
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }

    private static BlueprintBuff BuildBuff(string name, string guid, string displayName,
        DamageEnergyType energyType, UnityEngine.Sprite icon)
    {
        return BuffConfigurator.New(name, guid)
            .SetDisplayName(Loc.Str($"PW.{name}.Name", displayName, tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str($"PW.{name}.Desc",
                $"You have resistance 30 to {energyType.ToString().ToLower()} damage.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddDamageResistanceEnergy(type: energyType, value: ContextValues.Constant(30))
            .Configure();
    }

    private static void BuildVariant(string name, string guid, string displayName,
        string energyName, BlueprintBuff buff, UnityEngine.Sprite icon)
    {
        AbilityConfigurator.New(name, guid)
            .SetDisplayName(Loc.Str($"PW.{name}.Name", displayName, tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str($"PW.{name}.Desc",
                $"Your body adapts to {energyName} energy. You gain resistance 30 to {energyName} damage.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.10MinPerML", "10 minutes per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(
                    buff,
                    ContextDuration.Variable(ContextValues.Rank(), DurationRate.TenMinutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
