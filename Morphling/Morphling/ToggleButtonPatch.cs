using HarmonyLib;

namespace Morphling
{
    [HarmonyPatch]
    class ToggleButtonPatch
    {
        [HarmonyPatch(typeof(ToggleOption), nameof(ToggleOption.Toggle))]
        public static bool Prefix(ToggleOption __instance)
        {
            if (__instance.TitleText.Text == "Morphling Role")
            {
                Morphling.morphlingEnabled = !Morphling.morphlingEnabled;
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);

                __instance.oldValue = Morphling.morphlingEnabled;
                __instance.CheckMark.enabled = Morphling.morphlingEnabled;

                return false;
            }

            return true;
        }
    }
}
