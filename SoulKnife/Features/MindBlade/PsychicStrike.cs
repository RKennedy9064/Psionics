using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.MindBlade;

public static class PsychicStrike
{
    public static void Configure()
    {
        var icon = AbilityRefs.TrueStrike.Reference.Get().Icon;

        // Persistent charge buff — removed on hit, NOT on attack roll (persists through misses)
        var chargeBuff = BuffConfigurator.New("SKPsychicStrikeChargeBuff", Guids.PsychicStrikeChargeBuff)
            .SetDisplayName(Loc.Str("SK.PsychicStrike.Charge.Name", "Psychic Strike"))
            .SetDescription(Loc.Str("SK.PsychicStrike.Charge.Desc",
                "Your mind blade is imbued with destructive psychic energy. The next time this weapon hits a target, it deals bonus force damage (1d8 per 4 soulknife levels). This charge is retained until used — it is not lost on a miss."))
            .SetIcon(icon)
            .Configure();

        // Move-action ability: charge the mind blade
        AbilityConfigurator.New("SKPsychicStrikeChargeAbility", Guids.PsychicStrikeChargeAbility)
            .SetDisplayName(Loc.Str("SK.PsychicStrike.Charge.AbName", "Psychic Strike"))
            .SetDescription(Loc.Str("SK.PsychicStrike.Charge.AbDesc",
                "Move Action. Imbue your mind blade with psychic energy. Your next hit deals bonus force damage (1d8 at 3rd level, +1d8 per 4 levels). The charge persists until a hit lands."))
            .SetIcon(icon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Move)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(chargeBuff, ContextDuration.Fixed(10000)))
            .Configure();

        // Swift-action recharge: expend psionic focus to re-arm a spent charge
        AbilityConfigurator.New("SKPsychicStrikeRechargeAbility", Guids.PsychicStrikeRechargeAbility)
            .SetDisplayName(Loc.Str("SK.PsychicStrike.Recharge.AbName", "Psychic Strike (Swift Recharge)"))
            .SetDescription(Loc.Str("SK.PsychicStrike.Recharge.AbDesc",
                "Swift Action. Expend psionic focus to instantly recharge your Psychic Strike."))
            .SetIcon(icon)
            .SetType(AbilityType.Extraordinary)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityCasterHasFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(Guids.PsionicFocusBuff)])
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .RemoveBuff(Guids.PsionicFocusBuff)
                    .ApplyBuff(chargeBuff, ContextDuration.Fixed(10000)))
            .Configure();

        // Feature granted at level 3: grants charge ability + damage component
        FeatureConfigurator.New("SKPsychicStrikeFeature", Guids.PsychicStrikeFeature)
            .SetDisplayName(Loc.Str("SK.PsychicStrike.Name", "Psychic Strike"))
            .SetDescription(Loc.Str("SK.PsychicStrike.Desc",
                "As a move action, a soulknife of 3rd level or higher can imbue her mind blade with destructive psychic energy. This attack deals an extra 1d8 points of force damage. The charge persists until the next successful hit — it is not lost on a miss. At 7th level and every four levels thereafter, the extra damage increases by 1d8. A soulknife can also recharge a spent strike as a swift action by expending psionic focus."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFacts([Guids.PsychicStrikeChargeAbility, Guids.PsychicStrikeRechargeAbility])
            .AddComponent(new PsychicStrikeDamage())
            .Configure();
    }
}
