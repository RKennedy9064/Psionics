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
/// Mental Barrier (Abjuration)  -  Project a barrier of mental energy. +4 deflection bonus to AC
///.
/// </summary>
public static class MentalBarrier
{
    public static void Configure()
    {
        var icon = AbilityRefs.ShieldOfFaith.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWMentalBarrierBuff", Guids.PowerMentalBarrierBuff)
            .SetDisplayName(Loc.Str("PW.MentalBarrier.BuffName", "Mental Barrier", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.MentalBarrier.BuffDesc",
                "A barrier of mental force surrounds you, granting a deflection bonus to AC. +4 at ML 5, improving by 1 per 2 manifester levels.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddContextStatBonus(descriptor: ModifierDescriptor.Deflection, stat: StatType.AC, value: ContextValues.Rank())
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (6, 4), (8, 5), (10, 6), (12, 7), (14, 8), (16, 9), (18, 10), (20, 11)))
            .Configure();

        AbilityConfigurator.New("PWMentalBarrier", Guids.PowerMentalBarrier)
            .SetDisplayName(Loc.Str("PW.MentalBarrier.Name", "Mental Barrier", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.MentalBarrier.Desc",
                "You raise a barrier of pure mental energy, gaining a +4 deflection bonus to AC at ML 5, improving by 1 per 2 manifester levels.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1RoundPerML", "1 round per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(3, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[MentalBarrier] applying deflection (rank=ML; buff scales +4..+11 at ML 5..20)", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Rounds)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Abjuration)
            .Configure();
    }
}
