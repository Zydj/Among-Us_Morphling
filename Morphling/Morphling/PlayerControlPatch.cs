using HarmonyLib;
using Hazel;
using System.Linq;

namespace Morphling
{
    [HarmonyPatch]
    class PlayerControlPatch
    {
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        static void Postfix(byte ACCJCEHMKLN, MessageReader HFPCBBHJIPJ)
        {
            if (Morphling.debug)
            {
                Morphling.log.LogMessage("RPC is:" + ACCJCEHMKLN);
            }

            switch (ACCJCEHMKLN)
            {
                case (byte)CustomRPC.SyncCustomSettingsMorphling:
                    {
                        Morphling.morphlingEnabled = HFPCBBHJIPJ.ReadBoolean();
                        Morphling.morphDuration = System.BitConverter.ToSingle(HFPCBBHJIPJ.ReadBytes(4).ToArray(), 0);
                        Morphling.sampleCooldown = System.BitConverter.ToSingle(HFPCBBHJIPJ.ReadBytes(4).ToArray(), 0);
                        break;
                    }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
        public static void Postfix()
        {
            if (PlayerControl.AllPlayerControls.Count > 1)
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SyncCustomSettingsMorphling, Hazel.SendOption.Reliable);
                writer.Write(Morphling.morphlingEnabled);
                writer.Write(Morphling.morphDuration);
                writer.Write(Morphling.sampleCooldown);
                writer.EndMessage();
            }
        }
    }
}
