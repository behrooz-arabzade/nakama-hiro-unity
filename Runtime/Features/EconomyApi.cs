using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;

namespace NakamaHiro.Client
{
    public sealed class EconomyStoreItemCost
    {
        public Dictionary<string, double> Currencies { get; set; }
        public string Sku { get; set; }
    }

    public sealed class EconomyListStoreItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public EconomyStoreItemCost Cost { get; set; }
        public RewardConfig Reward { get; set; }
        public Dictionary<string, string> AdditionalProperties { get; set; }
        public bool Unavailable { get; set; }
    }

    public sealed class EconomyListPlacement
    {
        public string Id { get; set; }
        public RewardConfig Reward { get; set; }
    }

    public sealed class EconomyStoreGetResponse
    {
        public List<EconomyListStoreItem> StoreItems { get; set; }
        public List<EconomyListPlacement> Placements { get; set; }
        public Dictionary<string, double> Wallet { get; set; }
        public double CurrentTimeSec { get; set; }
    }

    public sealed class EconomyWalletGetResponse
    {
        public Dictionary<string, double> Wallet { get; set; }
        public double CurrentTimeSec { get; set; }
    }

    public sealed class EconomyGrantRequest
    {
        public Dictionary<string, double> Currencies { get; set; }
        public Dictionary<string, double> Items { get; set; }
    }

    public sealed class EconomyGrantResponse
    {
        public Dictionary<string, double> Wallet { get; set; }
        public double CurrentTimeSec { get; set; }
    }

    public sealed class EconomyPurchaseIntentRequest
    {
        public string ItemId { get; set; }
        public string Sku { get; set; }
    }

    public sealed class EconomyPurchaseIntentResponse
    {
        public bool Ok { get; set; }
        public double ExpiresSec { get; set; }
    }

    public sealed class EconomyPurchaseItemRequest
    {
        public string ItemId { get; set; }
        public string Receipt { get; set; }
    }

    public sealed class EconomyPurchaseItemResponse
    {
        public Dictionary<string, double> Wallet { get; set; }
        public double CurrentTimeSec { get; set; }
        public bool RewardApplied { get; set; }
    }

    public sealed class EconomyHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal EconomyHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<EconomyStoreGetResponse> StoreGetAsync(ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EconomyStoreGetResponse>(session, HiroRpcIds.EconomyStoreGet, null, cancellationToken);

        public Task<EconomyWalletGetResponse> WalletGetAsync(ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EconomyWalletGetResponse>(session, HiroRpcIds.EconomyWalletGet, null, cancellationToken);

        public Task<EconomyGrantResponse> GrantAsync(
            ISession session,
            EconomyGrantRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EconomyGrantResponse>(session, HiroRpcIds.EconomyGrant, request, cancellationToken);

        public Task<EconomyPurchaseIntentResponse> PurchaseIntentAsync(
            ISession session,
            EconomyPurchaseIntentRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EconomyPurchaseIntentResponse>(session, HiroRpcIds.EconomyPurchaseIntent, request, cancellationToken);

        public Task<EconomyPurchaseItemResponse> PurchaseItemAsync(
            ISession session,
            EconomyPurchaseItemRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EconomyPurchaseItemResponse>(session, HiroRpcIds.EconomyPurchaseItem, request, cancellationToken);
    }
}
