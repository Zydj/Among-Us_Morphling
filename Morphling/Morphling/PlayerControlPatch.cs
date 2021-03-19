using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;

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
                case (byte)CustomRPC.SetLocalPlayersMorphling:
                    {
                        if (Morphling.debug)
                        {
                            Morphling.log.LogMessage("Setting local players for sheriff");
                        }

                        Morphling.localPlayers.Clear();
                        Morphling.localPlayer = PlayerControl.LocalPlayer;

                        var localPlayerBytes = HFPCBBHJIPJ.ReadBytesAndSize();

                        foreach (var id in localPlayerBytes)
                        {
                            foreach (var player in PlayerControl.AllPlayerControls)
                            {
                                if (player.PlayerId == id)
                                {
                                    Morphling.localPlayers.Add(player);
                                }
                            }
                        }
                        break;
                    }

                case (byte)CustomRPC.SetMorphling:
                    {
                        if (Morphling.debug)
                        {
                            Morphling.log.LogMessage("Setting Morphling");
                        }

                        Morphling.introDone = false;

                        PlayerController.InitPlayers();
                        Player p = PlayerController.getPlayerById(HFPCBBHJIPJ.ReadByte());
                        p.components.Add("Morphling");

                        if (PlayerController.getLocalPlayer().hasComponent("Morphling"))
                        {
                            Morphling.sampleTimer = 0;
                        }

                        if (Morphling.debug)
                        {
                            Morphling.log.LogMessage("Morphling is: " + p.playerdata.nameText.Text);
                        }
                        break;
                    }

                case (byte)CustomRPC.SyncCustomSettingsMorphling:
                    {
                        Morphling.morphlingEnabled = HFPCBBHJIPJ.ReadBoolean();
                        Morphling.morphDuration = System.BitConverter.ToSingle(HFPCBBHJIPJ.ReadBytes(4).ToArray(), 0);
                        Morphling.sampleCooldown = System.BitConverter.ToSingle(HFPCBBHJIPJ.ReadBytes(4).ToArray(), 0);
                        break;
                    }
            }
        }

        public static List<Player> getImposters(Il2CppReferenceArray<GameData.Nested_1> infection)
        {
            List<Player> imposters = new List<Player>();
            foreach (Player player in PlayerController.players)
            {
                bool isInfected = false;
                foreach (GameData.Nested_1 infected in infection)
                {
                    if (player.playerdata.PlayerId == infected.IBJBIALCEKB.PlayerId)
                    {
                        isInfected = true;
                        break;
                    }
                }

                if (isInfected)
                {
                    imposters.Add(player);
                }
            }
            return imposters;
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
        public static void Postfix(Il2CppReferenceArray<GameData.Nested_1> FMAOEJEHPAO)
        {
            if (!Morphling.morphlingEnabled)
            {
                return;
            }

            Morphling.introDone = false;

            PlayerController.InitPlayers();

            List<Player> imposters = getImposters(FMAOEJEHPAO);
            var morphlingId = new System.Random().Next(0, imposters.Count);
            Player morphling = imposters[morphlingId];

            MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetMorphling, Hazel.SendOption.Reliable);
            writer.Write(morphling.playerdata.PlayerId);
            writer.EndMessage();
            morphling.components.Add("Morphling");

            if (PlayerController.getLocalPlayer().hasComponent("Morphling"))
            {
                Morphling.sampleTimer = 0;
            }

            Morphling.localPlayers.Clear();
            Morphling.localPlayer = PlayerControl.LocalPlayer;

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                Morphling.localPlayers.Add(player);
            }

            writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetLocalPlayersMorphling, Hazel.SendOption.Reliable);
            writer.WriteBytesAndSize(Morphling.localPlayers.Select(player => player.PlayerId).ToArray());
            writer.EndMessage();
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
