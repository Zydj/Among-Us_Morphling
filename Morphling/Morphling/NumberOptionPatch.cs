using HarmonyLib;
using System;

namespace Morphling
{
    [HarmonyPatch]
    public static class NumberOptionPatch
    {
        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Increase))]
        public static bool Prefix(NumberOption __instance)
        {
            if (__instance.TitleText.Text == "Morphling Morph Duration")
            {
                Morphling.morphDuration = Math.Min(Morphling.morphDuration + 1f, 20);

                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);

                GameOptionsMenuPatch.morphlingMorphDuration.Field_3 = Morphling.morphDuration;
                GameOptionsMenuPatch.morphlingMorphDuration.Value = Morphling.morphDuration;
                GameOptionsMenuPatch.morphlingMorphDuration.ValueText.Text = Morphling.morphDuration.ToString();

                return false;
            }
            else if (__instance.TitleText.Text ==  "Morphling Sample Cooldown")
            {
                Morphling.sampleCooldown = Math.Min(Morphling.sampleCooldown + 1f, 30);

                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);

                GameOptionsMenuPatch.morphlingSampleCooldown.Field_3 = Morphling.sampleCooldown;
                GameOptionsMenuPatch.morphlingSampleCooldown.Value = Morphling.sampleCooldown;
                GameOptionsMenuPatch.morphlingSampleCooldown.ValueText.Text = Morphling.sampleCooldown.ToString();

                return false;
            }
            
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Decrease))]
        public static bool Prefix1(NumberOption __instance)
        {
            if (__instance.TitleText.Text == "Morphling Morph Duration")
            {
                Morphling.morphDuration = Math.Max(Morphling.morphDuration - 1f, 5);

                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);

                GameOptionsMenuPatch.morphlingMorphDuration.Field_3 = Morphling.morphDuration;
                GameOptionsMenuPatch.morphlingMorphDuration.Value = Morphling.morphDuration;
                GameOptionsMenuPatch.morphlingMorphDuration.ValueText.Text = Morphling.morphDuration.ToString();

                return false;
            }
            else if (__instance.TitleText.Text == "Morphling Sample Cooldown")
            {
                Morphling.sampleCooldown = Math.Max(Morphling.sampleCooldown - 1f, 10);

                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);

                GameOptionsMenuPatch.morphlingSampleCooldown.Field_3 = Morphling.sampleCooldown;
                GameOptionsMenuPatch.morphlingSampleCooldown.Value = Morphling.sampleCooldown;
                GameOptionsMenuPatch.morphlingSampleCooldown.ValueText.Text = Morphling.sampleCooldown.ToString();

                return false;
            }

            return true;
        }
    }
}
