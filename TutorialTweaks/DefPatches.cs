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
        // Get config, definition repository and shared data
        private static readonly ModConfig modConfig = TutorialTweaks.Config;
        private static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();
        private static readonly SharedData Shared = GameUtl.GameComponent<SharedData>();
        public static void ApplyJacobAsSniper()
        {
            // Get Jacobs definition for the 1st part of the tutorial
            TacCharacterDef Jacob1 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Jacob_Tutorial_TacCharacterDef"));
            // Set class related definition for actor view
            Jacob1.Data.ViewElementDef = Repo.GetAllDefs<ViewElementDef>().First(ved => ved.name.Contains("E_View [PX_Sniper_ActorViewDef]"));
            // Switch the given Assault ClassTagDef in Jacobs GameTags to Sniper (keep both ClassTagDefs would make him dual classed rigth from scratch)
            GameTagDef Sniper_CTD = Repo.GetAllDefs<GameTagDef>().First(gtd => gtd.name.Contains("Sniper_ClassTagDef"));
            for (int i = 0; i < Jacob1.Data.GameTags.Length; i++)
            {
                if (Jacob1.Data.GameTags[i].GetType() == Sniper_CTD.GetType())
                {
                    Jacob1.Data.GameTags[i] = Sniper_CTD;
                }
            }
            if (modConfig.GiveJacobAssaultRifleProficiency)
            {
                // Add Assault Rifle item tag to Jacobs GameTags = Jacob should get proficiency for AR (still shows 'not proficient')
                List<GameTagDef> JacobsGameTagsList = Jacob1.Data.GameTags.ToList();
                JacobsGameTagsList.Add(Repo.GetAllDefs<GameTagDef>().First(gtd => gtd.name.Contains("AssaultRifleItem_TagDef")));
                Jacob1.Data.GameTags = JacobsGameTagsList.ToArray();
            }
            // Creating new arrays for Abilities, BodypartItems (armor), EquipmentItems (ready slots) and InventoryItems (backpack)
            // -> Overwrite old sets completely
            Jacob1.Data.Abilites = new TacticalAbilityDef[] // abilities -> Class proficiency
            { 
                Repo.GetAllDefs<ClassProficiencyAbilityDef>().First(cpad => cpad.name.Contains("Sniper_ClassProficiency_AbilityDef"))//,
                //defRepository.GetAllDefs<TacticalAbilityDef>().First(cpad => cpad.name.Contains("GoodShot_AbilityDef"))
            };
            Jacob1.Data.BodypartItems = new ItemDef[] // Armour
            {
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("PX_Sniper_Helmet_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("PX_Sniper_Torso_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("PX_Sniper_Legs_ItemDef"))
            };
            WeaponDef ModifiedAres = PrepareModifiedAres(); // Modify new Ares
            WeaponDef JacobsAR = modConfig.GiveJacobModifiedAres ? // If configured then give Jacob the new Ares, else a normal one
                ModifiedAres :
                Repo.GetAllDefs<WeaponDef>().First(wd => wd.name.Contains("PX_AssaultRifle_WeaponDef"));
            Jacob1.Data.EquipmentItems = new ItemDef[] // Ready slots
            {
                JacobsAR,
                Repo.GetAllDefs<WeaponDef>().First(wd => wd.name.Contains("PX_SniperRifle_WeaponDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("Medkit_EquipmentDef"))
            };
            Jacob1.Data.InventoryItems = new ItemDef[] // Backpack
            {
                Jacob1.Data.EquipmentItems[0].CompatibleAmmunition[0],
                Jacob1.Data.EquipmentItems[1].CompatibleAmmunition[0]
            };
            // Get Jacobs definition for the 2nd and following parts of the tutorial
            TacCharacterDef Jacob2 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Jacob_Tutorial2_TacCharacterDef"));
            // Copy changes from Jabobs 1st to his 2nd definition
            Jacob2.Data.ViewElementDef = Jacob1.Data.ViewElementDef;
            Jacob2.Data.GameTags = Jacob1.Data.GameTags;
            Jacob2.Data.Abilites = Jacob1.Data.Abilites;
            Jacob2.Data.BodypartItems = Jacob1.Data.BodypartItems;
            Jacob2.Data.EquipmentItems = Jacob1.Data.EquipmentItems;
            Jacob2.Data.InventoryItems = Jacob1.Data.InventoryItems;

            // Logging
            LogTacCharDef(Jacob1);
            LogTacCharDef(Jacob2);
        }

        // Special 'tutorial' Ares AR, unused def in repo, modified into a new AR
        private static WeaponDef PrepareModifiedAres()
        {
            // SharedData Shared = GameUtl.GameComponent<SharedData>();
            WeaponDef AresTutorial = Repo.GetAllDefs<WeaponDef>().First(tad => tad.name.Contains("PX_AssaultRifle_Tutorial_WeaponDef"));
            WeaponDef AresGold = Repo.GetAllDefs<WeaponDef>().First(tad => tad.name.Contains("PX_AssaultRifle_Gold_WeaponDef"));
            WeaponDef AresDefault = Repo.GetAllDefs<WeaponDef>().First(tad => tad.name.Contains("PX_AssaultRifle_WeaponDef"));
            if (modConfig.ModifiedAresSettings.NoPenaltyWithoutProficiency)
            {
                GameTagDef AllClasses_CTD = Repo.GetAllDefs<GameTagDef>().First(gtd => gtd.name.Contains("AllClasses_ClassTagDef"));
                AresTutorial.Tags.Add(AllClasses_CTD);                    // Selectable without warning and for all classes
                AresTutorial.IncompetenceAccuracyMultiplier = (float)1.0; // No penalty when using without proficiency
            }
            AresTutorial.ViewElementDef.DisplayName1.LocalizationKey = "KEY_PX_MSTT_ASSAULT_RIFLE_NAME";
            AresTutorial.ViewElementDef.Description.LocalizationKey = "KEY_PX_MSTT_ASSAULT_RIFLE_DESCRIPTION";
            AresTutorial.ViewElementDef.SmallIcon = AresGold.ViewElementDef.SmallIcon;
            AresTutorial.ViewElementDef.LargeIcon = AresGold.ViewElementDef.LargeIcon;
            AresTutorial.ViewElementDef.DeselectIcon = AresGold.ViewElementDef.DeselectIcon;
            AresTutorial.ViewElementDef.InventoryIcon = AresGold.ViewElementDef.InventoryIcon;
            AresTutorial.SkinData = AresGold.SkinData;
            AresTutorial.HitPoints = AresDefault.HitPoints;
            AresTutorial.ChargesMax = AresDefault.ChargesMax;
            AresTutorial.ManufactureTech = modConfig.ModifiedAresSettings.ManufacturingValues[ConfigHelper.Tech];
            AresTutorial.ManufactureMaterials = modConfig.ModifiedAresSettings.ManufacturingValues[ConfigHelper.Mat];
            AresTutorial.ManufacturePointsCost = modConfig.ModifiedAresSettings.ManufacturingValues[ConfigHelper.TP];
            AresTutorial.CrateSpawnWeight = AresDefault.CrateSpawnWeight;
            AresTutorial.DestroyOnActorDeathPerc = AresDefault.DestroyOnActorDeathPerc;
            AresTutorial.FumblePerc = 0;
            AresTutorial.DamagePayload.DamageKeywords = ConfigHelper.DamageKeywords.FindAll(dkp => dkp.Value > 0);
            AresTutorial.DamagePayload.DamageValue = AresTutorial.DamagePayload.DamageKeywords.First(dkp => dkp.DamageKeywordDef is DamageKeywordDef).Value;
            AresTutorial.DamagePayload.StopOnFirstHit = false;
            AresTutorial.DamagePayload.AutoFireShotCount = modConfig.ModifiedAresSettings.BurstCount;
            AresTutorial.DamagePayload.ProjectilesPerShot = modConfig.ModifiedAresSettings.ProjectilesPerShot;
            AresTutorial.SpreadDegrees = 40.99f / modConfig.ModifiedAresSettings.EffectiveRange;
            AresTutorial.ReturnFirePerc = 100;
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            Logger.Debug($"[Repo] patch tutorial Ares => Type: {AresTutorial.GetType().Name}, DefName: {AresTutorial.name}");
            Logger.Debug($"                            |=> DisplayName1: {AresTutorial.ViewElementDef.DisplayName1.LocalizeEnglish()}");
            Logger.Debug($"                            |=> Description: {AresTutorial.ViewElementDef.Description.LocalizeEnglish()}");
            Logger.Debug($"                            |=> Category: {AresTutorial.ViewElementDef.Category.LocalizeEnglish()}");
            Logger.Debug($"                            |=> ManufactureTech: {AresTutorial.ManufactureTech}");
            Logger.Debug($"                            |=> ManufactureMaterials: {AresTutorial.ManufactureMaterials}");
            Logger.Debug($"                            |=> ManufacturePointsCost: {AresTutorial.ManufacturePointsCost}");
            Logger.Debug($"                            |=> GameTags:");
            foreach (GameTagDef gameTagDef in AresTutorial.Tags)
            {
                Logger.Debug($"                                |=> Type: {gameTagDef.GetType().Name}, DefName: {gameTagDef.name}");
            }
            Logger.Debug($"                            |=> Damage:");
            foreach (DamageKeywordPair damageKeyword in AresTutorial.DamagePayload.DamageKeywords)
            {
                Logger.Debug($"                                |=> Type: {damageKeyword.DamageKeywordDef.name}, Value: {damageKeyword.Value}");
            }
            Logger.Debug("----------------------------------------------------------------------------------------------------", false);
            Logger.Debug($"                            |=> Burst: {AresTutorial.DamagePayload.AutoFireShotCount}");
            Logger.Debug($"                            |=> ProjectilesPerShot: {AresTutorial.DamagePayload.ProjectilesPerShot}");
            Logger.Debug($"                            |=> SpreadDegrees: {AresTutorial.SpreadDegrees}");
            Logger.Debug($"                            |=> EffectiveRange: {41f/AresTutorial.SpreadDegrees}");
            return AresTutorial;
        }

        // Make the new modified Ares manufacturable
        public static void MakeModifiedAresManufacturable()
        {
            // Get definition for the tutorial Ares = modified new Ares
            WeaponDef AresTutorial = Repo.GetAllDefs<WeaponDef>().First(tad => tad.name.Contains("PX_AssaultRifle_Tutorial_WeaponDef"));
            // Mark it as manufacturable by adding the necessary tag
            if (!AresTutorial.Tags.Contains(Shared.SharedGameTags.ManufacturableTag))
            {
                AresTutorial.Tags.Add(Shared.SharedGameTags.ManufacturableTag);
            }
            // Adding the weapon manufacturing as reward for the 'PhoenixProject' research (right from game start) --- todo => make the research configurable
            // Converting existing reward array to list, adding maufacturable items, reapply to research def as new array
            ResearchDef researchDef = Repo.GetAllDefs<ResearchDef>().First(rd => rd.name.Contains("PX_PhoenixProject_ResearchDef"));
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
