using System.Linq;
using Base.Core;
using Base.Defs;
using PhoenixPoint.Common.UI;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Common.Entities.Items;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Weapons;
using PhoenixPoint.Tactical.Entities.DamageKeywords;
using PhoenixPoint.Geoscape.Entities.Research;
using PhoenixPoint.Geoscape.Entities.Research.Reward;
using System.Collections.Generic;
using PhoenixPoint.Common.Core;

namespace MadSkunky.TutorialTweaks
{
    class DefPatches
    {
        // Get definition repository and shared data
        private static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();
        private static readonly SharedData Shared = GameUtl.GameComponent<SharedData>();
        public static void ApplyJacobAsSniper()
        {
            // Get Jacobs definition for the 1st part of the tutorial
            TacCharacterDef Jacob1 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Jacob_Tutorial_TacCharacterDef"));
            // Set class related definitions
            Jacob1.Data.ViewElementDef = Repo.GetAllDefs<ViewElementDef>().First(ved => ved.name.Contains("E_View [PX_Sniper_ActorViewDef]"));
            GameTagDef Sniper_CTD = Repo.GetAllDefs<GameTagDef>().First(gtd => gtd.name.Contains("Sniper_ClassTagDef"));
            for (int i = 0; i < Jacob1.Data.GameTags.Length; i++) // for safety, only change the right postition in the array for game tags
            {
                if (Jacob1.Data.GameTags[i].GetType() == Sniper_CTD.GetType())
                {
                    Jacob1.Data.GameTags[i] = Sniper_CTD;
                }
            }
            // creating new arrays for abilities, armor, equipment and inventory -> overwrite old sets completely
            Jacob1.Data.Abilites = new TacticalAbilityDef[] // abilities
            { 
                Repo.GetAllDefs<ClassProficiencyAbilityDef>().First(cpad => cpad.name.Contains("Sniper_ClassProficiency_AbilityDef"))//,
                //defRepository.GetAllDefs<TacticalAbilityDef>().First(cpad => cpad.name.Contains("GoodShot_AbilityDef"))
            };
            Jacob1.Data.BodypartItems = new ItemDef[] //armour
            {
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("PX_Sniper_Helmet_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("PX_Sniper_Torso_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("PX_Sniper_Legs_ItemDef"))
            };
            
            WeaponDef JacobsAR = TutorialTweaks.Config.UseNewSpecialAresForJacob ?
                PrepareTutorialAres(Repo) :
                Repo.GetAllDefs<WeaponDef>().First(wd => wd.name.Contains("PX_AssaultRifle_WeaponDef"));
            Jacob1.Data.EquipmentItems = new ItemDef[] // equipment = ready slots
            {
                JacobsAR,
                Repo.GetAllDefs<WeaponDef>().First(wd => wd.name.Contains("PX_SniperRifle_WeaponDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("Medkit_EquipmentDef"))
            };
            Jacob1.Data.InventoryItems = new ItemDef[] // inventory = backpack
            {
                Jacob1.Data.EquipmentItems[0].CompatibleAmmunition[0],
                Jacob1.Data.EquipmentItems[1].CompatibleAmmunition[0]
            };
            // Get Jacobs definition for the 2nd and following parts of the tutorial
            TacCharacterDef Jacob2 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Jacob_Tutorial2_TacCharacterDef"));
            // copy changes from above
            Jacob2.Data.ViewElementDef = Jacob1.Data.ViewElementDef;
            Jacob2.Data.GameTags = Jacob1.Data.GameTags;
            Jacob2.Data.Abilites = Jacob1.Data.Abilites;
            Jacob2.Data.BodypartItems = Jacob1.Data.BodypartItems;
            Jacob2.Data.EquipmentItems = Jacob1.Data.EquipmentItems;
            Jacob2.Data.InventoryItems = Jacob1.Data.InventoryItems;

            // logging
            LogTacCharDef(Jacob1);
            LogTacCharDef(Jacob2);
        }
        // special 'tutorial' Ares AR, unused def in repo, modified for Jacob
        private static WeaponDef PrepareTutorialAres(DefRepository Repo)
        {
            ModConfig modConfig = TutorialTweaks.Config;
            //SharedData Shared = GameUtl.GameComponent<SharedData>();
            WeaponDef AresTutorial = Repo.GetAllDefs<WeaponDef>().First(tad => tad.name.Contains("PX_AssaultRifle_Tutorial_WeaponDef"));
            WeaponDef AresGold = Repo.GetAllDefs<WeaponDef>().First(tad => tad.name.Contains("PX_AssaultRifle_Gold_WeaponDef"));
            WeaponDef AresDefault = Repo.GetAllDefs<WeaponDef>().First(tad => tad.name.Contains("PX_AssaultRifle_WeaponDef"));
            GameTagDef AllClasses_CTD = Repo.GetAllDefs<GameTagDef>().First(gtd => gtd.name.Contains("AllClasses_ClassTagDef"));
            AresTutorial.Tags.Add(AllClasses_CTD);
            AresTutorial.ViewElementDef.DisplayName1.LocalizationKey = "KEY_PX_MSTT_ASSAULT_RIFLE_NAME";
            AresTutorial.ViewElementDef.Description.LocalizationKey = "KEY_PX_MSTT_ASSAULT_RIFLE_DESCRIPTION";
            AresTutorial.ViewElementDef.SmallIcon = AresGold.ViewElementDef.SmallIcon;
            AresTutorial.ViewElementDef.LargeIcon = AresGold.ViewElementDef.LargeIcon;
            AresTutorial.ViewElementDef.DeselectIcon = AresGold.ViewElementDef.DeselectIcon;
            AresTutorial.ViewElementDef.InventoryIcon = AresGold.ViewElementDef.InventoryIcon;
            AresTutorial.SkinData = AresGold.SkinData;
            AresTutorial.HitPoints = AresDefault.HitPoints;
            AresTutorial.ChargesMax = AresDefault.ChargesMax;
            AresTutorial.ManufactureTech = modConfig.NewSpecialAresSettings.ManufacturingCost.Tech;
            AresTutorial.ManufactureMaterials = modConfig.NewSpecialAresSettings.ManufacturingCost.Materials;
            AresTutorial.ManufacturePointsCost = modConfig.NewSpecialAresSettings.ManufacturingCost.TimePoints;
            AresTutorial.CrateSpawnWeight = AresDefault.CrateSpawnWeight;
            AresTutorial.DestroyOnActorDeathPerc = AresDefault.DestroyOnActorDeathPerc;
            AresTutorial.FumblePerc = 0;
            AresTutorial.DamagePayload.DamageKeywords[0].Value = modConfig.NewSpecialAresSettings.Damage;
            if (modConfig.NewSpecialAresSettings.Shred > 0)
            {
                AresTutorial.DamagePayload.DamageKeywords.Add(new DamageKeywordPair {
                    DamageKeywordDef = Shared.SharedDamageKeywords.ShreddingKeyword,
                    Value = modConfig.NewSpecialAresSettings.Shred
                });
            }

            if (modConfig.NewSpecialAresSettings.Pierce > 0)
            {
                AresTutorial.DamagePayload.DamageKeywords.Add(new DamageKeywordPair {
                    DamageKeywordDef = Shared.SharedDamageKeywords.PiercingKeyword,
                    Value = modConfig.NewSpecialAresSettings.Pierce
                });
            }
            AresTutorial.DamagePayload.DamageValue = AresTutorial.DamagePayload.DamageKeywords[0].Value;
            AresTutorial.DamagePayload.StopOnFirstHit = false;
            AresTutorial.DamagePayload.AutoFireShotCount = modConfig.NewSpecialAresSettings.BurstCount;
            AresTutorial.SpreadDegrees = (float)41 / modConfig.NewSpecialAresSettings.EffectiveRange;
            AresTutorial.ReturnFirePerc = 100;
            AresTutorial.IncompetenceAccuracyMultiplier = (float)1.0; // no penalty when using without proficiency
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            Logger.Debug($"[Repo] patch tutorial Ares => Type: {AresTutorial.GetType().Name}, DefName: {AresTutorial.name}");
            Logger.Debug($"                            |=> DisplayName1: {AresTutorial.ViewElementDef.DisplayName1.LocalizeEnglish()}");
            Logger.Debug($"                            |=> Description: {AresTutorial.ViewElementDef.Description.LocalizeEnglish()}");
            Logger.Debug($"                            |=> Category: {AresTutorial.ViewElementDef.Category.LocalizeEnglish()}");
            foreach (GameTagDef GTD in AresTutorial.Tags)
            {
                Logger.Debug($"                GameTagDef  |=> Type: {GTD.GetType().Name}, DefName: {GTD.name}");
            }
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            return AresTutorial;
        }

        // Make special 'tutorial' Ares manufacturable
        public static void ApllyManufacturableTutAres()
        {
            // Get needed repo defs
            WeaponDef AresTutorial = Repo.GetAllDefs<WeaponDef>().First(tad => tad.name.Contains("PX_AssaultRifle_Tutorial_WeaponDef"));
            // Mark items as manufacturable
            if (!AresTutorial.Tags.Contains(Shared.SharedGameTags.ManufacturableTag))
            {
                AresTutorial.Tags.Add(Shared.SharedGameTags.ManufacturableTag);
            }
            // Adding the weapon manufacturing as reward for the 'Atmospheric Analysis' research
            // Converting existing reward array to list, adding maufacturable items, reapply to research def as new array
            ResearchDef researchDef = Repo.GetAllDefs<ResearchDef>().First(rd => rd.name.Contains("PX_AtmosphericAnalysis_ResearchDef"));
            List<ResearchRewardDef> rewardDefs = researchDef.Unlocks.ToList();
            rewardDefs.Add(new ManufactureResearchRewardDef() { Items = new ItemDef[] { AresTutorial } });
            researchDef.Unlocks = rewardDefs.ToArray();
        }

        private static void LogTacCharDef(TacCharacterDef TCD)
        {
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            Logger.Debug($"[Repo] patch Tactical Character => Type: {TCD.GetType().Name}, DefName: {TCD.name}, GameName: {TCD.Data.ViewElementDef.Name}");
            Logger.Debug($"[Repo]               Actor view  |=> Type: {TCD.Data.ViewElementDef.GetType().Name}, DefName: {TCD.Data.ViewElementDef.name}, GameName: {TCD.Data.ViewElementDef.Name}");
            foreach (GameTagDef GTD in TCD.Data.GameTags)
            {
                Logger.Debug($"[Repo]                 Game tag  |=> Type: {GTD.GetType().Name}, DefName: {GTD.name}");
            }
            foreach (TacticalAbilityDef TAD in TCD.Data.Abilites)
            {
                Logger.Debug($"[Repo]         Tactical ability  |=> Type: {TAD.GetType().Name}, DefName: {TAD.name}, GameName: {TAD.ViewElementDef.Name}");
            }
            foreach (ItemDef ID in TCD.Data.BodypartItems)
            {
                Logger.Debug($"[Repo]            Bodypart item  |=> Type: {ID.GetType().Name}, DefName: {ID.name}, GameName: {ID.ViewElementDef.Name}");
            }
            foreach (ItemDef ID in TCD.Data.EquipmentItems)
            {
                Logger.Debug($"[Repo]           Equipment item  |=> Type: {ID.GetType().Name}, DefName: {ID.name}, GameName: {ID.ViewElementDef.Name}");
            }
            foreach (ItemDef ID in TCD.Data.InventoryItems)
            {
                Logger.Debug($"[Repo]           Inventory item  |=> Type: {ID.GetType().Name}, DefName: {ID.name}, GameName: {ID.ViewElementDef.Name}");
            }
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
        }
    }
}
