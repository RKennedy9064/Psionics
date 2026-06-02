using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.Shared.Mechanics;

public static class Focus
{
    public static void Configure()
    {
        var focusIcon = AbilityRefs.RestorationLesser.Reference.Get().Icon;

        var focusBuff = BuffConfigurator.New("PsionicFocusBuff", Guids.PsionicFocusBuff)
            .SetDisplayName(Loc.Str("PW.Focus.Name", "Psionic Focus"))
            .SetDescription(Loc.Str("PW.Focus.Desc", "You are psionically focused. You can expend this focus to activate certain powers and maneuvers."))
            .SetIcon(focusIcon)
            .SetFxOnStart(BuffRefs.ArcaneAccuracyBuff.Reference.Get().FxOnStart)
            // When focus is lost, all path trance buffs end (RAW: trance is maintained by focus)
            .AddBuffActions(deactivated: ActionsBuilder.New()
                .RemoveBuff(Guids.WeaponmasterTranceBuff)
                .RemoveBuff(Guids.BrawlerTranceBuff)
                .RemoveBuff(Guids.ArcherTranceBuff)
                .RemoveBuff(Guids.AsceticTranceBuff)
                .RemoveBuff(Guids.AssassinsTranceBuff)
                .RemoveBuff(Guids.DervishTranceBuff)
                .RemoveBuff(Guids.FeralWarriorTranceBuff)
                .RemoveBuff(Guids.GladiatorTranceBuff)
                .RemoveBuff(Guids.InfiltratorTranceBuff)
                .RemoveBuff(Guids.InterceptorTranceBuff)
                .RemoveBuff(Guids.MindKnightTranceBuff)
                .RemoveBuff(Guids.SurvivorTranceBuff))
            .Configure();

        var gainFocusAbility = AbilityConfigurator.New("GainPsionicFocusAbility", Guids.GainPsionicFocusAbility)
            .SetDisplayName(Loc.Str("PW.GainFocusAbility.Name", "Gain Psionic Focus"))
            .SetDescription(Loc.Str("PW.GainFocusAbility.Desc", "Standard Action. Meditate to gain psionic focus."))
            .SetIcon(focusIcon)
            // Supernatural is the mechanically perfect fit: No AoO, No Spell Resistance, infinite uses.
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffPermanent(focusBuff))
            // Hidden once Psionic Meditation feat is taken (the move-action version supersedes this)
            .AddAbilityShowIfCasterHasFact(not: true, unitFact: Guids.PsionicMeditationFeat)
            .Configure();

        AbilityConfigurator.New("GainPsionicFocusMoveAbility", Guids.GainPsionicFocusMoveAbility)
            .SetDisplayName(Loc.Str("PW.GainFocusMoveAbility.Name", "Gain Psionic Focus (Move Action)"))
            .SetDescription(Loc.Str("PW.GainFocusMoveAbility.Desc", "Move Action. Meditate quickly to gain psionic focus."))
            .SetIcon(focusIcon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Move)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffPermanent(focusBuff))
            .Configure();

        FeatureConfigurator.New("GainPsionicFocusFeature", Guids.GainPsionicFocusFeature)
            .SetDisplayName(Loc.Str("PW.GainFocusFeature.Name", "Psionic Focus"))
            .SetDescription(Loc.Str("PW.GainFocusFeature.Desc", "A psychic warrior can meditate to gain psionic focus. While focused, they can trigger special maneuvers."))
            .SetIsClassFeature()
            .SetIcon(focusIcon)
            .AddFacts([gainFocusAbility.ToString()])
            .Configure();
    }
}