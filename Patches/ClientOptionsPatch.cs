using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace HNSRevamped
{
    // https://github.com/tukasa0001/TownOfHost
    [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Start))]
    public static class OptionsMenuBehaviourStartPatch
    {
        private static ClientOptionItem GM; // Currently not used yet
        private static ClientOptionItem UnlockFPS;
        private static ClientOptionItem ShowFPS;
        private static ClientOptionItem AutoStart;
        private static ClientOptionItem DarkTheme;
        private static ClientOptionItem LobbyMusic;

        public static void Postfix(OptionsMenuBehaviour __instance)
        {
            if (__instance.DisableMouseMovement == null) return;

            if (GM == null || GM.ToggleButton == null)
            {
                GM = ClientOptionItem.Create("GM", Main.GM, __instance, GMButtonToggle);

                static void GMButtonToggle()
                {
                    if (Main.GM.Value)
                        HudManager.Instance.ShowPopUp("Hi there! GM hasn't been implemented yet. Sorry for the inconvenience! -MV");
                }
            }

            if (UnlockFPS == null || UnlockFPS.ToggleButton == null)
            {
                UnlockFPS = ClientOptionItem.Create("UnlockFPS", Main.UnlockFps, __instance, UnlockFPSButtonToggle);

                static void UnlockFPSButtonToggle()
                {
                    Application.targetFrameRate = Main.UnlockFps.Value ? 120 : 60;
                    Logger.SendInGame($"FPS Set To {Application.targetFrameRate}");
                }
            }

            if (ShowFPS == null || ShowFPS.ToggleButton == null)
                ShowFPS = ClientOptionItem.Create("ShowFPS", Main.ShowFps, __instance);

            if (AutoStart == null || AutoStart.ToggleButton == null)
            {
                AutoStart = ClientOptionItem.Create("AutoStart", Main.AutoStart, __instance, AutoStartButtonToggle);

                static void AutoStartButtonToggle()
                {
                    if (!Main.AutoStart.Value && GameStartManager.InstanceExists &&
                        GameStartManager.Instance.startState == GameStartManager.StartingStates.Countdown)
                    {
                        GameStartManager.Instance.ResetStartState();
                        Logger.SendInGame("CancelStartCountDown");
                    }
                }
            }

            if (DarkTheme == null || DarkTheme.ToggleButton == null)
                DarkTheme = ClientOptionItem.Create("EnableDarkTheme", Main.DarkTheme, __instance);

            if (LobbyMusic == null || LobbyMusic.ToggleButton == null)
                LobbyMusic = ClientOptionItem.Create("LobbyMusic", Main.LobbyMusic, __instance);
        }
    }

    [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Close))]
    public static class OptionsMenuBehaviourClosePatch
    {
        public static void Postfix()
        {
            ClientOptionItem.CustomBackground?.gameObject.SetActive(false);
        }
    }
}