using System;
using System.Linq;
using System.Collections.Generic;
using Harmony;
using Base.Build;
using Base.Defs;
using Base.Core;
using UnityEngine;
using PhoenixPoint.Home.View.ViewModules;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.Characters;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;

namespace MadSkunky.TutorialTweaks
{
    class HarmonyPatches
    {
        // Get config, definition repository and shared data
        private static readonly ModConfig modConfig = TutorialTweaks.Config;
        private static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();
        private static readonly SharedData Shared = GameUtl.GameComponent<SharedData>();
        // This "tag" allows Harmony to find this class and apply it as a patch.
        [HarmonyPatch(typeof(UIModuleBuildRevision), "SetRevisionNumber")]
        // Class can be any name, but must be static.
        internal static class UIModuleBuildRevision_SetRevisionNumber
        {

            // Rewrite to use configured value. User may set it to null.
            private static void Postfix(UIModuleBuildRevision __instance)
            {
                __instance.BuildRevisionNumber.text = $"{RuntimeBuildInfo.UserVersion} w/MSTT v{TutorialTweaks.ModVersion} ";
            }
        }

        // Make personal abilities for tutorial characters cogfigurable => GeoscapeTutorial:InitSquad
        [HarmonyPatch(typeof(GeoscapeTutorial), "InitSquad")]
        internal static class GeoscapeTutorial_InitSquad
        {
            private static void Postfix(ref GeoLevelController ____level)
            {
                try
                {
                    string PersonalAbilities; // for logging
                    foreach (GeoCharacter geoCharacter in ____level.PhoenixFaction.Soldiers)
                    {
                        if (PersonalAbilitiesMap().ContainsKey(geoCharacter.DisplayName))
                        {
                            Dictionary<int, TacticalAbilityDef> newPersonalAbilities = PersonalAbilitiesMap()[geoCharacter.DisplayName];
                            AbilityTrackSlot[] abilityTrack = AbilityTrack.CreateFromDictionary(7, newPersonalAbilities, AbilityTrackSource.Personal).AbilitiesByLevel;
                            geoCharacter.Progression.PersonalAbilityTrack.AbilitiesByLevel = abilityTrack;
                            PersonalAbilities = "";
                            foreach (AbilityTrackSlot ats in geoCharacter.Progression.PersonalAbilityTrack.AbilitiesByLevel)
                            {
                                PersonalAbilities = PersonalAbilities + "[" + ats.Ability + "] ";
                            }
                            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
                            Logger.Debug($"'{geoCharacter.DisplayName}' personal abilities -");
                            Logger.Debug("New Set : " + PersonalAbilities);
                            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        // Create and return dictionary from config with game stable values
        internal static Dictionary<string, Dictionary<int, TacticalAbilityDef>> PersonalAbilitiesMap()
        {
            // Resulting dictionary
            Dictionary<string, Dictionary<int, TacticalAbilityDef>> result = new Dictionary<string, Dictionary<int, TacticalAbilityDef>>();
            // Helpers
            int level;
            string compString;
            TacticalAbilityDef tacticalAbilityDef;
            // Outer loop, get stable names of tutorial soldiers
            foreach (KeyValuePair<string, Dictionary<int, string>> nameAbilitiesPair in modConfig.SetPersonalAbilitiesFor)
            {
                // Get name, first letter is enough, they all start with different letters
                string name = TutorialSoldierNameMap.First(kvp => kvp.Key.ToLower().StartsWith(nameAbilitiesPair.Key.ToLower().Substring(0, 1))).Value;
                if (name != null && !result.Keys.Contains(name))
                {
                    result.Add(name, new Dictionary<int, TacticalAbilityDef>());
                    // Inner loop, get stable TacticalAbilityDef's
                    foreach (KeyValuePair<int, string> levelAbilityPair in modConfig.SetPersonalAbilitiesFor[nameAbilitiesPair.Key])
                    {
                        level = levelAbilityPair.Key;
                        compString = levelAbilityPair.Value.ToLower().Substring(0, 3);
                        // Only numbers between 1 and 7 allowed (7 levels), if out of this range and ability is not in ability map => skip
                        if (level > 0 && level < 8 && TacticalAbilityMap.Keys.Any(key => key.ToLower().StartsWith(compString)))
                        {
                            // Get tactical ability def from map, first 3 lowercase letters to find
                            tacticalAbilityDef = TacticalAbilityMap.First(kvp => kvp.Key.ToLower().StartsWith(compString)).Value;
                            if (!result[name].ContainsKey(level) && !result[name].ContainsValue(tacticalAbilityDef))
                            {
                                // Add level and ability definition to result, level -1 because level index goes from 0-6 for 7 levels
                                result[name].Add(level - 1, tacticalAbilityDef);
                            }
                        }
                    }
                    // if nothig useful found then remove the entry => no changes to abilities
                    if (result[name] == null)
                    {
                        result.Remove(name);
                    }
                }
            }
            return result;
        }
        // Internal helper dictionaries
        internal static Dictionary<string, TacticalAbilityDef> TacticalAbilityMap = new Dictionary<string, TacticalAbilityDef> {
            {"Biochemist", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("BioChemist_AbilityDef")) },
            {"Farsighted", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Brainiac_AbilityDef")) },
            {"Cautious", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Cautious_AbilityDef")) },
            {"Close Quarter Specialist", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("CloseQuartersSpecialist_AbilityDef")) },
            {"Bombardier", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Crafty_AbilityDef")) },
            {"Sniperist", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Focused_AbilityDef")) },
            {"Trooper", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("GoodShot_AbilityDef")) },
            {"Healer", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Helpful_AbilityDef")) },
            {"Quarterback", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Pitcher_AbilityDef")) },
            {"Reckless", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Reckless_AbilityDef")) },
            {"Resourceful", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Resourceful_AbilityDef")) },
            {"Self Defense Specialist", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("SelfDefenseSpecialist_AbilityDef")) },
            {"Strongman", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Strongman_AbilityDef")) },
            {"Thief", Repo.GetAllDefs<TacticalAbilityDef>().First(tac => tac.name.Contains("Thief_AbilityDef")) }
        };
        internal static Dictionary<string, string> TutorialSoldierNameMap = new Dictionary<string, string> {
            {"Sophia", "Sophia Brown" },
            {"Jacob", "Jacob Eber" },
            {"Omar", "Omar Ashour" },
            {"Irina", "Irina Sokolova" },
            {"Takeshi", "Takeshi Sato" }
        };
    }
}
