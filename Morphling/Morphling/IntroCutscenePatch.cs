using HarmonyLib;

namespace Morphling
{
    [HarmonyPatch]
    public static class IntroCutscenePatch
    {
        [HarmonyPatch(typeof(IntroCutscene.Nested_0), nameof(IntroCutscene.Nested_0.MoveNext))]
        public static void Postfix(IntroCutscene.Nested_0 __instance)
        {
            if (!Morphling.morphlingEnabled)
            {
                return;
            }

            if (PlayerController.getLocalPlayer().hasComponent("Morphling"))
            {
                __instance.__this.Title.Text = "Morphling";
            }
        }
    }
}
