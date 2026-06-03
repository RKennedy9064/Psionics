using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class BodyOfIron
{
    public static void Configure()
    {
        var icon = AbilityRefs.Stoneskin.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWBodyOfIronBuff", Guids.PowerBodyOfIronBuff)
            .SetDisplayName(Loc.Str("PW.BodyOfIron.BuffName", "Body of Iron"))
            .SetDescription(Loc.Str("PW.BodyOfIron.BuffDesc",
                "Your body becomes iron. You gain DR 15/adamantine, +6 Strength, -6 Dexterity, immunity to critical hits, mind-affecting effects, poison, disease, and stun."))
            .SetIcon(icon)
            .AddComponent(new AddDamageResistancePhysical
            {
                Value = 15,
                BypassedByMaterial = true,
                Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite,
            })
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: StatType.Strength, value: 6)
            .AddStatBonus(descriptor: ModifierDescriptor.UntypedStackable, stat: StatType.Dexterity, value: -6)
            .AddComponent(new AddImmunityToCriticalHits())
            .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.MindAffecting)
            .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Poison)
            .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Disease)
            .AddConditionImmunity(condition: UnitCondition.Stunned)
            .Configure();

        AbilityConfigurator.New("PWBodyOfIron", Guids.PowerBodyOfIron)
            .SetDisplayName(Loc.Str("PW.BodyOfIron.Name", "Body of Iron"))
            .SetDescription(Loc.Str("PW.BodyOfIron.Desc",
                "Your body transforms into living iron. You gain DR 15/adamantine, +6 Strength, -6 Dexterity, immunity to critical hits, mind-affecting effects, poison, disease, and stun."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[BodyOfIron] applying DR 15/adamantine", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(6, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
