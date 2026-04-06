using Nakama;
using Newtonsoft.Json;

namespace NakamaHiro.Client
{
    /// <summary>
    /// Typed Hiro-style RPC client for the nakama-hiro TypeScript module.
    /// Contract: sibling repo <c>nakama-hiro/src/main.ts</c> and <c>src/features/*/register*Rpcs.ts</c>.
    /// </summary>
    public sealed class NakamaHiroClient
    {
        public NakamaHiroClient(IClient client, JsonSerializerSettings jsonSettings = null)
        {
            var invoker = new HiroRpcInvoker(client, jsonSettings);
            Achievements = new AchievementsHiroClient(invoker);
            Inventory = new InventoryHiroClient(invoker);
            Economy = new EconomyHiroClient(invoker);
            Auctions = new AuctionsHiroClient(invoker);
            Progression = new ProgressionHiroClient(invoker);
            Stats = new StatsHiroClient(invoker);
            Streaks = new StreaksHiroClient(invoker);
            Leaderboards = new LeaderboardsHiroClient(invoker);
            EventLeaderboards = new EventLeaderboardsHiroClient(invoker);
            Energy = new EnergyHiroClient(invoker);
            Challenges = new ChallengesHiroClient(invoker);
            Unlockables = new UnlockablesHiroClient(invoker);
        }

        public AchievementsHiroClient Achievements { get; }
        public InventoryHiroClient Inventory { get; }
        public EconomyHiroClient Economy { get; }
        public AuctionsHiroClient Auctions { get; }
        public ProgressionHiroClient Progression { get; }
        public StatsHiroClient Stats { get; }
        public StreaksHiroClient Streaks { get; }
        public LeaderboardsHiroClient Leaderboards { get; }
        public EventLeaderboardsHiroClient EventLeaderboards { get; }
        public EnergyHiroClient Energy { get; }
        public ChallengesHiroClient Challenges { get; }
        public UnlockablesHiroClient Unlockables { get; }
    }
}
