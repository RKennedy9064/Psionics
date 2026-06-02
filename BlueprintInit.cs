using System.Linq;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Psionics.PsychicWarrior;

namespace Psionics;

[HarmonyPatch(typeof(BlueprintsCache), "Init")]
public static class BlueprintInit
{
    private static bool Initialized;

    [HarmonyPostfix]
    public static void Postfix()
    {
        if (Initialized) return;
        Initialized = true;

        var logger = LogWrapper.Get("Psionics");

        // Each Configure() call is wrapped so a single failure is logged without
        // aborting the rest of initialization or the class registration.
        static void Run(string name, System.Action action, LogWrapper log)
        {
            try { action(); }
            catch (System.Exception e)
            {
                log.Error($"[Psionics] {name} failed: {e}");
                UnityEngine.Debug.LogError($"[Psionics] {name} failed: {e}");
            }
        }

        // Keep Debug.LogError on Configure failures (above) — silent failures
        // led to a hard-to-find GUID collision; surfacing them to Player.log
        // is cheap insurance for future blueprint additions.

        // ── Phase 1: Foundation ────────────────────────────────────────────────
        Run(nameof(Shared.Mechanics.Focus), Shared.Mechanics.Focus.Configure, logger);
        Run(nameof(PsychicWarrior.Features.PsionicProficiency), PsychicWarrior.Features.PsionicProficiency.Configure, logger);
        Run(nameof(PsychicWarrior.Features.MartialPower), PsychicWarrior.Features.MartialPower.Configure, logger);

        // ── Phase 2: 0-level talent abilities (must exist before TalentsSelection) ──
        Run(nameof(PsychicWarrior.Powers.MinorPrecognition), PsychicWarrior.Powers.MinorPrecognition.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.Burst), PsychicWarrior.Powers.Burst.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.EmptyMind), PsychicWarrior.Powers.EmptyMind.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.Valor), PsychicWarrior.Powers.Valor.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.TelekineticPunch), PsychicWarrior.Powers.TelekineticPunch.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.PrecognitionDefensive), PsychicWarrior.Powers.PrecognitionDefensive.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.Deceleration), PsychicWarrior.Powers.Deceleration.Configure, logger);

        // Talent selection references the 0-level power GUIDs above
        Run(nameof(PsychicWarrior.Features.TalentsSelection), PsychicWarrior.Features.TalentsSelection.Configure, logger);

        // ── Phase 3: 1st-level powers ──────────────────────────────────────────
        Run(nameof(PsychicWarrior.Powers.Expansion), PsychicWarrior.Powers.Expansion.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.Compression), PsychicWarrior.Powers.Compression.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.MetaphysicalClaw), PsychicWarrior.Powers.MetaphysicalClaw.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.Vigor), PsychicWarrior.Powers.Vigor.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.ForceScreen), PsychicWarrior.Powers.ForceScreen.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.InertialArmor), PsychicWarrior.Powers.InertialArmor.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.ThickenSkin), PsychicWarrior.Powers.ThickenSkin.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.Biofeedback), PsychicWarrior.Powers.Biofeedback.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.MetaphysicalWeapon), PsychicWarrior.Powers.MetaphysicalWeapon.Configure, logger);

        // ── Phase 3b: 2nd-level powers ─────────────────────────────────────────
        Run(nameof(PsychicWarrior.Powers.PsionicLionsCharge), PsychicWarrior.Powers.PsionicLionsCharge.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.ConcealingAmorpha), PsychicWarrior.Powers.ConcealingAmorpha.Configure, logger);

        // ── Phase 8: 2nd-level powers ──────────────────────────────────────────
        Run(nameof(PsychicWarrior.Powers.BodyAdjustment), PsychicWarrior.Powers.BodyAdjustment.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.BodyPurification), PsychicWarrior.Powers.BodyPurification.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.StrengthOfMyEnemy), PsychicWarrior.Powers.StrengthOfMyEnemy.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.AnimalAffinity), PsychicWarrior.Powers.AnimalAffinity.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.DetectHostileIntent), PsychicWarrior.Powers.DetectHostileIntent.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.Hustle), PsychicWarrior.Powers.Hustle.Configure, logger);

        // ── Phase 14: 5th-level powers ─────────────────────────────────────────
        Run(nameof(PsychicWarrior.Powers.TrueMetabolism), PsychicWarrior.Powers.TrueMetabolism.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.AdaptBody), PsychicWarrior.Powers.AdaptBody.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.TrueSeeing), PsychicWarrior.Powers.TrueSeeing.Configure, logger);

        // ── Phase 14: 6th-level powers ─────────────────────────────────────────
        Run(nameof(PsychicWarrior.Powers.BodyOfIron), PsychicWarrior.Powers.BodyOfIron.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.DisintegratePsionic), PsychicWarrior.Powers.DisintegratePsionic.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.MindBlankPersonalPsionic), PsychicWarrior.Powers.MindBlankPersonalPsionic.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.OakBody), PsychicWarrior.Powers.OakBody.Configure, logger);

        // ── Phase 14: 4th-level powers ─────────────────────────────────────────
        Run(nameof(PsychicWarrior.Powers.DimensionDoor), PsychicWarrior.Powers.DimensionDoor.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.FreedomOfMovement), PsychicWarrior.Powers.FreedomOfMovement.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.WeaponOfEnergy), PsychicWarrior.Powers.WeaponOfEnergy.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.SteadfastPerception), PsychicWarrior.Powers.SteadfastPerception.Configure, logger);

        // ── Phase 3c: 3rd-level powers ─────────────────────────────────────────
        Run(nameof(PsychicWarrior.Powers.PhysicalAcceleration), PsychicWarrior.Powers.PhysicalAcceleration.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.DimensionSlide), PsychicWarrior.Powers.DimensionSlide.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.EvadeBurst), PsychicWarrior.Powers.EvadeBurst.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.UbiquitousVision), PsychicWarrior.Powers.UbiquitousVision.Configure, logger);

        // ── Phase 3d: 4th-level powers ─────────────────────────────────────────
        Run(nameof(PsychicWarrior.Powers.InertialBarrier), PsychicWarrior.Powers.InertialBarrier.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.ZealousFury), PsychicWarrior.Powers.ZealousFury.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.EnergyAdaptation), PsychicWarrior.Powers.EnergyAdaptation.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.BattleTransformation), PsychicWarrior.Powers.BattleTransformation.Configure, logger);

        // ── Phase 9: 3rd-level powers ──────────────────────────────────────────
        Run(nameof(PsychicWarrior.Powers.VampiricBlade), PsychicWarrior.Powers.VampiricBlade.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.MentalBarrier), PsychicWarrior.Powers.MentalBarrier.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.ConcealingAmorphaGreater), PsychicWarrior.Powers.ConcealingAmorphaGreater.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.GraftWeapon), PsychicWarrior.Powers.GraftWeapon.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.KeenEdgePsionic), PsychicWarrior.Powers.KeenEdgePsionic.Configure, logger);

        // ── Phase 4: Class features (paths first, skill bonuses reference path GUIDs) ──
        Run(nameof(PsychicWarrior.Features.PsychicWarriorProficiencies), PsychicWarrior.Features.PsychicWarriorProficiencies.Configure, logger);
        Run(nameof(PsychicWarrior.Features.PsychicWarriorBonusFeat), PsychicWarrior.Features.PsychicWarriorBonusFeat.Configure, logger);

        // Individual paths must be configured before PathSelection (which references their GUIDs)
        // and before PathSkillBonus (which references WeaponmasterPath/BrawlerPath GUIDs).
        Run(nameof(PsychicWarrior.Features.Paths.WeaponmasterPath), PsychicWarrior.Features.Paths.WeaponmasterPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.BrawlerPath), PsychicWarrior.Features.Paths.BrawlerPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.ArcherPath), PsychicWarrior.Features.Paths.ArcherPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.AsceticPath), PsychicWarrior.Features.Paths.AsceticPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.AssassinsPath), PsychicWarrior.Features.Paths.AssassinsPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.DervishPath), PsychicWarrior.Features.Paths.DervishPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.FeralWarriorPath), PsychicWarrior.Features.Paths.FeralWarriorPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.GladiatorPath), PsychicWarrior.Features.Paths.GladiatorPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.InfiltratorPath), PsychicWarrior.Features.Paths.InfiltratorPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.InterceptorPath), PsychicWarrior.Features.Paths.InterceptorPath.Configure, logger);
        Run(nameof(PsychicWarrior.Powers.CallWeaponry), PsychicWarrior.Powers.CallWeaponry.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.MindKnightPath), PsychicWarrior.Features.Paths.MindKnightPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.SurvivorPath), PsychicWarrior.Features.Paths.SurvivorPath.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.PsychicWarriorPathSelection), PsychicWarrior.Features.Paths.PsychicWarriorPathSelection.Configure, logger);
        Run(nameof(PsychicWarrior.Features.PathSkillBonus), PsychicWarrior.Features.PathSkillBonus.Configure, logger);
        Run(nameof(PsychicWarrior.Features.Paths.TwistingPathsPathweaving), PsychicWarrior.Features.Paths.TwistingPathsPathweaving.Configure, logger);

        // ── Phase 5: Feats ─────────────────────────────────────────────────────
        Run(nameof(Shared.Feats.PsionicMeditation), Shared.Feats.PsionicMeditation.Configure, logger);
        Run(nameof(Shared.Feats.PsionicWeapon), Shared.Feats.PsionicWeapon.Configure, logger);

        // ── Phase 6b: Psionic Feats ────────────────────────────────────────────
        // SpeedOfThought/PsionicDodge inject components into PsionicFocusBuff — must run after Focus.
        Run(nameof(Shared.Feats.PsionicBody), Shared.Feats.PsionicBody.Configure, logger);
        Run(nameof(Shared.Feats.SpeedOfThought), Shared.Feats.SpeedOfThought.Configure, logger);
        Run(nameof(Shared.Feats.PsionicDodge), Shared.Feats.PsionicDodge.Configure, logger);
        Run(nameof(Shared.Feats.CriticalRefocus), Shared.Feats.CriticalRefocus.Configure, logger);
        Run(nameof(Shared.Feats.AdvancedWeaponmasterPath), Shared.Feats.AdvancedWeaponmasterPath.Configure, logger);
        Run(nameof(Shared.Feats.AdvancedAsceticPath), Shared.Feats.AdvancedAsceticPath.Configure, logger);
        Run(nameof(Shared.Feats.AdvancedArcherPath), Shared.Feats.AdvancedArcherPath.Configure, logger);
        Run(nameof(Shared.Feats.AdvancedAssassinPath), Shared.Feats.AdvancedAssassinPath.Configure, logger);
        Run(nameof(Shared.Feats.AdvancedBrawlerPath), Shared.Feats.AdvancedBrawlerPath.Configure, logger);
        Run(nameof(Shared.Feats.AdvancedDervishPath), Shared.Feats.AdvancedDervishPath.Configure, logger);
        Run(nameof(Shared.Feats.AdvancedFeralWarriorPath), Shared.Feats.AdvancedFeralWarriorPath.Configure, logger);
        Run(nameof(Shared.Feats.AdvancedMindKnightPath), Shared.Feats.AdvancedMindKnightPath.Configure, logger);
        Run(nameof(Shared.Feats.PsionicFist), Shared.Feats.PsionicFist.Configure, logger);
        Run(nameof(Shared.Feats.PsionicShot), Shared.Feats.PsionicShot.Configure, logger);
        // Greater feats require base feat GUIDs and register with PsionicProficiencyPatch
        Run(nameof(Shared.Feats.GreaterPsionicWeapon), Shared.Feats.GreaterPsionicWeapon.Configure, logger);
        Run(nameof(Shared.Feats.GreaterPsionicFist), Shared.Feats.GreaterPsionicFist.Configure, logger);
        Run(nameof(Shared.Feats.GreaterPsionicShot), Shared.Feats.GreaterPsionicShot.Configure, logger);
        // Tier 2 feats
        Run(nameof(Shared.Feats.RapidMetabolism), Shared.Feats.RapidMetabolism.Configure, logger);
        Run(nameof(Shared.Feats.CombatManifestation), Shared.Feats.CombatManifestation.Configure, logger);
        Run(nameof(Shared.Feats.DeepImpact), Shared.Feats.DeepImpact.Configure, logger);
        Run(nameof(Shared.Feats.UpTheWalls), Shared.Feats.UpTheWalls.Configure, logger);
        Run(nameof(Shared.Feats.PsionicEndowment), Shared.Feats.PsionicEndowment.Configure, logger);

        // ── Tier 1 feats ──────────────────────────────────────────────────────
        Run(nameof(Shared.Feats.PsionicCritical), Shared.Feats.PsionicCritical.Configure, logger);
        Run(nameof(Shared.Feats.RecklessOffense), Shared.Feats.RecklessOffense.Configure, logger);
        Run(nameof(Shared.Feats.AlignedAttack), Shared.Feats.AlignedAttack.Configure, logger);
        Run(nameof(Shared.Feats.WoundingAttack), Shared.Feats.WoundingAttack.Configure, logger);
        Run(nameof(Shared.Feats.FellShot), Shared.Feats.FellShot.Configure, logger);
        Run(nameof(Shared.Feats.UnavoidableStrike), Shared.Feats.UnavoidableStrike.Configure, logger);
        Run(nameof(Shared.Feats.IntuitiveFighting), Shared.Feats.IntuitiveFighting.Configure, logger);

        // Populate class-specific feat list now that all psionic feats are registered
        Run("BonusFeatSelection.PopulateClassSpecificFeats",
            PsychicWarrior.Features.PsychicWarriorBonusFeat.PopulateClassSpecificFeats, logger);

        // ── Phase 15: Level 20 Capstone ───────────────────────────────────────
        Run(nameof(PsychicWarrior.Features.EternalWarrior), PsychicWarrior.Features.EternalWarrior.Configure, logger);

        // ── Phase 6: Class definition (must come last) ─────────────────────────
        Run(nameof(PsychicWarriorSpellbook), PsychicWarriorSpellbook.Configure, logger);
        Run(nameof(PsychicWarrior.Features.PrebuildPsychicWarriorFeatureList), PsychicWarrior.Features.PrebuildPsychicWarriorFeatureList.Configure, logger);
        Run(nameof(PsychicWarriorClass), PsychicWarriorClass.Configure, logger);

        // ── SoulKnife ─────────────────────────────────────────────────────────
        Run(nameof(SoulKnife.SoulKnifeProficiencies),                       SoulKnife.SoulKnifeProficiencies.Configure,                       logger);
        Run(nameof(SoulKnife.Features.MindBlade.MindBlade),                 SoulKnife.Features.MindBlade.MindBlade.Configure,                 logger);
        Run(nameof(SoulKnife.Features.MindBlade.WeaponFocusMindBlade),      SoulKnife.Features.MindBlade.WeaponFocusMindBlade.Configure,      logger);
        Run(nameof(SoulKnife.Features.MindBlade.PsychicStrike),             SoulKnife.Features.MindBlade.PsychicStrike.Configure,             logger);
        Run(nameof(SoulKnife.Features.EnhancedMindBlade),                   SoulKnife.Features.EnhancedMindBlade.Configure,                   logger);
        Run(nameof(SoulKnife.Features.BladeSkills.FocusedOffense),         SoulKnife.Features.BladeSkills.FocusedOffense.Configure,          logger);
        Run(nameof(SoulKnife.Features.BladeSkills.FocusedDefense),         SoulKnife.Features.BladeSkills.FocusedDefense.Configure,          logger);
        Run(nameof(SoulKnife.Features.BladeSkills.EnergyBlades),            SoulKnife.Features.BladeSkills.EnergyBlades.Configure,            logger);
        Run(nameof(SoulKnife.Features.BladeSkills.MobilityBladeSkills),     SoulKnife.Features.BladeSkills.MobilityBladeSkills.Configure,     logger);
        Run(nameof(SoulKnife.Features.BladeSkills.CombatBladeSkills),       SoulKnife.Features.BladeSkills.CombatBladeSkills.Configure,       logger);
        Run(nameof(SoulKnife.Features.BladeSkills.BladeSkillsSelection),    SoulKnife.Features.BladeSkills.BladeSkillsSelection.Configure,    logger);
        Run(nameof(SoulKnife.SoulKnifeClass),                               SoulKnife.SoulKnifeClass.Configure,                               logger);
        // Recommended build — after the class + its features/selections exist (drives the
        // "Premade Build Balance" radar + "Use Recommended Build" button on the class screen).
        Run(nameof(SoulKnife.Features.SoulKnifePrebuild),                   SoulKnife.Features.SoulKnifePrebuild.Configure,                   logger);

        // ── Class registration ─────────────────────────────────────────────────
        try
        {
            var root = BlueprintRoot.Instance;
            var pwClassRef = BlueprintTool.GetRef<BlueprintCharacterClassReference>(Utils.Guids.PsychicWarriorClass);
            var skClassRef = BlueprintTool.GetRef<BlueprintCharacterClassReference>(Utils.Guids.SoulKnifeClass);

            if (!root.Progression.m_CharacterClasses.Contains(pwClassRef))
                root.Progression.m_CharacterClasses = [.. root.Progression.m_CharacterClasses, pwClassRef];
            if (!root.Progression.m_CharacterClasses.Contains(skClassRef))
                root.Progression.m_CharacterClasses = [.. root.Progression.m_CharacterClasses, skClassRef];
        }
        catch (System.Exception e)
        {
            logger.Error($"[Psionics] Class registration failed: {e}");
        }

    }
}
