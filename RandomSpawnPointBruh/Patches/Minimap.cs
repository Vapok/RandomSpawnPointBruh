using HarmonyLib;
using Jotunn.Managers;

namespace RandomSpawnPointBruh.Patches;

[HarmonyPatch(typeof(Minimap), "UpdateExplore")]
internal static class MinimapUpdateExplorePatch
{
    private static bool _suppressionActive;

    [HarmonyPrefix]
    private static bool Prefix(Player player)
    {
        if (GUIManager.IsHeadless())
        {
            return true;
        }

        if (player != null && player.InIntro())
        {
            if (!_suppressionActive)
            {
                _suppressionActive = true;
                RandomSpawnPointBruh.Log.Debug("Minimap: Sensor active - Suppressing fog-of-war exploration during Valkyrie flight.");
            }

            return false;
        }

        if (_suppressionActive)
        {
            _suppressionActive = false;
            RandomSpawnPointBruh.Log.Debug("Minimap: Valkyrie flight complete - Fog-of-war exploration resumed.");
        }

        return true;
    }
}
