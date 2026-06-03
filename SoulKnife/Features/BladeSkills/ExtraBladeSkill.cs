using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Psionics.Utils;

namespace Psionics.SoulKnife.Features.BladeSkills;

/// <summary>
/// Extra Blade Skill [Psionic]: a general feat that grants one additional blade skill. Implemented as a
/// feat-group FeatureSelection whose options mirror the soulknife's Blade Skill list — picking it in a feat
/// slot cascades straight into a blade-skill choice (and the chosen skill, not this selection, is what the
/// character ends up with, so it can be taken again in a later feat slot for a different skill). Each blade
/// skill keeps its own prerequisites.
/// </summary>
public static class ExtraBladeSkill
{
    public static void Configure()
    {
        FeatureSelectionConfigurator.New("SKExtraBladeSkill", Guids.ExtraBladeSkillFeat)
            .SetDisplayName(Loc.Str("SK.ExtraBladeSkill.Name", "Extra Blade Skill"))
            .SetDescription(Loc.Str("SK.ExtraBladeSkill.Desc",
                "You have unlocked a new facet of your mind blade. You gain one additional blade skill of your "
                + "choice (you must meet its prerequisites). You can select this feat more than once."))
            .SetIcon(AbilityRefs.MagicWeapon.Reference.Get().Icon)
            .SetGroup(FeatureGroup.Feat)
            // Requires the blade skill class feature; soulknives gain Blade Skills at 2nd level.
            .AddPrerequisiteClassLevel(Guids.SoulKnifeClass, 2)
            .OnConfigure(bp =>
            {
                // Mirror the live Blade Skill list so the feat always offers exactly the same options.
                var src = BlueprintTool.Get<BlueprintFeatureSelection>(Guids.BladeSkillsSelection);
                if (src != null) bp.m_AllFeatures = [.. src.m_AllFeatures];
            })
            .Configure();

        // Offer it in the general feat list (soulknife prerequisite hides it from everyone else).
        FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection.ToString())
            .OnConfigure(bp =>
            {
                var featRef = BlueprintTool.GetRef<BlueprintFeatureReference>(Guids.ExtraBladeSkillFeat);
                bp.m_AllFeatures = [.. bp.m_AllFeatures.Where(f => f.Guid != featRef.Guid), featRef];
            })
            .Configure();
    }
}
