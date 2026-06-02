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

public static class PrecognitionDefensive
{
    public static void Configure()
    {
        var buff = BuffConfigurator.New("PWPrecognitionDefensiveBuff", Guids.PowerPrecognitionDefensiveBuff)
            .SetDisplayName(Loc.Str("PW.PrecognitionDef.BuffName", "Precognition, Defensive"))
            .SetDescription(Loc.Str("PW.PrecognitionDef.BuffDesc", "+1 insight bonus to AC."))
            .SetIcon(AbilityRefs.MirrorImage.Reference.Get().Icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Insight, stat: StatType.AC, value: 1)
            .Configure();

        AbilityConfigurator.New("PWPrecognitionDefensive", Guids.PowerPrecognitionDefensive)
            .SetDisplayName(Loc.Str("PW.PrecognitionDef.Name", "Precognition, Defensive"))
            .SetDescription(Loc.Str("PW.PrecognitionDef.Desc",
                "Your psionic foresight warns you of incoming attacks. You gain a +1 insight bonus to AC."))
            .SetIcon(AbilityRefs.MirrorImage.Reference.Get().Icon)
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
