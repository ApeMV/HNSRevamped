using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

// https://github.com/tukasa0001/TownOfHost/blob/main/Modules/OptionHolder.cs
namespace HNSRevamped
{
    [HarmonyPatch]
    public static class Options
    {
        static Task taskOptionsLoad;

        [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.Initialize)), HarmonyPostfix]
        public static void OptionsLoadStart()
        {
            taskOptionsLoad = Task.Run(Load);
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPostfix]
        public static void WaitOptionsLoad()
        {
            taskOptionsLoad.Wait();
        }

        public static OptionItem Testy;
        public static OptionItem Testy1;
        public static OptionItem Testy2;


        public static bool IsLoaded = false;

        public static void Load()
        {
            if (IsLoaded) return;

            //Main settings
            Testy = IntegerOptionItem.Create(3, "Testy", new IntegerValueRule(0, 50, 5), 10, TabGroup.GameModifiers, false);
            Testy1 = BooleanOptionItem.Create(4, "Testy1", true, TabGroup.ModSettings, false)
                .SetColor(Color.red);
            Testy2 = BooleanOptionItem.Create(5, "Testy2", true, TabGroup.ModSettings, false)
                .SetParent(Testy1);
            IsLoaded = true;
        }
    }
}