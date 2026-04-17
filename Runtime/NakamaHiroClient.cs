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
        private readonly HiroRpcInvoker _invoker;

        public NakamaHiroClient(IClient client, JsonSerializerSettings jsonSettings = null)
        {
            _invoker = new HiroRpcInvoker(client, jsonSettings);
            Achievements = new AchievementsHiroClient(_invoker);
            Inventory = new InventoryHiroClient(_invoker);
            Economy = new EconomyHiroClient(_invoker);
            Auctions = new AuctionsHiroClient(_invoker);
            Progression = new ProgressionHiroClient(_invoker);
            Stats = new StatsHiroClient(_invoker);
            Streaks = new StreaksHiroClient(_invoker);
            Leaderboards = new LeaderboardsHiroClient(_invoker);
            EventLeaderboards = new EventLeaderboardsHiroClient(_invoker);
            Energy = new EnergyHiroClient(_invoker);
            Challenges = new ChallengesHiroClient(_invoker);
            Unlockables = new UnlockablesHiroClient(_invoker);
            MatchManager = new MatchManagerHiroClient(_invoker);
            ServerLoadBalancer = new ServerLoadBalancerHiroClient(_invoker);
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
        public MatchManagerHiroClient MatchManager { get; }
        public ServerLoadBalancerHiroClient ServerLoadBalancer { get; }

        /// <summary>
        /// Shared RPC invoker. Pass to game-specific *HiroClient constructors
        /// so they inherit the same IClient and JSON settings as the base clients.
        /// </summary>
        public HiroRpcInvoker Invoker => _invoker;
    }
}
