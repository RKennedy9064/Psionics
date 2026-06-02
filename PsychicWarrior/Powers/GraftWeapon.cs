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
/// Graft Weapon (Psychometabolism â†' Transmutation)  -  Your hand and weapon become one psionic
/// instrument. RAW makes you immune to being disarmed; since disarm isn't a meaningful mechanic in
/// WoTR, the unified hand-weapon precision instead grants a scaling enhancement bonus to attack
/// and damage rolls (+2 at CL 8, +3 at 12, +4 at 16, +5 at 20; floor +2, max +5; formula CL/4).
/// Duration: 1 minute per manifester level.
/// </summary>
public static class GraftWeapon
{
    public static void Configure()
    {
        var icon = AbilityRefs.AlignWeapon.Reference.Get().Icon;

        var buff = BuffConfigurator.New("PWGraftWeaponBuff", Guids.PowerGraftWeaponBuff)
            .SetDisplayName(Loc.Str("PW.GraftWeapon.BuffName", "Graft Weapon", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.GraftWeapon.BuffDesc",
                "Your weapon is melded into your hand. You wield it with supernatural precision, gaining an enhancement bonus to attack and damage that scales with manifester level (+2 to +5).",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .AddContextStatBonus(
                stat: StatType.AdditionalAttackBonus,
                descriptor: ModifierDescriptor.Enhancement,
                value: ContextValues.Rank())
            .AddContextStatBonus(
                stat: StatType.AdditionalDamage,
                descriptor: ModifierDescriptor.Enhancement,
                value: ContextValues.Rank())
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel(min: 2, max: 5).WithDivStepProgression(4))
            .Configure();

        AbilityConfigurator.New("PWGraftWeapon", Guids.PowerGraftWeapon)
            .SetDisplayName(Loc.Str("PW.GraftWeapon.Name", "Graft Weapon", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.GraftWeapon.Desc",
                "You meld your hand and weapon together into a unified psionic instrument. You gain an enhancement bonus to attack and damage rolls (+2, increasing by +1 per 4 manifester levels to a maximum of +5).",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1MinPerML", "1 minute per manifester level"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(3, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().ApplyBuff(
                    buff,
                    ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
