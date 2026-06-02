using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Psionics.Utils;

namespace Psionics.SoulKnife;

public static class SoulKnifeProficiencies
{
    public static void Configure()
    {
        FeatureConfigurator.New("SoulKnifeProficiencies", Guids.SoulKnifeProficiencies)
            .SetDisplayName(Loc.Str("SK.Proficiencies.Name", "Soulknife Proficiencies"))
            .SetDescription(Loc.Str("SK.Proficiencies.Desc",
                "A soulknife is proficient with all simple and martial weapons, with light and medium armor, and with shields (except tower shields). She is always proficient with her mind blade, regardless of form."))
            .SetIcon(FeatureRefs.MartialWeaponProficiency.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddFacts(
            [
                FeatureRefs.SimpleWeaponProficiency.ToString(),
                FeatureRefs.MartialWeaponProficiency.ToString(),
                FeatureRefs.LightArmorProficiency.ToString(),
                FeatureRefs.MediumArmorProficiency.ToString(),
                FeatureRefs.ShieldsProficiency.ToString(),
            ])
            .Configure();
    }
}
