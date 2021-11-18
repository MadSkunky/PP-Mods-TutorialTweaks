using I2.Loc;
using System;
using System.IO;
using System.Linq;

namespace MadSkunky.TutorialTweaks
{
    class LocalizationPatches
    {
        public static void AddLocalizationFromCSV(string LocalizationPath, string Category = null)
        {
            try
            {
                string CSVstring = File.ReadAllText(LocalizationPath);
                LanguageSourceData SourceToChange = Category == null ? // if category is not given
                    LocalizationManager.Sources[0] :                   // use fist language source
                    LocalizationManager.Sources.First(source => source.GetCategories().Contains(Category));
                //LanguageSourceData SourceToChange = LocalizationManager.Sources.First(source => source.GetCategories().Contains(Category));
                if (SourceToChange != null)
                {
                    _ = SourceToChange.Import_CSV(string.Empty, CSVstring, eSpreadsheetUpdateMode.AddNewTerms, ',');
                    LocalizationManager.LocalizeAll(true);    // Force localing all enabled labels/sprites with the new data
                    Logger.Debug("----------------------------------------------------------------------------------------------------", false);
                    Logger.Debug($"Localization data from {LocalizationPath} in localization source added.");
                    Logger.Debug("CSV Data:" + Environment.NewLine + CSVstring);
                    Logger.Debug("----------------------------------------------------------------------------------------------------", false);
                }
                else
                {
                    Logger.Debug("----------------------------------------------------------------------------------------------------", false);
                    Logger.Debug($"No language source with category {Category} found!");
                    Logger.Debug("----------------------------------------------------------------------------------------------------", false);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
