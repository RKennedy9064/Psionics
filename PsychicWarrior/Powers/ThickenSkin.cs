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

public static class ThickenSkin
{
    public static void Configure()
    {
        var buff = BuffConfigurator.New("PWThickenSkinBuff", Guids.PowerThickenSkinBuff)
            .SetDisplayName(Loc.Str("PW.ThickenSkin.BuffName", "Thicken Skin"))
            .SetDescription(Loc.Str("PW.ThickenSkin.BuffDesc",
                "Your skin thickens and toughens, granting a natural armor bonus to AC. The bonus increases by 1 for every 3 manifester levels (+1 at ML 1, +2 at ML 4, +3 at ML 7, +4 at ML 10, +5 at ML 13, +6 at ML 16, +7 at ML 19)."))
            .SetIcon(AbilityRefs.Barkskin.Reference.Get().Icon)
            .AddContextStatBonus(descriptor: ModifierDescriptor.NaturalArmor, stat: Kingmaker.EntitySystem.Stats.StatType.AC, value: ContextValues.Rank())
            .AddContextRankConfig(
                ContextRankConfigs.CasterLevel().WithCustomProgression(
                    (3, 1), (6, 2), (9, 3), (12, 4), (15, 5), (18, 6), (20, 7)))
            .Configure();

        AbilityConfigurator.New("PWThickenSkin", Guids.PowerThickenSkin)
            .SetDisplayName(Loc.Str("PW.ThickenSkin.Name", "Thicken Skin"))
            .SetDescription(Loc.Str("PW.ThickenSkin.Desc",
                "Your skin thickens and toughens, granting a natural armor bonus to AC. The bonus increases by 1 for every 3 manifester levels (+1 at ML 1, +2 at ML 4, +3 at ML 7, +4 at ML 10, +5 at ML 13, +6 at ML 16, +7 at ML 19)."))
            .SetIcon(AbilityRefs.Barkskin.Reference.Get().Icon)
            .SetType(AbilityType.Supernatural)
            .SetRange(AbilityRange.Personal)
            .SetLocalizedDuration(Loc.Str("PW.Duration.1Hour", "1 hour"))
            .SetActionType(UnitCommand.CommandType.Standard)
            .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
            .AddSpellListComponent(1, Guids.SpellList)
            .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                    .Add(new ContextActionLog { Message = "[ThickenSkin] applying natural armor (rank=ML; buff scales +1 to +7 at ML 1/4/7/10/13/16/19)", LogRank = true })
                    .ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Hours)))
            .AddContextRankConfig(ContextRankConfigs.CasterLevel())
            .AddSpellComponent(SpellSchool.Transmutation)
            .Configure();
    }
}
