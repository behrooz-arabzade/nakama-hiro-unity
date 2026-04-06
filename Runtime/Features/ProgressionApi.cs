using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class ProgressionsGetFilter
    {
        public string Category { get; set; }
    }

    public sealed class ProgressionsListResponse
    {
        public List<JObject> Progressions { get; set; }
    }

    public sealed class ProgressionsPurchaseRequest
    {
        public string Id { get; set; }
    }

    public sealed class ProgressionsUpdateRequest
    {
        public string Id { get; set; }
        public Dictionary<string, double> Counts { get; set; }
    }

    public sealed class ProgressionsResetRequest
    {
        public List<string> Ids { get; set; }
    }

    public sealed class ProgressionHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal ProgressionHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<ProgressionsListResponse> GetAsync(
            ISession session,
            ProgressionsGetFilter filter = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ProgressionsListResponse>(session, HiroRpcIds.ProgressionsGet, filter, cancellationToken);

        public Task<ProgressionsListResponse> PurchaseAsync(
            ISession session,
            ProgressionsPurchaseRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ProgressionsListResponse>(session, HiroRpcIds.ProgressionsPurchase, request, cancellationToken);

        public Task<ProgressionsListResponse> UpdateAsync(
            ISession session,
            ProgressionsUpdateRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ProgressionsListResponse>(session, HiroRpcIds.ProgressionsUpdate, request, cancellationToken);

        public Task<ProgressionsListResponse> ResetAsync(
            ISession session,
            ProgressionsResetRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ProgressionsListResponse>(session, HiroRpcIds.ProgressionsReset, request, cancellationToken);
    }
}
