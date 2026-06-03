using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features.Paths;

public static class PsychicWarriorPathSelection
{
    public static void Configure()
    {
        var icon = AbilityRefs.DivineFavor.Reference.Get().Icon;

        FeatureSelectionConfigurator.New("PsychicWarriorPathSelection", Guids.PathSelectionLevel1)
            .SetDisplayName(Loc.Str("PW.PathSelection.Name", "Psychic Warrior Path"))
            .SetDescription(Loc.Str("PW.PathSelection.Desc",
                "At 1st level, a psychic warrior chooses a path that shapes her training and fighting style. " +
                "She gains the trance and maneuver of her chosen path."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddToAllFeatures(
                Guids.WeaponmasterPath,
                Guids.BrawlerPath,
                Guids.ArcherPath,
                Guids.AsceticPath,
                Guids.AssassinsPath,
                Guids.DervishPath,
                Guids.FeralWarriorPath,
                Guids.GladiatorPath,
                Guids.InfiltratorPath,
                Guids.InterceptorPath,
                Guids.MindKnightPath,
                Guids.SurvivorPath)
            .Configure();

        FeatureSelectionConfigurator.New("PathExpandedManeuver", Guids.PathExpandedManeuver)
            .SetDisplayName(Loc.Str("PW.PathExpandedManeuver.Name", "Expanded Path Maneuver"))
            .SetDescription(Loc.Str("PW.PathExpandedManeuver.Desc",
                "At 3rd level, you gain an additional maneuver from your chosen path, expanding your tactical options in combat."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .SetIgnorePrerequisites(false)
            .AddToAllFeatures(
                Guids.WeaponmasterExpandedManeuver,
                Guids.BrawlerExpandedManeuver,
                Guids.ArcherExpandedFeature,
                Guids.AsceticExpandedFeature,
                Guids.AssassinsExpandedFeature,
                Guids.DervishExpandedFeature,
                Guids.FeralWarriorExpandedFeature,
                Guids.GladiatorExpandedFeature,
                Guids.InfiltratorExpandedFeature,
                Guids.InterceptorExpandedFeature,
                Guids.MindKnightExpandedFeature,
                Guids.SurvivorExpandedFeature)
            .Configure();

        FeatureSelectionConfigurator.New("SecondaryPathSelection", Guids.SecondaryPathSelection)
            .SetDisplayName(Loc.Str("PW.SecondaryPath.Name", "Secondary Path"))
            .SetDescription(Loc.Str("PW.SecondaryPath.Desc",
                "At 9th level, a psychic warrior chooses a second warrior's path. He gains the trance and maneuver of that path. " +
                "The psychic warrior may only benefit from one trance at a time (see Twisting Paths)."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddToAllFeatures(
                Guids.WeaponmasterPath,
                Guids.BrawlerPath,
                Guids.ArcherPath,
                Guids.AsceticPath,
                Guids.AssassinsPath,
                Guids.DervishPath,
                Guids.FeralWarriorPath,
                Guids.GladiatorPath,
                Guids.InfiltratorPath,
                Guids.InterceptorPath,
                Guids.MindKnightPath,
                Guids.SurvivorPath)
            .Configure();

        // Twisting Paths and Pathweaving features are configured in TwistingPathsPathweaving.cs (Phase 7).
    }
}
