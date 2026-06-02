using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Psionics.Utils;

namespace Psionics.PsychicWarrior.Features;

public static class PsychicWarriorProficiencies
{
    public static void Configure()
    {
        var icon = FeatureRefs.MartialWeaponProficiency.Reference.Get().Icon;

        FeatureConfigurator.New("PsychicWarriorProficiencies", Guids.Proficiencies)
            .SetDisplayName(Loc.Str("PW.Proficiencies.Name", "Psychic Warrior Proficiencies"))
            .SetDescription(Loc.Str("PW.Proficiencies.Desc", "Psychic Warriors are proficient with all simple and martial weapons, and with all armor (heavy, light, and medium) and shields (except tower shields)."))
            .SetIcon(icon)
            .SetIsClassFeature()
            .AddFacts(
            [
                FeatureRefs.SimpleWeaponProficiency.ToString(),
                FeatureRefs.MartialWeaponProficiency.ToString(),
                FeatureRefs.LightArmorProficiency.ToString(),
                FeatureRefs.MediumArmorProficiency.ToString(),
                FeatureRefs.HeavyArmorProficiency.ToString(),
                FeatureRefs.ShieldsProficiency.ToString()
            ])
            .Configure();
    }
}
