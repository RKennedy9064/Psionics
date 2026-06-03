using BlueprintCore.Utils;
using Kingmaker.Localization;

namespace Psionics.Utils;

/// <summary>
/// Thin wrapper around <see cref="LocalizationTool.CreateString"/> that defaults
/// <c>tagEncyclopediaEntries</c> to <c>false</c>. The default <c>true</c> behavior wraps recognized
/// words (e.g. "Speed", "Body", "Initiative") with <c>&lt;link="Encyclopedia:..."&gt;</c> tags,
/// which some WoTR UI contexts (e.g. FeatureSelection lists, ability variant lists) render as raw
/// text rather than styled links. We never want our custom strings auto-tagged.
/// </summary>
public static class Loc
{
    public static LocalizedString Str(string key, string value, bool tagEncyclopediaEntries = false)
        => LocalizationTool.CreateString(key, value, tagEncyclopediaEntries);
}
