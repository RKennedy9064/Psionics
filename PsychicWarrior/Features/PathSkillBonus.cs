using System.Collections.Generic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Psionics.Utils;
using UnityEngine;

namespace Psionics.PsychicWarrior.Features;

/// <summary>
/// Path Skill (Ex): At 4th level the psychic warrior gains a +2 competence bonus to one skill
/// associated with a path he is on. Every three levels thereafter he can increase the bonus to
/// one path skill by +2 (maximum +6 for any one skill). The same skill may be chosen again.
///
/// Implementation: one feature per skill (Athletics, Mobility, Perception, Stealth) with an
/// OR-prerequisite over all paths that grant that skill. The selection only shows skills the
/// player's current path(s) qualify for.
/// </summary>
public static class PathSkillBonus
{
    public static void Configure()
    {
        var athleticsIcon = FeatureRefs.SkillFocusPhysique.Reference.Get().Icon;
        var mobilityIcon = FeatureRefs.SkillFocusAcrobatics.Reference.Get().Icon;
        var perceptionIcon = FeatureRefs.SkillFocusPerception.Reference.Get().Icon;
        var stealthIcon = FeatureRefs.SkillFocusStealth.Reference.Get().Icon;

        var athletics = MakeSkill(
            "PathSkillAthletics", Guids.PathSkillAthletics,
            "Athletics", StatType.SkillAthletics, athleticsIcon,
            Guids.WeaponmasterPath, Guids.BrawlerPath, Guids.DervishPath,
            Guids.FeralWarriorPath, Guids.GladiatorPath);

        var mobility = MakeSkill(
            "PathSkillMobility", Guids.PathSkillMobility,
            "Mobility", StatType.SkillMobility, mobilityIcon,
            Guids.BrawlerPath, Guids.AsceticPath, Guids.AssassinsPath,
            Guids.DervishPath, Guids.InfiltratorPath, Guids.InterceptorPath);

        var perception = MakeSkill(
            "PathSkillPerception", Guids.PathSkillPerception,
            "Perception", StatType.SkillPerception, perceptionIcon,
            Guids.WeaponmasterPath, Guids.ArcherPath, Guids.AsceticPath,
            Guids.GladiatorPath, Guids.InterceptorPath, Guids.MindKnightPath,
            Guids.SurvivorPath);

        var stealth = MakeSkill(
            "PathSkillStealth", Guids.PathSkillStealth,
            "Stealth", StatType.SkillStealth, stealthIcon,
            Guids.ArcherPath, Guids.AssassinsPath, Guids.FeralWarriorPath,
            Guids.InfiltratorPath, Guids.MindKnightPath, Guids.SurvivorPath);

        FeatureSelectionConfigurator.New("PathSkillBonusSelection", Guids.PathSkillBonusSelection)
            .SetDisplayName(Loc.Str("PW.PathSkillSel.Name", "Path Skill", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str("PW.PathSkillSel.Desc",
                "Select one skill associated with your warrior path to gain a +2 competence bonus (maximum +6 for any one skill).",
                tagEncyclopediaEntries: false))
            .SetIcon(perceptionIcon)
            .SetIsClassFeature(true)
            .SetIgnorePrerequisites(false)
            .AddToAllFeatures(athletics, mobility, perception, stealth)
            .Configure();
    }

    private static BlueprintFeature MakeSkill(
        string name, string guid, string skillLabel, StatType skill, Sprite icon,
        params string[] eligiblePathGuids)
    {
        var prereqs = new List<Blueprint<BlueprintFeatureReference>>();
        foreach (var p in eligiblePathGuids)
            prereqs.Add(p);

        return FeatureConfigurator.New(name, guid)
            .SetDisplayName(Loc.Str($"PW.{name}.Name",
                $"Path Skill: {skillLabel}", tagEncyclopediaEntries: false))
            .SetDescription(Loc.Str($"PW.{name}.Desc",
                $"You gain a +2 competence bonus to {skillLabel} checks. Can be selected up to three times (maximum +6).",
                tagEncyclopediaEntries: false))
            .SetIcon(icon)
            .SetIsClassFeature()
            .SetRanks(3)
            .AddStatBonus(descriptor: ModifierDescriptor.Competence, stat: skill, value: 2)
            .AddPrerequisiteFeaturesFromList(prereqs, amount: 1)
            .Configure();
    }
}
