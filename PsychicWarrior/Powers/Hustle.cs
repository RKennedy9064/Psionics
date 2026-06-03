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
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

/// <summary>
/// Hustle (Transmutation)  -  Gain a haste-like boost.
///
/// Simplified from RAW (which grants an extra move action this round). WoTR doesn't model
/// per-round action economy the same way, so we apply a Haste-style buff.
/// </summary>
public static class Hustle
{
    public static void Configure()
    {
        var icon = AbilityRefs.Longstrider.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWHustleBuff", Guids.PowerHustleBuff)
            .SetDisplayName(Loc.Str("PW.Hustle.BuffName", "Hustle", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.Hustle.BuffDesc",
                "+30 ft. speed, +1 dodge bonus to AC, +1 dodge bonus to Reflex saves, extra attack at highest BAB on full attack.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.AC, value: 1)
            .AddStatBonus(descriptor: ModifierDescriptor.Dodge, stat: StatType.SaveReflex, value: 1)
            .AddBuffMovementSpeed(descriptor: ModifierDescriptor.Enhancement, value: 30)
            .AddBuffExtraAttack(haste: true, number: 1)
            .Configure();

        AbilityConfigurator.New("PWHustle", Guids.PowerHustle)
            .SetDisplayName(Loc.Str("PW.Hustle.Name", "Hustle", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.Hustle.Desc",
                "You psionically accelerate yourself. You gain the benefits of haste: +30 ft. speed, +1 dodge bonus to AC and Reflex saves, and an extra attack at your highest BAB on a full attack.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(2, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[Hustle] applying haste (1 round: +30ft speed, +1 dodge AC, +1 Reflex, extra attack)" })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
