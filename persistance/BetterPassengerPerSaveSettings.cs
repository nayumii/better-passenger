namespace BetterPassenger
{
    public class BetterPassengerPerSaveSettings
    {
        public string SaveName { get; set; }

        public float IncomePassengerMultiplier { get; set; }

        public BetterPassengerPerSaveSettings(string saveName, float incomePassengerMultiplier)
        {
            SaveName = saveName;
            IncomePassengerMultiplier = incomePassengerMultiplier;
        }
    }
}