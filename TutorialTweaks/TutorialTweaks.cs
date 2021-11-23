using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Base.Core;
using Base.Defs;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Tactical.Entities.Abilities;

namespace MadSkunky.TutorialTweaks
{
    public class TutorialTweaks
    {
        internal static ModConfig Config;
        internal static string ModDirectory;
        internal static string ModName;
        internal static Version ModVersion;
        internal static HarmonyInstance Harmony;
        private static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();
        private static readonly SharedData Shared = GameUtl.GameComponent<SharedData>();

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
            DefPatches.ApplyJacobAsSniper();
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            Logger.Debug("HomeMod: Repository definitions patched");
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);

            // Harmony patches
            Harmony = HarmonyInstance.Create("MadSkunky.TutorialTweaks");
            Harmony.PatchAll();
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            Logger.Debug("HomeMod: Harmony patches applied");
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);

            _ = api("log verbose", "Mod Initialised.");
        }
        public static void GeoscapeMod()
        {
            if (Config.ModifiedAresSettings.Manufacturable)
            {
                DefPatches.MakeModifiedAresManufacturable();
            }
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            Logger.Debug("GeoscapeMod: Repository definitions patched");
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
        }
    }
}
