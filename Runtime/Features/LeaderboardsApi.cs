using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class LeaderboardVariantView
    {
        public string RegionCode { get; set; }
        public string NakamaLeaderboardId { get; set; }
    }

    public sealed class LeaderboardCatalogEntryView
    {
        public string Id { get; set; }
        public string SortOrder { get; set; }
        public string Operator { get; set; }
        public string ResetSchedule { get; set; }
        public bool Authoritative { get; set; }
        public List<string> Regions { get; set; }
        public List<LeaderboardVariantView> Variants { get; set; }
    }

    public sealed class LeaderboardsConfigGetResponse
    {
        public List<LeaderboardCatalogEntryView> Leaderboards { get; set; }
    }

    public sealed class LeaderboardsWriteScoreRequest
    {
        public string LeaderboardId { get; set; }
        public string RegionId { get; set; }
        public double Score { get; set; }
        public double? Subscore { get; set; }
        public JToken Metadata { get; set; }
    }

    public sealed class LeaderboardsRecordGetRequest
    {
        public string LeaderboardId { get; set; }
        public string RegionId { get; set; }
    }

    public sealed class LeaderboardRecordGetWrapper
    {
        public LeaderboardRecordView Record { get; set; }
    }

    public sealed class LeaderboardRecordView
    {
        public string LeaderboardId { get; set; }
        public string OwnerId { get; set; }
        public string Username { get; set; }
        public double Score { get; set; }
        public double Subscore { get; set; }
        public double NumScore { get; set; }
        public JObject Metadata { get; set; }
        public string Rank { get; set; }
        public string CreateTimeSec { get; set; }
        public string UpdateTimeSec { get; set; }
        public string ExpiryTimeSec { get; set; }
    }

    public sealed class LeaderboardsRecordsGetRequest
    {
        public string LeaderboardId { get; set; }
        public List<string> UserIds { get; set; }
        public string RegionId { get; set; }
    }

    public sealed class LeaderboardsRecordsListRequest
    {
        public string LeaderboardId { get; set; }
        public string RegionId { get; set; }
        public int? Limit { get; set; }
        public string Cursor { get; set; }
    }

    public sealed class LeaderboardsRecordsHaystackRequest
    {
        public string LeaderboardId { get; set; }
        public string RegionId { get; set; }
        public string OwnerId { get; set; }
        public int? Limit { get; set; }
        public string Cursor { get; set; }
        public double? OverrideExpiry { get; set; }
    }

    public sealed class LeaderboardRecordListResponse
    {
        public List<LeaderboardRecordView> Records { get; set; }
        public List<LeaderboardRecordView> OwnerRecords { get; set; }
        public string NextCursor { get; set; }
    }

    public sealed class LeaderboardsHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal LeaderboardsHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<LeaderboardsConfigGetResponse> ConfigGetAsync(ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<LeaderboardsConfigGetResponse>(session, HiroRpcIds.LeaderboardsConfigGet, null, cancellationToken);

        public Task<LeaderboardRecordView> WriteScoreAsync(
            ISession session,
            LeaderboardsWriteScoreRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<LeaderboardRecordView>(session, HiroRpcIds.LeaderboardsWriteScore, request, cancellationToken);

        public Task<LeaderboardRecordGetWrapper> RecordGetAsync(
            ISession session,
            LeaderboardsRecordGetRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<LeaderboardRecordGetWrapper>(session, HiroRpcIds.LeaderboardsRecordGet, request, cancellationToken);

        public Task<LeaderboardRecordListResponse> RecordsGetAsync(
            ISession session,
            LeaderboardsRecordsGetRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<LeaderboardRecordListResponse>(session, HiroRpcIds.LeaderboardsRecordsGet, request, cancellationToken);

        public Task<LeaderboardRecordListResponse> RecordsListAsync(
            ISession session,
            LeaderboardsRecordsListRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<LeaderboardRecordListResponse>(session, HiroRpcIds.LeaderboardsRecordsList, request, cancellationToken);

        public Task<LeaderboardRecordListResponse> RecordsHaystackAsync(
            ISession session,
            LeaderboardsRecordsHaystackRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<LeaderboardRecordListResponse>(session, HiroRpcIds.LeaderboardsRecordsHaystack, request, cancellationToken);
    }
}
