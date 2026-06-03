using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class MetaphysicalClaw
{
    public static void Configure()
    {
        var icon = AbilityRefs.MagicFang.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWMetaphysicalClawBuff", Guids.PowerMetaphysicalClawBuff)
            .SetDisplayName(Loc.Str("PW.MetaphysicalClaw.BuffName", "Metaphysical Claw"))
            .SetDescription(Loc.Str("PW.MetaphysicalClaw.BuffDesc",
                "Your natural weapons are imbued with psychic energy, granting a +1 enhancement bonus to attack and damage rolls."))
            .SetIcon(icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: StatType.AdditionalAttackBonus, value: 1)
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: StatType.AdditionalDamage, value: 1)
            .Configure();

        AbilityConfigurator.New("PWMetaphysicalClaw", Guids.PowerMetaphysicalClaw)
            .SetDisplayName(Loc.Str("PW.MetaphysicalClaw.Name", "Metaphysical Claw"))
            .SetDescription(Loc.Str("PW.MetaphysicalClaw.Desc",
                "You imbue your natural weapons with psychic energy, granting a +1 enhancement bonus to attack rolls and damage rolls."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[MetaphysicalClaw] applying +1 enh to natural weapons", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(1, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
