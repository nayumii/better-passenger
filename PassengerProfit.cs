using HarmonyLib;
using VoxelTycoon;
using VoxelTycoon.Buildings;
using VoxelTycoon.Game.UI;
using VoxelTycoon.Modding;
using System.IO;
using UnityEngine;
using VoxelTycoon.Serialization;


namespace BetterPassenger
{
    public class PassengerProfit : Mod
    {
        private Harmony harmony;
        public static string savename;

        protected override void Initialize()
        {
            BetterPassengersSettingsHandler.initialize();
            BetterPassengersSettingsHandler.saveSettings();

            if (SaveManager.LoadingMetadata != null)
            {
                savename = SaveManager.LoadingMetadata.Filename;
                BetterPassengersSettingsHandler.addSave(savename);
            }

            harmony = new Harmony("com.betterpassenger.patch");
            harmony.PatchAll();
        }

        protected override void Deinitialize()
        {
            harmony.UnpatchAll();
        }
        
    }

    [HarmonyPatch(typeof(Company))]
    [HarmonyPatch("AddMoney")]
    public static class CompanyPassengerEarningPatch
    {
        static void Prefix(ref double value, BudgetItem budgetItem, bool important = true)
        {
            if (budgetItem == BudgetItem.PassengerTransportation)
            {
                value *= BetterPassengersSettingsHandler.currentIncomePassengerMultiplier;
            }
        }
    }

    [HarmonyPatch(typeof(House))]
    [HarmonyPatch("TryAcceptTransaction")]
    public static class TransactionPassengerEarningPatch
    {
        private static IStorageTransaction _transactionToModify;

        static void Prefix(NodeStoragePair source,
            NodeStoragePair target,
            ref IStorageTransaction transaction,
            bool preview)
        {
            _transactionToModify = transaction;
        }

        static void Postfix()
        {
            _transactionToModify.Price *= BetterPassengersSettingsHandler.currentIncomePassengerMultiplier;
        }
    }

    [HarmonyPatch(typeof(GameSettingsWindowDifficultyPage))]
    [HarmonyPatch("InitializeInternal")]
    public static class GameSettingsInWorld
    {
        private static SettingsControl _settingsControlToModify;

        static void Prefix(ref SettingsControl settingsControl)
        {
            _settingsControlToModify = settingsControl;
        }

        static void Postfix()
        {
            _settingsControlToModify.AddPercentSlider("Passenger income multiplier",
                "100% for vanilla Game", () => BetterPassengersSettingsHandler.currentIncomePassengerMultiplier,
                (value) =>
                {
                    BetterPassengersSettingsHandler.currentIncomePassengerMultiplier = value;
                    BetterPassengersSettingsHandler.updateSettings(PassengerProfit.savename, value);
                }, 10, 1000, 25
            );
        }
    }
}