using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class EventLeaderboardListRequest
    {
        public List<string> Categories { get; set; }
        public bool? WithScores { get; set; }
    }

    public sealed class EventLeaderboardGetRequest
    {
        public string Id { get; set; }
        public bool? WithScores { get; set; }
    }

    public sealed class EventLeaderboardUpdateRequest
    {
        public string Id { get; set; }
        public double Score { get; set; }
        public double? Subscore { get; set; }
        public JObject Metadata { get; set; }
        public string Operator { get; set; }
    }

    public sealed class EventLeaderboardIdRequest
    {
        public string Id { get; set; }
    }

    public sealed class EventLeaderboardRollRequest
    {
        public string Id { get; set; }
        public bool? ClaimReward { get; set; }
    }

    public sealed class EventLeaderboardRecordView
    {
        public string OwnerId { get; set; }
        public string Username { get; set; }
        public double Score { get; set; }
        public double Subscore { get; set; }
        public double NumScore { get; set; }
        public JObject Metadata { get; set; }
        public string Rank { get; set; }
        public string UpdateTimeSec { get; set; }
    }

    public sealed class EventLeaderboardView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool Ascending { get; set; }
        public string Operator { get; set; }
        public string BestRank { get; set; }
        public bool CanClaim { get; set; }
        public bool CanRoll { get; set; }
        public bool CanRollWithClaim { get; set; }
        public bool CanUpdate { get; set; }
        public bool IsActive { get; set; }
        public string MaxRolls { get; set; }
        public bool RerollCountFreeze { get; set; }
        public string RollCooldownSec { get; set; }
        public string Rolls { get; set; }
        public string ScoreTarget { get; set; }
        public string ScoreTargetPlayers { get; set; }
        public double Tier { get; set; }
        public string TierDeltaPerPhase { get; set; }
        public string ClaimTimeSec { get; set; }
        public string EndTimeSec { get; set; }
        public string ExpiryTimeSec { get; set; }
        public string RollCooldownEndsSec { get; set; }
        public string ScoreTimeLimitEndsSec { get; set; }
        public string ScoreTimeLimitSec { get; set; }
        public string StartTimeSec { get; set; }
        public List<EventLeaderboardRecordView> Records { get; set; }
        public string NextCursor { get; set; }
    }

    public sealed class EventLeaderboardListResponse
    {
        public List<EventLeaderboardView> EventLeaderboards { get; set; }
    }

    public sealed class EventLeaderboardsHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal EventLeaderboardsHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<EventLeaderboardListResponse> ListAsync(
            ISession session,
            EventLeaderboardListRequest request = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EventLeaderboardListResponse>(session, HiroRpcIds.EventLeaderboardList, request, cancellationToken);

        public Task<EventLeaderboardView> GetAsync(
            ISession session,
            EventLeaderboardGetRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EventLeaderboardView>(session, HiroRpcIds.EventLeaderboardGet, request, cancellationToken);

        public Task<EventLeaderboardView> UpdateAsync(
            ISession session,
            EventLeaderboardUpdateRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EventLeaderboardView>(session, HiroRpcIds.EventLeaderboardUpdate, request, cancellationToken);

        public Task<EventLeaderboardView> ClaimAsync(
            ISession session,
            EventLeaderboardIdRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EventLeaderboardView>(session, HiroRpcIds.EventLeaderboardClaim, request, cancellationToken);

        public Task<EventLeaderboardView> RollAsync(
            ISession session,
            EventLeaderboardRollRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EventLeaderboardView>(session, HiroRpcIds.EventLeaderboardRoll, request, cancellationToken);
    }
}
