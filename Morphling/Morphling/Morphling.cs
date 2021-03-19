using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using System.Collections.Generic;

namespace Morphling
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class Morphling : BasePlugin
    {
        public static bool debug = true;

        public const string Id = "org.bepinex.plugins.Morphling";
        public static BepInEx.Logging.ManualLogSource log;

        public static bool morphlingEnabled = true;
        public static bool introDone = false;

        public static float sampleCooldown = 25f;
        public static float morphDuration = 12f;
        public static float sampleTimer = sampleCooldown;
        public static float morphTimer = morphDuration;

        public static PlayerControl localPlayer = null;
        public static List<PlayerControl> localPlayers = new List<PlayerControl>();

        public Harmony Harmony { get; } = new Harmony(Id);

        public ConfigEntry<string> Name { get; private set; }

        public override void Load()
        {
            log = Log;
            log.LogMessage("Morphling Mod Loaded");

            Harmony.PatchAll();
        }
    }
}
