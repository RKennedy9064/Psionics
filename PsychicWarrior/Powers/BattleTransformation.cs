using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class BattleTransformation
{
    public static void Configure()
    {
        var icon = AbilityRefs.Rage.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWBattleTransformationBuff", Guids.PowerBattleTransformationBuff)
            .SetDisplayName(Loc.Str("PW.BattleTransformation.BuffName", "Battle Transformation"))
            .SetDescription(Loc.Str("PW.BattleTransformation.BuffDesc",
                "Your body is transformed for battle. You gain +2 luck bonus to attack rolls, damage rolls, and Strength, plus temporary hit points equal to your manifester level."))
            .SetIcon(icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Luck, stat: StatType.AdditionalAttackBonus, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.Luck, stat: StatType.AdditionalDamage, value: 2)
            .AddStatBonus(descriptor: ModifierDescriptor.Luck, stat: StatType.Strength, value: 2)
            .AddTemporaryHitPointsFromAbilityValue(
                descriptor: ModifierDescriptor.UntypedStackable,
                value: ContextValues.Rank())
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .Configure();

        AbilityConfigurator.New("PWBattleTransformation", Guids.PowerBattleTransformation)
            .SetDisplayName(Loc.Str("PW.BattleTransformation.Name", "Battle Transformation"))
            .SetDescription(Loc.Str("PW.BattleTransformation.Desc",
                "You psionically transform your body for combat. You gain +2 luck bonus to attack rolls, damage rolls, and Strength, plus temporary hit points equal to your manifester level."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[BattleTransformation] applying buff", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(4, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
