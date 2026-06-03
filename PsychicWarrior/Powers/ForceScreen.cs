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

public static class ForceScreen
{
    public static void Configure()
    {
        var buff = BuffConfigurator.New("PWForceScreenBuff", Guids.PowerForceScreenBuff)
            .SetDisplayName(Loc.Str("PW.ForceScreen.BuffName", "Force Screen"))
            .SetDescription(Loc.Str("PW.ForceScreen.BuffDesc",
                "An invisible disk of force hovers before you, granting a shield bonus to AC scaling with manifester level (+4 at ML 1, +1 per 2 levels)."))
            .SetIcon(AbilityRefs.MageShield.Reference.Get().Icon)
            .AddContextStatBonus(descriptor: ModifierDescriptor.Shield, stat: StatType.AC, value: ContextValues.Rank())
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (2, 4), (4, 5), (6, 6), (8, 7), (10, 8), (12, 9), (14, 10), (16, 11), (18, 12), (20, 13)))
            .Configure();

        AbilityConfigurator.New("PWForceScreen", Guids.PowerForceScreen)
            .SetDisplayName(Loc.Str("PW.ForceScreen.Name", "Force Screen"))
            .SetDescription(Loc.Str("PW.ForceScreen.Desc",
                "You create an invisible mobile disk of force that hovers in front of you. It grants a shield bonus to AC scaling with manifester level (+4 at ML 1, +1 per 2 levels)."))
            .SetIcon(AbilityRefs.MageShield.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(1, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[ForceScreen] applying shield (rank=ML; buff scales +4..+13 at ML 1..20)", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Abjuration)
            .Configure();
    }
}