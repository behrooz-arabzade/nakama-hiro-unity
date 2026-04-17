using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class MatchManagerGetRequest
    {
        public string MatchId { get; set; }
    }

    public sealed class MatchHistoryEntry
    {
        public string MatchId { get; set; }
        public string Status { get; set; }
        public double? EndedAt { get; set; }
        public JObject ResultSummary { get; set; }
    }

    public sealed class ManagedMatchRecord
    {
        public string MatchId { get; set; }
        public List<string> ParticipantUserIds { get; set; }
        public string Status { get; set; }
        public JObject Details { get; set; }
        public JObject Result { get; set; }
        public double CreatedAt { get; set; }
        public double UpdatedAt { get; set; }
    }

    public sealed class MatchManagerGetResponse
    {
        public ManagedMatchRecord Match { get; set; }
    }

    public sealed class MatchManagerMineResponse
    {
        public string CurrentMatchId { get; set; }
        public string ManagedMatchId { get; set; }
        public List<MatchHistoryEntry> History { get; set; }
    }

    public sealed class MatchManagerHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal MatchManagerHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<MatchManagerGetResponse> GetAsync(
            ISession session,
            MatchManagerGetRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<MatchManagerGetResponse>(session, HiroRpcIds.MatchManagerGet, request, cancellationToken);

        public Task<MatchManagerMineResponse> MineAsync(
            ISession session,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<MatchManagerMineResponse>(session, HiroRpcIds.MatchManagerMine, null, cancellationToken);
    }
}
