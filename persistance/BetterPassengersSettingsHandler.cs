using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace BetterPassenger
{
    public static class BetterPassengersSettingsHandler
    {
        private static BetterPassengerSettings settings;

        private static string settingDirectoryPath =
            $"{Application.persistentDataPath}\\Mod_properties\\passenger_profit";

        private static string settingFileName = "Settings.json";

        public static float currentIncomePassengerMultiplier { get; set; }

        public static void initialize()
        {
            Directory.CreateDirectory(settingDirectoryPath);
            if (File.Exists($"{settingDirectoryPath}\\{settingFileName}"))
            {
                loadSettings();
                
            }
            else
            {
                settings = new BetterPassengerSettings();
                saveSettings();
                updateSettings();
            }
        }

        public static void saveSettings()
        {
            var str = JsonConvert.SerializeObject(settings, (Formatting) 1);
            using (StreamWriter streamWriter =
                new StreamWriter($"{settingDirectoryPath}\\{settingFileName}"))
                streamWriter.WriteLine(str);
        }

        public static void loadSettings()
        {
            settings = JsonConvert.DeserializeObject<BetterPassengerSettings>(
                File.ReadAllText($"{settingDirectoryPath}\\{settingFileName}"));
        }

        public static void updateSettings(string saveName = null, float newMultiplier = 1f)
        {
            if (saveName == null) return;
            if (settings.SaveParameters.Any(k => k.SaveName == saveName))
            {
                var saveParam =
                    settings.SaveParameters.First(k => k.SaveName == saveName).IncomePassengerMultiplier =
                        newMultiplier;
            }
            else
            {
                settings.SaveParameters.Add(new BetterPassengerPerSaveSettings(saveName, newMultiplier));
            }

            currentIncomePassengerMultiplier = newMultiplier;
            saveSettings();
        }

        public static void addSave(string saveName)
        {
            if (settings.SaveParameters.Any(k => k.SaveName == saveName))
            {
                currentIncomePassengerMultiplier = settings.SaveParameters.First(k => k.SaveName == saveName).IncomePassengerMultiplier;
                return;
            }

            currentIncomePassengerMultiplier = 1f;
            settings.SaveParameters.Add(new BetterPassengerPerSaveSettings(saveName, settings.DefaultIncomePassengerMultiplier));
            saveSettings();
        }
    }
}