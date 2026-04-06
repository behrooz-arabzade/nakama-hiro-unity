using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;

namespace NakamaHiro.Client
{
    public sealed class UnlockableCostMap
    {
        public Dictionary<string, double> Items { get; set; }
        public Dictionary<string, double> Currencies { get; set; }
    }

    public sealed class UnlockablesCatalogEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double WaitTimeSec { get; set; }
        public double CostUnitTimeSec { get; set; }
        public UnlockableCostMap StartCost { get; set; }
        public UnlockableCostMap Cost { get; set; }
        public Dictionary<string, string> AdditionalProperties { get; set; }
    }

    public sealed class UnlockablesInstanceWire
    {
        public string InstanceId { get; set; }
        public string UnlockableId { get; set; }
        public string Phase { get; set; }
        public double? UnlockStartedAtSec { get; set; }
        public double? UnlockCompletesAtSec { get; set; }
        public double? SecondsRemaining { get; set; }
    }

    public sealed class UnlockablesGetResponse
    {
        public double CurrentTimeSec { get; set; }
        public List<UnlockablesCatalogEntry> Catalog { get; set; }
        public List<UnlockablesInstanceWire> Instances { get; set; }
        public List<string> QueuedInstanceIds { get; set; }
        public double OwnedConcurrentUnlockSlots { get; set; }
        public double MaxConcurrentUnlockSlots { get; set; }
        public double MaxStoredInstances { get; set; }
    }

    public sealed class UnlockablesCreateRequest
    {
        public string UnlockableId { get; set; }
    }

    public sealed class UnlockablesInstanceIdRequest
    {
        public string InstanceId { get; set; }
    }

    public sealed class UnlockablesUnlockAdvanceRequest
    {
        public string InstanceId { get; set; }
        public double Seconds { get; set; }
    }

    public sealed class UnlockablesQueueIdsRequest
    {
        public List<string> InstanceIds { get; set; }
    }

    public sealed class UnlockablesQueueSetRequest
    {
        public List<string> InstanceIds { get; set; }
    }

    public sealed class UnlockablesHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal UnlockablesHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<UnlockablesGetResponse> GetAsync(ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesGet, null, cancellationToken);

        public Task<UnlockablesGetResponse> CreateAsync(
            ISession session,
            UnlockablesCreateRequest request = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesCreate, request, cancellationToken);

        public Task<UnlockablesGetResponse> UnlockStartAsync(
            ISession session,
            UnlockablesInstanceIdRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesUnlockStart, request, cancellationToken);

        public Task<UnlockablesGetResponse> UnlockAdvanceAsync(
            ISession session,
            UnlockablesUnlockAdvanceRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesUnlockAdvance, request, cancellationToken);

        public Task<UnlockablesGetResponse> PurchaseUnlockAsync(
            ISession session,
            UnlockablesInstanceIdRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesPurchaseUnlock, request, cancellationToken);

        public Task<UnlockablesGetResponse> PurchaseSlotAsync(ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesPurchaseSlot, null, cancellationToken);

        public Task<UnlockablesGetResponse> ClaimAsync(
            ISession session,
            UnlockablesInstanceIdRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesClaim, request, cancellationToken);

        public Task<UnlockablesGetResponse> QueueAddAsync(
            ISession session,
            UnlockablesQueueIdsRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesQueueAdd, request, cancellationToken);

        public Task<UnlockablesGetResponse> QueueRemoveAsync(
            ISession session,
            UnlockablesQueueIdsRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesQueueRemove, request, cancellationToken);

        public Task<UnlockablesGetResponse> QueueSetAsync(
            ISession session,
            UnlockablesQueueSetRequest request = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<UnlockablesGetResponse>(session, HiroRpcIds.UnlockablesQueueSet, request, cancellationToken);
    }
}
