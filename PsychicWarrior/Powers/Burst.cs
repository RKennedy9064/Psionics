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
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class Burst
{
    public static void Configure()
    {
        var buff = BuffConfigurator.New("PWBurstBuff", Guids.PowerBurstBuff)
            .SetDisplayName(Loc.Str("PW.Burst.BuffName", "Burst"))
            .SetDescription(Loc.Str("PW.Burst.BuffDesc", "Movement speed is increased by 10 feet."))
            .SetIcon(AbilityRefs.ExpeditiousRetreat.Reference.Get().Icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: StatType.Speed, value: 10)
            .Configure();

        AbilityConfigurator.New("PWBurst", Guids.PowerBurst)
            .SetDisplayName(Loc.Str("PW.Burst.Name", "Burst"))
            .SetDescription(Loc.Str("PW.Burst.Desc",
                "You gain a +10-foot enhancement bonus to your speed."))
            .SetIcon(AbilityRefs.ExpeditiousRetreat.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1Round", "1 round"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)))
            .Configure();
    }
}
