using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class ZealousFury
{
    public static void Configure()
    {
        var icon = AbilityRefs.HeroismGreater.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWZealousFuryBuff", Guids.PowerZealousFuryBuff)
            .SetDisplayName(Loc.Str("PW.ZealousFury.BuffName", "Zealous Fury"))
            .SetDescription(Loc.Str("PW.ZealousFury.BuffDesc",
                "You attack with reckless fury. Iterative attack penalties are reduced by 5."))
            .SetIcon(icon)
            .AddStatBonus(descriptor: ModifierDescriptor.Morale, stat: StatType.AdditionalAttackBonus, value: 5)
            .Configure();

        AbilityConfigurator.New("PWZealousFury", Guids.PowerZealousFury)
            .SetDisplayName(Loc.Str("PW.ZealousFury.Name", "Zealous Fury"))
            .SetDescription(Loc.Str("PW.ZealousFury.Desc",
                "Swift Action. Your psionic fury drives your blows. Iterative attack penalties are reduced by 5."))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1Round", "1 round"))
            .SetActionType(UnitCommand.CommandType.Swift)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[ZealousFury] applying +5 AdditionalAttackBonus 1r" })
                    .ApplyBuff(buff, ContextDuration.Fixed(1)))
            .AddSpellListComponent(4, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
