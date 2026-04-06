using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class AuctionConditionBid
    {
        public Dictionary<string, double> Currencies { get; set; }
    }

    public sealed class AuctionConditionBidIncrement
    {
        public double? Percentage { get; set; }
        public AuctionConditionBid Fixed { get; set; }
    }

    public sealed class AuctionConditionFee
    {
        public double? Percentage { get; set; }
        public AuctionConditionBid Fixed { get; set; }
    }

    public sealed class AuctionConditionCost
    {
        public Dictionary<string, double> Currencies { get; set; }
        public Dictionary<string, double> Energies { get; set; }
        public Dictionary<string, double> Items { get; set; }
    }

    public sealed class AuctionCondition
    {
        public double DurationSec { get; set; }
        public AuctionConditionCost ListingCost { get; set; }
        public AuctionConditionBid BidStart { get; set; }
        public AuctionConditionBidIncrement BidIncrement { get; set; }
        public double? ExtensionThresholdSec { get; set; }
        public double? ExtensionSec { get; set; }
        public double? ExtensionMaxSec { get; set; }
        public AuctionConditionFee Fee { get; set; }
    }

    public sealed class AuctionTemplateWire
    {
        public List<string> Items { get; set; }
        public List<string> ItemSets { get; set; }
        public Dictionary<string, AuctionCondition> Conditions { get; set; }
    }

    public sealed class AuctionTemplatesGetResponse
    {
        public Dictionary<string, AuctionTemplateWire> Templates { get; set; }
    }

    public sealed class ItemInstanceDto
    {
        public string ItemId { get; set; }
        public double OwnedTime { get; set; }
        public double UpdateTime { get; set; }
        public double Count { get; set; }
        public Dictionary<string, string> StringProperties { get; set; }
        public Dictionary<string, double> NumericProperties { get; set; }
    }

    public sealed class AuctionRecord
    {
        public string Id { get; set; }
        public string SellerId { get; set; }
        public string TemplateId { get; set; }
        public string ConditionId { get; set; }
        public string Status { get; set; }
        public double StartTimeSec { get; set; }
        public double OriginalEndTimeSec { get; set; }
        public double EndTimeSec { get; set; }
        public double ExtensionAppliedSec { get; set; }
        public Dictionary<string, double> BidCurrencies { get; set; }
        public string HighBidderId { get; set; }
        public List<ItemInstanceDto> EscrowItems { get; set; }
        public bool BidClaimed { get; set; }
        public bool CreatedClaimed { get; set; }
    }

    public sealed class AuctionListEntry
    {
        public AuctionRecord Auction { get; set; }
        public string Version { get; set; }
        public double CurrentTimeSec { get; set; }
    }

    public sealed class AuctionListResponse
    {
        public List<AuctionListEntry> Auctions { get; set; }
        public string Cursor { get; set; }
        public double CurrentTimeSec { get; set; }
    }

    public sealed class AuctionListRequest
    {
        public string Query { get; set; }
        public List<string> Sort { get; set; }
        public int? Limit { get; set; }
        public string Cursor { get; set; }
    }

    public sealed class AuctionCreateRequest
    {
        public string TemplateId { get; set; }
        public string ConditionId { get; set; }
        public Dictionary<string, JToken> InstanceIds { get; set; }
    }

    public sealed class AuctionBidRequest
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public Dictionary<string, JToken> Currencies { get; set; }
    }

    public sealed class AuctionIdRequest
    {
        public string Id { get; set; }
    }

    public sealed class AuctionUserListRequest
    {
        public int? Limit { get; set; }
        public string Cursor { get; set; }
    }

    public sealed class AuctionsHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal AuctionsHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<AuctionTemplatesGetResponse> TemplatesGetAsync(ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AuctionTemplatesGetResponse>(session, HiroRpcIds.AuctionsTemplatesGet, null, cancellationToken);

        public Task<AuctionListResponse> ListAsync(
            ISession session,
            AuctionListRequest request = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AuctionListResponse>(session, HiroRpcIds.AuctionsList, request, cancellationToken);

        public Task<AuctionListEntry> CreateAsync(
            ISession session,
            AuctionCreateRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AuctionListEntry>(session, HiroRpcIds.AuctionsCreate, request, cancellationToken);

        public Task<AuctionListEntry> BidAsync(
            ISession session,
            AuctionBidRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AuctionListEntry>(session, HiroRpcIds.AuctionsBid, request, cancellationToken);

        public Task<AuctionListEntry> CancelAsync(
            ISession session,
            AuctionIdRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AuctionListEntry>(session, HiroRpcIds.AuctionsCancel, request, cancellationToken);

        public Task<AuctionListEntry> ClaimBidAsync(
            ISession session,
            AuctionIdRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AuctionListEntry>(session, HiroRpcIds.AuctionsClaimBid, request, cancellationToken);

        public Task<AuctionListEntry> ClaimCreatedAsync(
            ISession session,
            AuctionIdRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AuctionListEntry>(session, HiroRpcIds.AuctionsClaimCreated, request, cancellationToken);

        public Task<AuctionListResponse> ListCreatedAsync(
            ISession session,
            AuctionUserListRequest request = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AuctionListResponse>(session, HiroRpcIds.AuctionsListCreated, request, cancellationToken);

        public Task<AuctionListResponse> ListBidsAsync(
            ISession session,
            AuctionUserListRequest request = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<AuctionListResponse>(session, HiroRpcIds.AuctionsListBids, request, cancellationToken);
    }
}
