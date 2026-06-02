using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.BladeSkills;

/// <summary>
/// Blade skills: Fire / Ice / Lightning / Thunder Blade. While the soulknife has the skill, each
/// successful melee hit with her mind blade deals an extra die of the matching energy type.
/// (Adaptation: the tabletop "expend psionic focus on hit for a secondary rider" is omitted; the
/// base energy damage is applied passively. Secondary riders — Firestorm/Freezing Ice/etc. — are
/// tracked as a later batch.)
/// </summary>
public static class EnergyBlades
{
    public static void Configure()
    {
        Build("SKFireBlade", Guids.BladeSkillFireBlade, "Fire Blade", DamageEnergyType.Fire,
            AbilityRefs.BurningHands.Reference.Get().Icon);
        Build("SKIceBlade", Guids.BladeSkillIceBlade, "Ice Blade", DamageEnergyType.Cold,
            AbilityRefs.RayOfFrost.Reference.Get().Icon);
        Build("SKLightningBlade", Guids.BladeSkillLightningBlade, "Lightning Blade", DamageEnergyType.Electricity,
            AbilityRefs.CallLightning.Reference.Get().Icon);
        Build("SKThunderBlade", Guids.BladeSkillThunderBlade, "Thunder Blade", DamageEnergyType.Sonic,
            AbilityRefs.ResistSonic.Reference.Get().Icon);

        // Secondary riders — each requires the matching energy blade and adds a further +1d10 of
        // that energy on a mind blade hit. (Adaptation of the tabletop delayed-AoE riders, which
        // require positional/end-of-turn tracking WotR can't express cleanly.)
        BuildRider("SKFirestorm", Guids.BladeSkillFirestorm, Guids.BladeSkillFireBlade, "Firestorm",
            DamageEnergyType.Fire, AbilityRefs.BurningHands.Reference.Get().Icon);
        BuildRider("SKFreezingIce", Guids.BladeSkillFreezingIce, Guids.BladeSkillIceBlade, "Freezing Ice",
            DamageEnergyType.Cold, AbilityRefs.RayOfFrost.Reference.Get().Icon);
        BuildRider("SKLightningArc", Guids.BladeSkillLightningArc, Guids.BladeSkillLightningBlade, "Lightning Arc",
            DamageEnergyType.Electricity, AbilityRefs.CallLightning.Reference.Get().Icon);
        BuildRider("SKResoundingThunder", Guids.BladeSkillResoundingThunder, Guids.BladeSkillThunderBlade, "Resounding Thunder",
            DamageEnergyType.Sonic, AbilityRefs.ResistSonic.Reference.Get().Icon);
    }

    private static void BuildRider(string name, string guid, string requiredEnergyBlade, string displayName,
        DamageEnergyType energyType, UnityEngine.Sprite icon)
    {
        var energyName = energyType.ToString().ToLower();
        FeatureConfigurator.New(name, guid)
            .SetDisplayName(Loc.Str($"SK.{name}.Name", displayName))
            .SetDescription(Loc.Str($"SK.{name}.Desc",
                $"Your {energyName} mind blade burns hotter: each successful melee hit deals an additional 1d10 {energyName} damage."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddPrerequisiteFeature(requiredEnergyBlade)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().DealDamage(
                    damageType: DamageTypes.Energy(energyType),
                    value: ContextDice.Value(DiceType.D10, 1)),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Melee)
            .Configure();
    }

    private static void Build(string name, string guid, string displayName,
        DamageEnergyType energyType, UnityEngine.Sprite icon)
    {
        var energyName = energyType.ToString().ToLower();
        FeatureConfigurator.New(name, guid)
            .SetDisplayName(Loc.Str($"SK.{name}.Name", displayName))
            .SetDescription(Loc.Str($"SK.{name}.Desc",
                $"Your mind blade is wreathed in {energyName} energy. Each successful melee hit with your mind blade deals an extra 1d6 {energyName} damage."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 8)
            .AddInitiatorAttackWithWeaponTrigger(
                action: ActionsBuilder.New().DealDamage(
                    damageType: DamageTypes.Energy(energyType),
                    value: ContextDice.Value(DiceType.D6, 1)),
                onlyHit: true,
                checkWeaponRangeType: true,
                rangeType: WeaponRangeType.Melee)
            .Configure();
    }
}
