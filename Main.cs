using HarmonyLib;
using UnityModManagerNet;
using BlueprintCore.Utils;

namespace Psionics;

public static class Main
{
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
        LogWrapper logger = LogWrapper.Get("Psionics");

        var harmony = new Harmony(modEntry.Info.Id);
        try
        {
            harmony.PatchAll();
            logger.Info("PsychicWarrior: PatchAll succeeded");
        }
        catch (System.Exception e)
        {
            logger.Error($"PsychicWarrior: PatchAll threw: {e}");
            UnityEngine.Debug.LogError($"[Psionics] PatchAll threw: {e}");
        }

        return true;
    }
}