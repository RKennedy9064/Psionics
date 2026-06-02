using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class InertialArmor
{
    public static void Configure()
    {
        var buff = BuffConfigurator.New("PWInertialArmorBuff", Guids.PowerInertialArmorBuff)
            .SetDisplayName(Loc.Str("PW.InertialArmor.BuffName", "Inertial Armor"))
            .SetDescription(Loc.Str("PW.InertialArmor.BuffDesc",
                "A psychokinetic field surrounds you, granting an armor bonus to AC scaling with manifester level (+4 at ML 1, +1 per 2 levels)."))
            .SetIcon(AbilityRefs.MageArmor.Reference.Get().Icon)
            .AddContextStatBonus(descriptor: ModifierDescriptor.Armor, stat: Kingmaker.EntitySystem.Stats.StatType.AC, value: ContextValues.Rank())
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (2, 4), (4, 5), (6, 6), (8, 7), (10, 8), (12, 9), (14, 10), (16, 11), (18, 12), (20, 13)))
            .Configure();

        AbilityConfigurator.New("PWInertialArmor", Guids.PowerInertialArmor)
            .SetDisplayName(Loc.Str("PW.InertialArmor.Name", "Inertial Armor"))
            .SetDescription(Loc.Str("PW.InertialArmor.Desc",
                "You create an invisible psychokinetic field around your body. You gain an armor bonus to AC scaling with manifester level (+4 at ML 1, +1 per 2 levels). Unlike mundane armor, inertial armor never results in armor check penalties or arcane spell failure."))
            .SetIcon(AbilityRefs.MageArmor.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1Hour", "1 hour"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(1, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[InertialArmor] applying armor (rank=ML; buff scales +4..+13 at ML 1..20)", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Hours)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Abjuration)
            .Configure();
    }
}
