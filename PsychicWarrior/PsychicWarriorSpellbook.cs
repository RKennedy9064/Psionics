using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Psionics.Utils;

namespace Psionics.PsychicWarrior;

/// <summary>
/// Psychic Warrior spellbook configuration.
/// 
/// Power Points Per Day Table:
/// Lvl  | 1st | 2nd | 3rd | 4th | 5th | 6th | Total
/// -----|-----|-----|-----|-----|-----|-----|-------
/// 1    | 1   | -   | -   | -   | -   | -   | 1
/// 2    | 2   | -   | -   | -   | -   | -   | 2
/// 3    | 4   | -   | -   | -   | -   | -   | 4
/// 4    | 6   | 3   | -   | -   | -   | -   | 9
/// 5    | 8   | 5   | -   | -   | -   | -   | 13
/// 6    | 12  | 7   | 3   | -   | -   | -   | 22
/// 7    | 16  | 10  | 5   | -   | -   | -   | 31
/// 8    | 20  | 13  | 7   | 3   | -   | -   | 43
/// ...and so on through level 20 (20 powers known, up to 6th level powers, 128 power points/day)
/// 
/// In WOTR, we approximate this using spell slots. The Inquisitor progression is a reasonable match.
/// </summary>
public static class PsychicWarriorSpellbook
{
    public static void Configure()
    {
        var spellList = SpellListConfigurator.New("PsychicWarriorSpellList", Guids.SpellList)
            .OnConfigure(bp =>
            {
                bp.SpellsByLevel = new SpellLevelList[7];
                for (int i = 0; i < 7; i++)
                {
                    bp.SpellsByLevel[i] = new SpellLevelList(i);
                }

                // 1st-level powers
                bp.SpellsByLevel[1].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerMetaphysicalClaw));
                bp.SpellsByLevel[1].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerExpansion));
                bp.SpellsByLevel[1].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerCompression));
                bp.SpellsByLevel[1].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerVigor));
                bp.SpellsByLevel[1].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerForceScreen));
                bp.SpellsByLevel[1].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerInertialArmor));
                bp.SpellsByLevel[1].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerThickenSkin));
                bp.SpellsByLevel[1].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerBiofeedback));
                bp.SpellsByLevel[1].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerMetaphysicalWeapon));

                // 2nd-level powers
                bp.SpellsByLevel[2].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerPsionicLionsCharge));
                bp.SpellsByLevel[2].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerConcealingAmorpha));
                bp.SpellsByLevel[2].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerBodyAdjustment));
                bp.SpellsByLevel[2].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerBodyPurification));
                bp.SpellsByLevel[2].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerStrengthOfMyEnemy));
                bp.SpellsByLevel[2].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerAnimalAffinity));
                bp.SpellsByLevel[2].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerDetectHostileIntent));
                bp.SpellsByLevel[2].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerHustle));

                // 5th-level powers
                bp.SpellsByLevel[5].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerTrueMetabolism));
                bp.SpellsByLevel[5].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerAdaptBody));
                bp.SpellsByLevel[5].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerTrueSeeing));

                // 6th-level powers
                bp.SpellsByLevel[6].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerBodyOfIron));
                bp.SpellsByLevel[6].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerDisintegratePsionic));
                bp.SpellsByLevel[6].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerMindBlankPersonal));
                bp.SpellsByLevel[6].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerOakBody));

                // 4th-level powers
                bp.SpellsByLevel[4].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerEnergyAdaptation));
                bp.SpellsByLevel[4].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerBattleTransformation));
                bp.SpellsByLevel[4].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerInertialBarrier));
                bp.SpellsByLevel[4].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerZealousFury));
                bp.SpellsByLevel[4].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerDimensionDoor));
                bp.SpellsByLevel[4].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerFreedomOfMovement));
                bp.SpellsByLevel[4].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerWeaponOfEnergy));
                bp.SpellsByLevel[4].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerSteadfastPerception));

                // 3rd-level powers
                bp.SpellsByLevel[3].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerEvadeBurst));
                bp.SpellsByLevel[3].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerUbiquitousVision));
                bp.SpellsByLevel[3].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerPhysicalAcceleration));
                bp.SpellsByLevel[3].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerDimensionSlide));
                bp.SpellsByLevel[3].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerVampiricBlade));
                bp.SpellsByLevel[3].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerMentalBarrier));
                bp.SpellsByLevel[3].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerConcealingAmorphaGreater));
                bp.SpellsByLevel[3].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerGraftWeapon));
                bp.SpellsByLevel[3].m_Spells.Add(BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.PowerKeenEdgePsionic));
            })
            .Configure();

        SpellbookConfigurator.New("PsychicWarriorSpellbook", Guids.Spellbook)
            .SetName(Loc.Str("PW.Spellbook.Name", "Psychic Warrior Powers"))
            .SetCharacterClass(Guids.PsychicWarriorClass)
            .SetSpellsPerDay(SpellsTableRefs.InquisitorSpellSlotsTable.ToString())
            .SetSpellsKnown(SpellsTableRefs.InquisitorSpellsKnownTable.ToString())
            .SetSpellList(spellList)
            .SetCastingAttribute(StatType.Wisdom)
            .SetSpontaneous(true)
            .SetIsArcane(false)
            .SetAllSpellsKnown(false)
            .SetCantripsType(CantripsType.Cantrips)
            .Configure();
    }
}