using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class PsionicLionsCharge
{
    public static void Configure()
    {
        var icon = AbilityRefs.ChargeAbility.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWPsionicLionsChargeBuff", Guids.PowerPsionicLionsChargeBuff)
            .SetDisplayName(Loc.Str("PW.PsionicLionsCharge.BuffName", "Psionic Lion's Charge"))
            .SetDescription(Loc.Str("PW.PsionicLionsCharge.BuffDesc",
                "You may make a full attack at the end of your charge this round."))
            .SetIcon(icon)
            .AddMechanicsFeature(AddMechanicsFeature.MechanicsFeatureType.Pounce)
            .Configure();

        AbilityConfigurator.New("PWPsionicLionsCharge", Guids.PowerPsionicLionsCharge)
            .SetDisplayName(Loc.Str("PW.PsionicLionsCharge.Name", "Psionic Lion's Charge"))
            .SetDescription(Loc.Str("PW.PsionicLionsCharge.Desc",
                "Swift Action. You psionically prepare your body for a devastating charge. You may make a full attack at the end of your charge this round."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1Round", "1 round"))
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[PsionicLionsCharge] applying Pounce 1r" })
                    .ApplyBuff(buff, ContextDuration.Fixed(1)))
            .AddSpellListComponent(2, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
