using HarmonyLib;
using UnityEngine;
using System.Linq;
using UnhollowerBaseLib;

namespace Morphling
{
    [HarmonyPatch]
    class CustomPlayerMenuPatch
    {
        public static void DeleteOptions(bool destroy)
        {
            if (GameOptionsMenuPatch.showMorphlingOption != null && GameOptionsMenuPatch.morphlingSampleCooldown != null && GameOptionsMenuPatch.morphlingMorphDuration != null)
            {
                GameOptionsMenuPatch.showMorphlingOption.gameObject.SetActive(false);
                GameOptionsMenuPatch.morphlingSampleCooldown.gameObject.SetActive(false);
                GameOptionsMenuPatch.morphlingMorphDuration.gameObject.SetActive(false);

                if (destroy)
                {
                    GameObject.Destroy(GameOptionsMenuPatch.showMorphlingOption);
                    GameObject.Destroy(GameOptionsMenuPatch.morphlingSampleCooldown);
                    GameObject.Destroy(GameOptionsMenuPatch.morphlingMorphDuration);

                    GameOptionsMenuPatch.showMorphlingOption = null;
                    GameOptionsMenuPatch.morphlingSampleCooldown = null;
                    GameOptionsMenuPatch.morphlingMorphDuration = null;
                }
            }
        }

        public static void AddOptions()
        {
            if (Morphling.debug)
            {
                Morphling.log.LogMessage("Adding options");
            }

            if (GameOptionsMenuPatch.showMorphlingOption == null || GameOptionsMenuPatch.morphlingSampleCooldown == null || GameOptionsMenuPatch.morphlingMorphDuration == null)
            {
                ToggleOption showAnonymousVotes = GameObject.FindObjectsOfType<ToggleOption>().ToList().Where(x => x.TitleText.Text == "Anonymous Votes").First();
                GameOptionsMenuPatch.showMorphlingOption = GameObject.Instantiate(showAnonymousVotes);

                NumberOption killCooldown = GameObject.FindObjectsOfType<NumberOption>().ToList().Where(x => x.TitleText.Text == "Kill Cooldown").First();
                GameOptionsMenuPatch.morphlingMorphDuration = GameObject.Instantiate(killCooldown);

                GameOptionsMenuPatch.morphlingSampleCooldown = GameObject.Instantiate(killCooldown);

                OptionBehaviour[] options = new OptionBehaviour[GameOptionsMenuPatch.instance.Children.Count + 3];
                GameOptionsMenuPatch.instance.Children.ToArray().CopyTo(options, 0);
                options[options.Length - 3] = GameOptionsMenuPatch.showMorphlingOption;
                options[options.Length - 2] = GameOptionsMenuPatch.morphlingMorphDuration;
                options[options.Length - 1] = GameOptionsMenuPatch.morphlingSampleCooldown;
                GameOptionsMenuPatch.instance.Children = new Il2CppReferenceArray<OptionBehaviour>(options);
            }
            else
            {
                GameOptionsMenuPatch.showMorphlingOption.gameObject.SetActive(true);
                GameOptionsMenuPatch.morphlingMorphDuration.gameObject.SetActive(true);
                GameOptionsMenuPatch.morphlingSampleCooldown.gameObject.SetActive(true);
            }

            GameOptionsMenuPatch.showMorphlingOption.TitleText.Text = "Morphling Role";
            GameOptionsMenuPatch.showMorphlingOption.oldValue = Morphling.morphlingEnabled;
            GameOptionsMenuPatch.showMorphlingOption.CheckMark.enabled = Morphling.morphlingEnabled;

            GameOptionsMenuPatch.morphlingMorphDuration.TitleText.Text = "Morphling Morph Duration";
            GameOptionsMenuPatch.morphlingMorphDuration.Value = Morphling.morphDuration;
            GameOptionsMenuPatch.morphlingMorphDuration.ValueText.Text = Morphling.morphDuration.ToString();

            GameOptionsMenuPatch.morphlingSampleCooldown.TitleText.Text = "Morphling Sample Cooldown";
            GameOptionsMenuPatch.morphlingSampleCooldown.Value = Morphling.sampleCooldown;
            GameOptionsMenuPatch.morphlingSampleCooldown.ValueText.Text = Morphling.sampleCooldown.ToString();
        }

        [HarmonyPatch(typeof(CustomPlayerMenu), nameof(CustomPlayerMenu.Close))]
        public static void Postfix()
        {
            DeleteOptions(true);
        }

        [HarmonyPatch(typeof(CustomPlayerMenu), nameof(CustomPlayerMenu.OpenTab))]
        public static void Prefix(GameObject CCAHNLMBCOD, CustomPlayerMenu __instance)
        {
            if (CCAHNLMBCOD.name == "GameGroup" && GameOptionsMenuPatch.instance != null)
            {
                AddOptions();
            }
            else
            {
                DeleteOptions(false);
            }
        }
    }
}
