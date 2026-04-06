using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class InventoryCategoryFilter
    {
        public string ItemCategory { get; set; }
    }

    public sealed class InventoryCodexResponse
    {
        public List<JObject> Items { get; set; }
    }

    public sealed class InventoryOwnedResponse
    {
        public List<JObject> Inventory { get; set; }
    }

    public sealed class InventoryGrantRequest
    {
        public Dictionary<string, JToken> Items { get; set; }
        public bool? IgnoreLimits { get; set; }
    }

    public sealed class InventoryConsumeRequest
    {
        public Dictionary<string, JToken> Items { get; set; }
        public Dictionary<string, JToken> Instances { get; set; }
        public bool? Overconsume { get; set; }
    }

    public sealed class InventoryConsumeResponse
    {
        public bool Ok { get; set; }
        public Dictionary<string, double> Consumed { get; set; }
    }

    public sealed class ItemPropertyUpdate
    {
        public Dictionary<string, string> StringProperties { get; set; }
        public Dictionary<string, double> NumericProperties { get; set; }
    }

    public sealed class InventoryUpdateRequest
    {
        public Dictionary<string, ItemPropertyUpdate> ItemUpdates { get; set; }
    }

    public sealed class InventoryHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal InventoryHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<InventoryCodexResponse> ListAsync(
            ISession session,
            InventoryCategoryFilter filter = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<InventoryCodexResponse>(session, HiroRpcIds.InventoryList, filter, cancellationToken);

        public Task<InventoryOwnedResponse> ListOwnedAsync(
            ISession session,
            InventoryCategoryFilter filter = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<InventoryOwnedResponse>(session, HiroRpcIds.InventoryListOwned, filter, cancellationToken);

        public Task<InventoryOwnedResponse> GrantAsync(
            ISession session,
            InventoryGrantRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<InventoryOwnedResponse>(session, HiroRpcIds.InventoryGrant, request, cancellationToken);

        public Task<InventoryConsumeResponse> ConsumeAsync(
            ISession session,
            InventoryConsumeRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<InventoryConsumeResponse>(session, HiroRpcIds.InventoryConsume, request, cancellationToken);

        public Task<InventoryOwnedResponse> UpdateAsync(
            ISession session,
            InventoryUpdateRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<InventoryOwnedResponse>(session, HiroRpcIds.InventoryUpdate, request, cancellationToken);
    }
}
