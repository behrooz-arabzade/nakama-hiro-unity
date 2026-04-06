using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class StreakRewardConfigDto
    {
        public double CountMin { get; set; }
        public double CountMax { get; set; }
        public RewardConfig Reward { get; set; }
    }

    public sealed class StreakRuntimeView
    {
        public double CurrentCount { get; set; }
        public double AddedCountThisResetPeriod { get; set; }
        public double? LastProcessedResetBoundarySec { get; set; }
        public double? LastSuccessfulUpdateLogicalSec { get; set; }
        public List<string> ClaimedRewardTierKeys { get; set; }
    }

    public sealed class StreakListEntryView
    {
        public string StreakId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double MaxCount { get; set; }
        public double MaxCountCurrentReset { get; set; }
        public string ResetCronexpr { get; set; }
        public List<StreakRewardConfigDto> Rewards { get; set; }
        public bool Active { get; set; }
        public string InactiveReason { get; set; }
        public StreakRuntimeView Runtime { get; set; }
    }

    public sealed class StreaksListResponse
    {
        public double CurrentTimeSec { get; set; }
        public List<StreakListEntryView> Streaks { get; set; }
    }

    public sealed class StreaksUpdateRequest
    {
        public Dictionary<string, JToken> Ids { get; set; }
    }

    public sealed class StreaksClaimRequest
    {
        public List<string> Ids { get; set; }
    }

    public sealed class StreaksResetRequest
    {
        public List<string> Ids { get; set; }
    }

    public sealed class StreaksHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal StreaksHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<StreaksListResponse> ListAsync(ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<StreaksListResponse>(session, HiroRpcIds.StreaksList, null, cancellationToken);

        public Task<StreaksListResponse> UpdateAsync(
            ISession session,
            StreaksUpdateRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<StreaksListResponse>(session, HiroRpcIds.StreaksUpdate, request, cancellationToken);

        public Task<StreaksListResponse> ClaimAsync(
            ISession session,
            StreaksClaimRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<StreaksListResponse>(session, HiroRpcIds.StreaksClaim, request, cancellationToken);

        public Task<StreaksListResponse> ResetAsync(
            ISession session,
            StreaksResetRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<StreaksListResponse>(session, HiroRpcIds.StreaksReset, request, cancellationToken);
    }
}
