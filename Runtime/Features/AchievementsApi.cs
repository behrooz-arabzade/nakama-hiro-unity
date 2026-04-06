using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class AchievementsGetFilter
    {
        public string Category { get; set; }
        public bool? AvailableOnly { get; set; }
        public bool? RepeatOnly { get; set; }
    }

    public sealed class AchievementsListResponse
    {
        public List<JObject> Achievements { get; set; }
        public List<JObject> RepeatAchievements { get; set; }
    }

    public sealed class AchievementsUpdateRequest
    {
        public Dictionary<string, long> Updates { get; set; }
        public List<string> Ids { get; set; }
        public long? Amount { get; set; }
    }

    public sealed class AchievementsClaimRequest
    {
        public List<string> Ids { get; set; }
        public bool? ClaimTotalReward { get; set; }
    }

    public sealed class AchievementsHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal AchievementsHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<AchievementsListResponse> GetAsync(
            ISession session,
            AchievementsGetFilter filter = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AchievementsListResponse>(session, HiroRpcIds.AchievementsGet, filter, cancellationToken);

        public Task<AchievementsListResponse> UpdateAsync(
            ISession session,
            AchievementsUpdateRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AchievementsListResponse>(session, HiroRpcIds.AchievementsUpdate, request, cancellationToken);

        public Task<AchievementsListResponse> ClaimAsync(
            ISession session,
            AchievementsClaimRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AchievementsListResponse>(session, HiroRpcIds.AchievementsClaim, request, cancellationToken);
    }
}
