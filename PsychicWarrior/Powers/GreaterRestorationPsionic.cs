using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

public static class MindBlankPersonalPsionic
{
    public static void Configure()
    {
        var icon = AbilityRefs.Heal.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWMindBlankPersonalBuff", Guids.PowerMindBlankPersonalBuff)
            .SetDisplayName(Loc.Str("PW.MindBlankPersonal.BuffName", "Mind Blank, Personal", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.MindBlankPersonal.BuffDesc",
                "You are immune to all mind-affecting spells and powers.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.MindAffecting)
            .Configure();

        AbilityConfigurator.New("PWMindBlankPersonal", Guids.PowerMindBlankPersonal)
            .SetDisplayName(Loc.Str("PW.MindBlankPersonal.Name", "Mind Blank, Personal (Psionic)", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.MindBlankPersonal.Desc",
                "You are protected from all spells and effects that detect, influence, or read emotions or thoughts. You become immune to all mind-affecting spells and powers.",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(
                    buff,
                    ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellListComponent(6, Guids.SpellList)
            .AddSpellComponent(SpellSchool.Abjuration)
            .Configure();
    }
}
