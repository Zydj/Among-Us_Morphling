using HarmonyLib;
using UnityEngine;

namespace Morphling
{
    [HarmonyPatch]
    class GameOptionsMenuPatch
    {
        public static ToggleOption showMorphlingOption;
        public static NumberOption morphlingSampleCooldown;
        public static NumberOption morphlingMorphDuration;

        public static OptionBehaviour option;

        public static GameOptionsMenu instance;

        public static float defaultBounds;

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        public static void Postfix(GameOptionsMenu __instance)
        {
            if (Morphling.debug)
            {
                Morphling.log.LogMessage("Starting");
            }

            instance = __instance;

            defaultBounds = __instance.GetComponentInParent<Scroller>().YBounds.max;

            option = __instance.Children[__instance.Children.Count - 1];

            CustomPlayerMenuPatch.AddOptions();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        public static void Postfix1(GameOptionsMenu __instance)
        {
            if (showMorphlingOption != null && morphlingSampleCooldown != null && morphlingMorphDuration != null)
            {
                showMorphlingOption.transform.position = option.transform.position - new Vector3(0, 0.5f, 0);
                morphlingSampleCooldown.transform.position = option.transform.position - new Vector3(0, 1f, 0);
                morphlingMorphDuration.transform.position = option.transform.position - new Vector3(0, 1.5f, 0);

                __instance.GetComponentInParent<Scroller>().YBounds.max = defaultBounds + 1.5f;
            }
        }
    }
}
