using System.Collections.Generic;

namespace NakamaHiro.Client
{
    public sealed class RewardGuaranteedRange
    {
        public int Min { get; set; }
        public int? Max { get; set; }
    }

    public sealed class RewardEnergyModifier
    {
        public string Id { get; set; }
        public string Operator { get; set; }
        public RewardGuaranteedRange Value { get; set; }
        public RewardGuaranteedRange DurationSec { get; set; }
    }

    public sealed class RewardGuaranteed
    {
        public Dictionary<string, RewardGuaranteedRange> Currencies { get; set; }
        public Dictionary<string, RewardGuaranteedRange> Items { get; set; }
    }

    public sealed class RewardConfig
    {
        public RewardGuaranteed Guaranteed { get; set; }
        public Dictionary<string, RewardGuaranteedRange> Energies { get; set; }
        public List<RewardEnergyModifier> EnergyModifiers { get; set; }
    }
}
