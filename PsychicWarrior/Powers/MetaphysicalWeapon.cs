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

public static class MetaphysicalWeapon
{
    public static void Configure()
    {
        var buff = BuffConfigurator.New("PWMetaphysicalWeaponBuff", Guids.PowerMetaphysicalWeaponBuff)
            .SetDisplayName(Loc.Str("PW.MetaphysicalWeapon.BuffName", "Metaphysical Weapon"))
            .SetDescription(Loc.Str("PW.MetaphysicalWeapon.BuffDesc",
                "Your weapon is imbued with psychic energy, granting a +1 enhancement bonus to attack and damage rolls at ML 1, improving by 1 per 4 levels (max +5 at ML 17)."))
            .SetIcon(AbilityRefs.MagicWeapon.Reference.Get().Icon)
            .AddContextStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus, value: ContextValues.Rank())
            .AddContextStatBonus(descriptor: ModifierDescriptor.Enhancement, stat: Kingmaker.EntitySystem.Stats.StatType.AdditionalDamage, value: ContextValues.Rank())
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (4, 1), (8, 2), (12, 3), (16, 4), (20, 5)))
            .Configure();

        AbilityConfigurator.New("PWMetaphysicalWeapon", Guids.PowerMetaphysicalWeapon)
            .SetDisplayName(Loc.Str("PW.MetaphysicalWeapon.Name", "Metaphysical Weapon"))
            .SetDescription(Loc.Str("PW.MetaphysicalWeapon.Desc",
                "You imbue your weapon with psychic energy, granting a +1 enhancement bonus to attack rolls and damage rolls at ML 1, improving by 1 per 4 levels (max +5 at ML 17)."))
            .SetIcon(AbilityRefs.MagicWeapon.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(1, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[MetaphysicalWeapon] applying enh bonus (rank=ML; buff scales +1..+5 at ML 1/5/9/13/17)", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
