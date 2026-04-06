using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json.Linq;

namespace NakamaHiro.Client
{
    public sealed class ChallengeCreateRequest
    {
        public string TemplateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Invitees { get; set; }
        public bool? Open { get; set; }
        public double? MaxScores { get; set; }
        public double? StartDelaySec { get; set; }
        public double? DurationSec { get; set; }
        public double? MaxParticipants { get; set; }
        public string Category { get; set; }
    }

    public sealed class ChallengeListRequest
    {
        public List<string> Categories { get; set; }
        public bool? WithScores { get; set; }
    }

    public sealed class ChallengeSearchRequest
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int? Limit { get; set; }
        public string Cursor { get; set; }
    }

    public sealed class ChallengeGetRequest
    {
        public string ChallengeId { get; set; }
        public bool? WithScores { get; set; }
    }

    public sealed class ChallengeInviteRequest
    {
        public string ChallengeId { get; set; }
        public List<string> Invitees { get; set; }
    }

    public sealed class ChallengeSubmitScoreRequest
    {
        public string ChallengeId { get; set; }
        public double Score { get; set; }
        public double? Subscore { get; set; }
        public string Metadata { get; set; }
        public string Operator { get; set; }
    }

    public sealed class ChallengeIdOnlyRequest
    {
        public string ChallengeId { get; set; }
    }

    public sealed class ChallengeRecordView
    {
        public string OwnerId { get; set; }
        public string Username { get; set; }
        public double Score { get; set; }
        public double Subscore { get; set; }
        public double NumScore { get; set; }
        public JObject Metadata { get; set; }
        public string Rank { get; set; }
        public string UpdateTimeSec { get; set; }
    }

    public sealed class ChallengeView
    {
        public string Id { get; set; }
        public string TemplateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool Open { get; set; }
        public bool Ascending { get; set; }
        public string Operator { get; set; }
        public string CreatorUserId { get; set; }
        public string Status { get; set; }
        public string StartTimeSec { get; set; }
        public string EndTimeSec { get; set; }
        public string MaxParticipants { get; set; }
        public string MaxScoresPerParticipant { get; set; }
        public string ParticipantCount { get; set; }
        public bool CanSubmitScore { get; set; }
        public bool CanJoin { get; set; }
        public bool CanClaim { get; set; }
        public bool Claimed { get; set; }
        public List<ChallengeRecordView> Records { get; set; }
        public string NextCursor { get; set; }
    }

    public sealed class ChallengeListResponse
    {
        public List<ChallengeView> Challenges { get; set; }
    }

    public sealed class ChallengeSearchResponse
    {
        public List<ChallengeView> Challenges { get; set; }
        public string NextCursor { get; set; }
    }

    public sealed class ChallengesHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal ChallengesHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<ChallengeView> CreateAsync(
            ISession session,
            ChallengeCreateRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ChallengeView>(session, HiroRpcIds.ChallengeCreate, request, cancellationToken);

        public Task<ChallengeListResponse> ListAsync(
            ISession session,
            ChallengeListRequest request = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ChallengeListResponse>(session, HiroRpcIds.ChallengeList, request, cancellationToken);

        public Task<ChallengeSearchResponse> SearchAsync(
            ISession session,
            ChallengeSearchRequest request = null,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ChallengeSearchResponse>(session, HiroRpcIds.ChallengeSearch, request, cancellationToken);

        public Task<ChallengeView> GetAsync(
            ISession session,
            ChallengeGetRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ChallengeView>(session, HiroRpcIds.ChallengeGet, request, cancellationToken);

        public Task<ChallengeView> InviteAsync(
            ISession session,
            ChallengeInviteRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ChallengeView>(session, HiroRpcIds.ChallengeInvite, request, cancellationToken);

        public Task<ChallengeView> SubmitScoreAsync(
            ISession session,
            ChallengeSubmitScoreRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ChallengeView>(session, HiroRpcIds.ChallengeSubmitScore, request, cancellationToken);

        public Task<ChallengeView> JoinAsync(
            ISession session,
            ChallengeIdOnlyRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ChallengeView>(session, HiroRpcIds.ChallengeJoin, request, cancellationToken);

        public Task<ChallengeView> LeaveAsync(
            ISession session,
            ChallengeIdOnlyRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ChallengeView>(session, HiroRpcIds.ChallengeLeave, request, cancellationToken);

        public Task<ChallengeView> ClaimAsync(
            ISession session,
            ChallengeIdOnlyRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<ChallengeView>(session, HiroRpcIds.ChallengeClaim, request, cancellationToken);
    }
}
