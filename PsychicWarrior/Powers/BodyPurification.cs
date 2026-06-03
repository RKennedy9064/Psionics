using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Powers;

/// <summary>
/// Body Purification (Conjuration) — Restore physical ability damage to yourself
/// (1d4 + caster level, max +5).
/// </summary>
public static class BodyPurification
{
    public static void Configure()
    {
        AbilityConfigurator.New("PWBodyPurification", Guids.PowerBodyPurification)
            .SetDisplayName(Loc.Str("PW.BodyPurification.Name", "Body Purification", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.BodyPurification.Desc",
                "You cleanse your body of toxins and impurities, restoring 1d4 + your manifester level (max +5) points of physical ability damage.",
                tagEncyclopediaEntries: false))
            .SetIcon(AbilityRefs.Restoration.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(2, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New().HealStatDamage(
                    healType: ContextActionHealStatDamage.StatDamageHealType.Dice,
                    statClass: ContextActionHealStatDamage.StatClass.Physical,
                    value: ContextDice.Value(DiceType.D4, 1, ContextValues.Rank())))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel(max: 5))
            .AddSpellComponent(SpellSchool.Conjuration)
            .Configure();
    }
}
