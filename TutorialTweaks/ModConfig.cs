using System.Collections.Generic;
using Base.Core;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Tactical.Entities.DamageKeywords;

namespace MadSkunky.TutorialTweaks
{
    // Config class
    internal class ModConfig
    {
        // Config fields

        // If true Jacob get Assault Rifle proficiency
        public bool GiveJacobAssaultRifleProficiency = false;
        // If true Jacob get the new modified special Ares as main weapon (see below), otherwise a standard Ares AR-1
        public bool GiveJacobModifiedAres = false;
        // Settings for the new modified special Ares
        public WeaponSettings ModifiedAresSettings = new WeaponSettings(
                tech: 6f,
                materials: 95f,
                tp: 65f,
                damage: 35f,
                shred: 1f,
                burst: 4,
                pps: 1,
                er: 40f
            );

        // Set personal abilities of tutorial operatives
        public Dictionary<string, Dictionary<int, string>> SetPersonalAbilitiesFor = new Dictionary<string, Dictionary<int, string>> {
            {"Sophia", new Dictionary<int, string> {
                {1, "" },
                {2, "Self Defense Specialist" },
                {3, "" },
                {4, "Quarterback" },
                {5, "Thief" },
                {6, "" },
                {7, "" }
            }},
            {"Jacob", new Dictionary<int, string> {
                {1, "Trooper" },
                {2, "" },
                {3, "Cautious" },
                {4, "" },
                {5, "Thief" },
                {6, "" },
                {7, "" }
            }},
            {"Omar", new Dictionary<int, string> {
                {1, "" },
                {2, "" },
                {3, "Close Quarter Specialist" },
                {4, "Reckless" },
                {5, "" },
                {6, "" },
                {7, "Biochemist" }
            }},
            {"Irina", new Dictionary<int, string> {
                {1, "Farsighted" },
                {2, "Bombardier" },
                {3, "" },
                {4, "" },
                {5, "Healer" },
                {6, "" },
                {7, "" }
            }},
            {"Takeshi", new Dictionary<int, string> {
                {1, "" },
                {2, "Close Quarter Specialist" },
                {3, "Reckless" },
                {4, "" },
                {5, "" },
                {6, "Strongman" },
                {7, "" }
            }},
        };

        // DebugLevel (0: nothing, 1: error, 2: debug, 3: info)
        public int Debug = 1;

        internal struct WeaponSettings
        {
            public WeaponSettings(bool noPenalty = false,
                                  bool manufacurable = false,
                                  float tech = 5,
                                  float materials = 86,
                                  float tp = 58,
                                  float damage = 30,
                                  float shred = 1,
                                  float pierce = 0,
                                  float bleed = 0,
                                  float poison = 0,
                                  float viral = 0,
                                  float para = 0,
                                  float acid = 0,
                                  float fire = 0,
                                  float shock = 0,
                                  float sonic = 0,
                                  float syphon = 0,
                                  int burst = 6,
                                  int pps = 1,
                                  float er = 25)
            {
                NoPenaltyWithoutProficiency = noPenalty;
                Manufacturable = manufacurable;
                ManufacturingValues = new Dictionary<string, float>
                {
                    {ConfigHelper.Tech, tech },
                    {ConfigHelper.Mat, materials },
                    {ConfigHelper.TP, tp }
                };
                DamagetypeValues = new Dictionary<string, float>
                {
                    { ConfigHelper.Damage, damage },
                    { ConfigHelper.Shred, shred },
                    { ConfigHelper.Pierce, pierce },
                    { ConfigHelper.Bleed, bleed },
                    { ConfigHelper.Poison, poison },
                    { ConfigHelper.Viral, viral },
                    { ConfigHelper.Paralyse, para },
                    { ConfigHelper.Acid, acid },
                    { ConfigHelper.Fire, fire },
                    { ConfigHelper.Shock, shock },
                    { ConfigHelper.Sonic, sonic },
                    { ConfigHelper.Syphon, syphon }
                };
                BurstCount = burst;
                ProjectilesPerShot = pps;
                EffectiveRange = er;
            }

            public bool NoPenaltyWithoutProficiency;
            public bool Manufacturable;
            public Dictionary<string, float> ManufacturingValues;
            public Dictionary<string, float> DamagetypeValues;
            public int BurstCount;
            public int ProjectilesPerShot;
            public float EffectiveRange;
        }
    }

    internal class ConfigHelper
    {
        private static readonly Dictionary<string, float> _damagetypeValues = TutorialTweaks.Config.ModifiedAresSettings.DamagetypeValues;
        private static readonly SharedDamageKeywordsDataDef _sharedDamageKeywords = GameUtl.GameComponent<SharedData>().SharedDamageKeywords;

        public const string Tech = "Tech";
        public const string Mat = "Materials";
        public const string TP = "TimePoints";
        public const string Damage = "Damage";
        public const string Shred = "Shred";
        public const string Pierce = "Pierce";
        public const string Bleed = "Bleed";
        public const string Poison = "Poison";
        public const string Viral = "Viral";
        public const string Paralyse = "Paralyse";
        public const string Acid = "Acid";
        public const string Fire = "Fire";
        public const string Shock = "Shock";
        public const string Sonic = "Sonic";
        public const string Syphon = "Syphon";

        public static List<DamageKeywordPair> DamageKeywords = new List<DamageKeywordPair>
        {
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.DamageKeyword, Value = _damagetypeValues[Damage] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.ShreddingKeyword, Value = _damagetypeValues[Shred] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.PiercingKeyword, Value = _damagetypeValues[Pierce] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.BleedingKeyword, Value = _damagetypeValues[Bleed] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.PoisonousKeyword, Value = _damagetypeValues[Poison] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.ViralKeyword, Value = _damagetypeValues[Viral] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.ParalysingKeyword, Value = _damagetypeValues[Paralyse] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.AcidKeyword, Value = _damagetypeValues[Acid] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.BurningKeyword, Value = _damagetypeValues[Fire] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.ShockKeyword, Value = _damagetypeValues[Shock] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.SonicKeyword, Value = _damagetypeValues[Sonic] },
            new DamageKeywordPair{DamageKeywordDef = _sharedDamageKeywords.SyphonKeyword, Value = _damagetypeValues[Syphon] }
        };
    }
}
