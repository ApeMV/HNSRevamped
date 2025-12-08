global using HarmonyLib;
global using System.Collections.Generic;
global using System.Linq;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using System;
using UnityEngine;

namespace HNSRevamped;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
public partial class Main : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);
    public static BepInEx.Logging.ManualLogSource Logger;
    public static BasePlugin Instance;

    public static ConfigEntry<bool> GM { get; private set; }
    public static ConfigEntry<bool> UnlockFps { get; private set; }
    public static ConfigEntry<bool> ShowFps { get; private set; }
    public static ConfigEntry<bool> AutoStart { get; private set; }
    public static ConfigEntry<bool> DarkTheme { get; private set; }
    public static ConfigEntry<bool> LobbyMusic { get; private set; }

    public static bool HasArgumentException;
    public static string CredentialsText;

    public static PlayerControl[] AllPlayerControls
    {
        get
        {
            int count = PlayerControl.AllPlayerControls.Count;
            var result = new PlayerControl[count];
            var i = 0;

            foreach (PlayerControl pc in PlayerControl.AllPlayerControls)
            {
                if (pc == null || pc.PlayerId >= 254) continue;

                result[i++] = pc;
            }

            if (i == 0) return [];

            Array.Resize(ref result, i);
            return result;
        }
    }

    public override void Load()
    {
        var handler = HNSRevamped.Logger.Handler("GitVersion");        
        Logger = BepInEx.Logging.Logger.CreateLogSource("HNSRevamped");
        HNSRevamped.Logger.Enable();
        Instance = this;

        AutoStart = Config.Bind("Client Options", "AutoStart", false);
        GM = Config.Bind("Client Options", "GM", false);
        UnlockFps = Config.Bind("Client Options", "UnlockFPS", false);
        ShowFps = Config.Bind("Client Options", "ShowFPS", false);
        AutoStart = Config.Bind("Client Options", "AutoStart", false);
        DarkTheme = Config.Bind("Client Options", "DarkTheme", true);
        LobbyMusic = Config.Bind("Client Options", "LobbyMusic", false);

        BanManager.Init();
        
        Harmony.PatchAll();
    }

    [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
    class ModManagerLateUpdatePatch
    {
        public static void Prefix(ModManager __instance)
        {
            __instance.ShowModStamp();
            LateTask.Update(Time.deltaTime);
        }
    }
}
