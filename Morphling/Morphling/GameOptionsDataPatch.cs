using HarmonyLib;

namespace Morphling
{
    [HarmonyPatch]
    public class GameOptionsDataPatch
    {
        [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.Method_5))]
        public static void Postfix(ref string __result)
        {
            if (Morphling.morphlingEnabled)
            {
                __result += "Morphling Role: On\n";
            }
            else
            {
                __result += "Morphling Role: Off\n";
            }

            __result += "Morphling Morph Duration: " + Morphling.morphDuration.ToString() + "s\n";

            __result += "Morphling Smaple Cooldown: " + Morphling.sampleCooldown.ToString() + "s\n";

            HudManager.Instance.GameSettings.scale = 0.6f;
        }
    }
}
