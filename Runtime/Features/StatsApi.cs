using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class StatValueView
    {
        public double Value { get; set; }
    }

    public sealed class StatsSnapshot
    {
        public Dictionary<string, StatValueView> Public { get; set; }
        public Dictionary<string, StatValueView> Private { get; set; }
    }

    public sealed class StatUpdateItem
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public JToken Operator { get; set; }
    }

    public sealed class StatsUpdateRequest
    {
        public List<StatUpdateItem> Public { get; set; }
        public List<StatUpdateItem> Private { get; set; }
    }

    public sealed class StatsHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal StatsHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<StatsSnapshot> GetAsync(ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<StatsSnapshot>(session, HiroRpcIds.StatsGet, null, cancellationToken);

        public Task<StatsSnapshot> UpdateAsync(
            ISession session,
            StatsUpdateRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<StatsSnapshot>(session, HiroRpcIds.StatsUpdate, request, cancellationToken);
    }
}
