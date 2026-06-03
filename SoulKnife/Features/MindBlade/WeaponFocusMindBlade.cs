using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Psionics.Shared.Mechanics;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.MindBlade;

public static class WeaponFocusMindBlade
{
    public static void Configure()
    {
        FeatureConfigurator.New("SKWeaponFocusMindBlade", Guids.WeaponFocusMindBlade)
            .SetDisplayName(Loc.Str("SK.WeaponFocusMB.Name", "Weapon Focus (Mind Blade)"))
            .SetDescription(Loc.Str("SK.WeaponFocusMB.Desc",
                "You gain a +1 bonus on all attack rolls made with your mind blade."))
            .SetIcon(FeatureRefs.WeaponFocusGreatsword.Reference.Get().Icon)
            .SetIsClassFeature()
            .AddComponent<WeaponFocusMindBladeComponent>()
            .Configure();
    }
}
