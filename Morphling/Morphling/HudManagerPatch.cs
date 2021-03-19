using HarmonyLib;
using InnerNet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Morphling
{
    [HarmonyPatch]
    public static class HudManagerPatch
    {
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        static void Postfix(HudManager __instance)
        {
            if (!Morphling.morphlingEnabled)
            {
                return;
            }

            if (AmongUsClient.Instance.GameState != InnerNetClient.Nested_0.Started)
                return;

            if (!Morphling.introDone)
            {
                return;
            }

            if (PlayerController.LocalPlayer.hasComponent("Morphling") && !PlayerController.LocalPlayer.playerdata.Data.AKOHOAJIHBE)
            {
                KillButtonManager morphButton = GameObject.FindObjectsOfType<KillButtonManager>().ToList().Where(x => x.name == "Morph").FirstOrDefault();

                if (morphButton != null)
                {
                    morphButton.gameObject.SetActive(false);
                    morphButton.isActive = false;

                    GameObject.Destroy(morphButton);

                    morphButton = null;
                }

                morphButton = GameObject.Instantiate(__instance.KillButton);
                morphButton.transform.position = __instance.KillButton.transform.position + new Vector3(0, 1.25f, 0);                                   

                morphButton.gameObject.SetActive(true);
                morphButton.isActive = true;

                morphButton.name = "Morph";                          
            }
        }
    }
}
