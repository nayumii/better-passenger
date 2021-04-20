using System.Collections.Generic;

namespace BetterPassenger
{
    public class BetterPassengerSettings
    {
        public float DefaultIncomePassengerMultiplier { get; set; } = 1;

        public IList<BetterPassengerPerSaveSettings> SaveParameters { get; set; } = new List<BetterPassengerPerSaveSettings>();
    }
}