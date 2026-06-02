using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features;

/// <summary>
/// Psionic Proficiency (Ex): A psychic warrior treats his base attack bonus as equal to his psychic warrior
/// level for the purposes of requirements for psionic feats. Base attack bonuses granted from other classes
/// are unaffected and are added normally.
/// </summary>
public static class PsionicProficiency
{
    public static void Configure()
    {
        var icon = AbilityRefs.DivineFavor.Reference.Get().Icon;

        FeatureConfigurator.New("PsionicProficiency", Guids.PsionicProficiency)
            .SetDisplayName(Loc.Str("PW.PsionicProf.Name", "Psionic Proficiency"))
            .SetDescription(Loc.Str(
                "PW.PsionicProf.Desc",
                "You treat your base attack bonus as equal to your Psychic Warrior level for the purposes of meeting prerequisites for psionic feats. Base attack bonuses granted from other classes are unaffected and are added normally."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .Configure();
    }
}
