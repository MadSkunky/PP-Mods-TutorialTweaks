using System;
using System.IO;
using System.Reflection;
using Harmony;

namespace MadSkunky.TutorialTweaks
{
    public class TutorialTweaks
    {
        internal static ModConfig Config;
        internal static string ModDirectory;
        internal static string ModName;
        internal static Version ModVersion;
        internal static HarmonyInstance Harmony;

        public static void HomeMod(Func<string, object, object> api)
        {
            // Read config and assign to field
            Config = api("config", null) as ModConfig ?? new ModConfig();

            // Get mod directory
            ModDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Build path for logger and initialize
            string LogPath = Path.Combine(ModDirectory, "TutorialTweaks.log");
            Logger.Initialize(LogPath, Config.Debug, ModDirectory, nameof(TutorialTweaks));

            // Build path for localization file and add new localizations
            string LocalizationPath = Path.Combine(ModDirectory, "Localization.csv");
            LocalizationPatches.AddLocalizationFromCSV(LocalizationPath);

            // Read info from mod_info.js
            object ModInfo = api("mod_info", null);
            ModName = ModInfo.GetType().GetField("Name").GetValue(ModInfo).ToString();
            ModVersion = (Version)ModInfo.GetType().GetField("Version").GetValue(ModInfo);

            // Apply reopsitory definition patches
            DefPatches.Apply();
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            Logger.Debug("Repository definitions patched");
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);

            // Harmony patches
            Harmony = HarmonyInstance.Create("MadSkunky.TutorialTweaks");
            Harmony.PatchAll();
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            Logger.Debug("Harmony patches applied");
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);

            _ = api("log verbose", "Mod Initialised.");
        }
    }
}
