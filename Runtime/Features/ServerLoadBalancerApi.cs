using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class ServerLbRegionsListRequest
    {
        public string GameId { get; set; }
    }

    public sealed class ServerLbPublicRegionItem
    {
        public string Id { get; set; }
        public string GameId { get; set; }
        public string Region { get; set; }
        public int Weight { get; set; }
        public string Label { get; set; }
        public JObject Metadata { get; set; }
        public string UpdateTimeSec { get; set; }
    }

    public sealed class ServerLbRegionsListResponse
    {
        public List<ServerLbPublicRegionItem> Regions { get; set; }
    }

    public sealed class ServerLoadBalancerHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal ServerLoadBalancerHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<ServerLbRegionsListResponse> RegionsListAsync(
            ISession session,
            ServerLbRegionsListRequest request = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ServerLbRegionsListResponse>(
                session,
                HiroRpcIds.ServerLbRegionsList,
                request,
                cancellationToken);
    }
}
