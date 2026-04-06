using System;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using NakamaHiro.Client;
using UnityEngine;

namespace NakamaHiro.Client.Unity
{
    public sealed class NakamaHiroAchievementsSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<AchievementsListResponse> GetCompleted;
        public event Action<AchievementsListResponse> UpdateCompleted;
        public event Action<AchievementsListResponse> ClaimCompleted;

        public async Task<AchievementsListResponse> GetAsync(
            AchievementsGetFilter filter = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Achievements.GetAsync(await SessionAsync(cancellationToken), filter, cancellationToken);
            GetCompleted?.Invoke(r);
            return r;
        }

        public async Task<AchievementsListResponse> UpdateAsync(
            AchievementsUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Achievements.UpdateAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            UpdateCompleted?.Invoke(r);
            return r;
        }

        public async Task<AchievementsListResponse> ClaimAsync(
            AchievementsClaimRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Achievements.ClaimAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ClaimCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroInventorySystem : NakamaHiroFeatureSystemBase
    {
        public event Action<InventoryCodexResponse> ListCompleted;
        public event Action<InventoryOwnedResponse> ListOwnedCompleted;
        public event Action<InventoryOwnedResponse> GrantCompleted;
        public event Action<InventoryConsumeResponse> ConsumeCompleted;
        public event Action<InventoryOwnedResponse> UpdateCompleted;

        public async Task<InventoryCodexResponse> ListAsync(
            InventoryCategoryFilter filter = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Inventory.ListAsync(await SessionAsync(cancellationToken), filter, cancellationToken);
            ListCompleted?.Invoke(r);
            return r;
        }

        public async Task<InventoryOwnedResponse> ListOwnedAsync(
            InventoryCategoryFilter filter = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Inventory.ListOwnedAsync(await SessionAsync(cancellationToken), filter, cancellationToken);
            ListOwnedCompleted?.Invoke(r);
            return r;
        }

        public async Task<InventoryOwnedResponse> GrantAsync(
            InventoryGrantRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Inventory.GrantAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            GrantCompleted?.Invoke(r);
            return r;
        }

        public async Task<InventoryConsumeResponse> ConsumeAsync(
            InventoryConsumeRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Inventory.ConsumeAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ConsumeCompleted?.Invoke(r);
            return r;
        }

        public async Task<InventoryOwnedResponse> UpdateAsync(
            InventoryUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Inventory.UpdateAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            UpdateCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroEconomySystem : NakamaHiroFeatureSystemBase
    {
        public event Action<EconomyStoreGetResponse> StoreGetCompleted;
        public event Action<EconomyWalletGetResponse> WalletGetCompleted;
        public event Action<EconomyGrantResponse> GrantCompleted;
        public event Action<EconomyPurchaseIntentResponse> PurchaseIntentCompleted;
        public event Action<EconomyPurchaseItemResponse> PurchaseItemCompleted;

        public async Task<EconomyStoreGetResponse> StoreGetAsync(CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Economy.StoreGetAsync(await SessionAsync(cancellationToken), cancellationToken);
            StoreGetCompleted?.Invoke(r);
            return r;
        }

        public async Task<EconomyWalletGetResponse> WalletGetAsync(CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Economy.WalletGetAsync(await SessionAsync(cancellationToken), cancellationToken);
            WalletGetCompleted?.Invoke(r);
            return r;
        }

        public async Task<EconomyGrantResponse> GrantAsync(
            EconomyGrantRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Economy.GrantAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            GrantCompleted?.Invoke(r);
            return r;
        }

        public async Task<EconomyPurchaseIntentResponse> PurchaseIntentAsync(
            EconomyPurchaseIntentRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Economy.PurchaseIntentAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            PurchaseIntentCompleted?.Invoke(r);
            return r;
        }

        public async Task<EconomyPurchaseItemResponse> PurchaseItemAsync(
            EconomyPurchaseItemRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Economy.PurchaseItemAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            PurchaseItemCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroAuctionsSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<AuctionTemplatesGetResponse> TemplatesGetCompleted;
        public event Action<AuctionListResponse> ListCompleted;
        public event Action<AuctionListEntry> CreateCompleted;
        public event Action<AuctionListEntry> BidCompleted;
        public event Action<AuctionListEntry> CancelCompleted;
        public event Action<AuctionListEntry> ClaimBidCompleted;
        public event Action<AuctionListEntry> ClaimCreatedCompleted;
        public event Action<AuctionListResponse> ListCreatedCompleted;
        public event Action<AuctionListResponse> ListBidsCompleted;

        public async Task<AuctionTemplatesGetResponse> TemplatesGetAsync(CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Auctions.TemplatesGetAsync(await SessionAsync(cancellationToken), cancellationToken);
            TemplatesGetCompleted?.Invoke(r);
            return r;
        }

        public async Task<AuctionListResponse> ListAsync(
            AuctionListRequest request = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Auctions.ListAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ListCompleted?.Invoke(r);
            return r;
        }

        public async Task<AuctionListEntry> CreateAsync(
            AuctionCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Auctions.CreateAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            CreateCompleted?.Invoke(r);
            return r;
        }

        public async Task<AuctionListEntry> BidAsync(
            AuctionBidRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Auctions.BidAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            BidCompleted?.Invoke(r);
            return r;
        }

        public async Task<AuctionListEntry> CancelAsync(
            AuctionIdRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Auctions.CancelAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            CancelCompleted?.Invoke(r);
            return r;
        }

        public async Task<AuctionListEntry> ClaimBidAsync(
            AuctionIdRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Auctions.ClaimBidAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ClaimBidCompleted?.Invoke(r);
            return r;
        }

        public async Task<AuctionListEntry> ClaimCreatedAsync(
            AuctionIdRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Auctions.ClaimCreatedAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ClaimCreatedCompleted?.Invoke(r);
            return r;
        }

        public async Task<AuctionListResponse> ListCreatedAsync(
            AuctionUserListRequest request = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Auctions.ListCreatedAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ListCreatedCompleted?.Invoke(r);
            return r;
        }

        public async Task<AuctionListResponse> ListBidsAsync(
            AuctionUserListRequest request = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Auctions.ListBidsAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ListBidsCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroProgressionSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<ProgressionsListResponse> GetCompleted;
        public event Action<ProgressionsListResponse> PurchaseCompleted;
        public event Action<ProgressionsListResponse> UpdateCompleted;
        public event Action<ProgressionsListResponse> ResetCompleted;

        public async Task<ProgressionsListResponse> GetAsync(
            ProgressionsGetFilter filter = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Progression.GetAsync(await SessionAsync(cancellationToken), filter, cancellationToken);
            GetCompleted?.Invoke(r);
            return r;
        }

        public async Task<ProgressionsListResponse> PurchaseAsync(
            ProgressionsPurchaseRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Progression.PurchaseAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            PurchaseCompleted?.Invoke(r);
            return r;
        }

        public async Task<ProgressionsListResponse> UpdateAsync(
            ProgressionsUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Progression.UpdateAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            UpdateCompleted?.Invoke(r);
            return r;
        }

        public async Task<ProgressionsListResponse> ResetAsync(
            ProgressionsResetRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Progression.ResetAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ResetCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroStatsSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<StatsSnapshot> GetCompleted;
        public event Action<StatsSnapshot> UpdateCompleted;

        public async Task<StatsSnapshot> GetAsync(CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Stats.GetAsync(await SessionAsync(cancellationToken), cancellationToken);
            GetCompleted?.Invoke(r);
            return r;
        }

        public async Task<StatsSnapshot> UpdateAsync(
            StatsUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Stats.UpdateAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            UpdateCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroStreaksSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<StreaksListResponse> ListCompleted;
        public event Action<StreaksListResponse> UpdateCompleted;
        public event Action<StreaksListResponse> ClaimCompleted;
        public event Action<StreaksListResponse> ResetCompleted;

        public async Task<StreaksListResponse> ListAsync(CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Streaks.ListAsync(await SessionAsync(cancellationToken), cancellationToken);
            ListCompleted?.Invoke(r);
            return r;
        }

        public async Task<StreaksListResponse> UpdateAsync(
            StreaksUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Streaks.UpdateAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            UpdateCompleted?.Invoke(r);
            return r;
        }

        public async Task<StreaksListResponse> ClaimAsync(
            StreaksClaimRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Streaks.ClaimAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ClaimCompleted?.Invoke(r);
            return r;
        }

        public async Task<StreaksListResponse> ResetAsync(
            StreaksResetRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Streaks.ResetAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ResetCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroLeaderboardsSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<LeaderboardsConfigGetResponse> ConfigGetCompleted;
        public event Action<LeaderboardRecordView> WriteScoreCompleted;
        public event Action<LeaderboardRecordGetWrapper> RecordGetCompleted;
        public event Action<LeaderboardRecordListResponse> RecordsGetCompleted;
        public event Action<LeaderboardRecordListResponse> RecordsListCompleted;
        public event Action<LeaderboardRecordListResponse> RecordsHaystackCompleted;

        public async Task<LeaderboardsConfigGetResponse> ConfigGetAsync(CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Leaderboards.ConfigGetAsync(await SessionAsync(cancellationToken), cancellationToken);
            ConfigGetCompleted?.Invoke(r);
            return r;
        }

        public async Task<LeaderboardRecordView> WriteScoreAsync(
            LeaderboardsWriteScoreRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Leaderboards.WriteScoreAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            WriteScoreCompleted?.Invoke(r);
            return r;
        }

        public async Task<LeaderboardRecordGetWrapper> RecordGetAsync(
            LeaderboardsRecordGetRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Leaderboards.RecordGetAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            RecordGetCompleted?.Invoke(r);
            return r;
        }

        public async Task<LeaderboardRecordListResponse> RecordsGetAsync(
            LeaderboardsRecordsGetRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Leaderboards.RecordsGetAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            RecordsGetCompleted?.Invoke(r);
            return r;
        }

        public async Task<LeaderboardRecordListResponse> RecordsListAsync(
            LeaderboardsRecordsListRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Leaderboards.RecordsListAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            RecordsListCompleted?.Invoke(r);
            return r;
        }

        public async Task<LeaderboardRecordListResponse> RecordsHaystackAsync(
            LeaderboardsRecordsHaystackRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Leaderboards.RecordsHaystackAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            RecordsHaystackCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroEventLeaderboardsSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<EventLeaderboardListResponse> ListCompleted;
        public event Action<EventLeaderboardView> GetCompleted;
        public event Action<EventLeaderboardView> UpdateCompleted;
        public event Action<EventLeaderboardView> ClaimCompleted;
        public event Action<EventLeaderboardView> RollCompleted;

        public async Task<EventLeaderboardListResponse> ListAsync(
            EventLeaderboardListRequest request = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.EventLeaderboards.ListAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ListCompleted?.Invoke(r);
            return r;
        }

        public async Task<EventLeaderboardView> GetAsync(
            EventLeaderboardGetRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.EventLeaderboards.GetAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            GetCompleted?.Invoke(r);
            return r;
        }

        public async Task<EventLeaderboardView> UpdateAsync(
            EventLeaderboardUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.EventLeaderboards.UpdateAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            UpdateCompleted?.Invoke(r);
            return r;
        }

        public async Task<EventLeaderboardView> ClaimAsync(
            EventLeaderboardIdRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.EventLeaderboards.ClaimAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ClaimCompleted?.Invoke(r);
            return r;
        }

        public async Task<EventLeaderboardView> RollAsync(
            EventLeaderboardRollRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.EventLeaderboards.RollAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            RollCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroEnergySystem : NakamaHiroFeatureSystemBase
    {
        public event Action<EnergyListResponse> GetCompleted;
        public event Action<EnergyListResponse> SpendCompleted;
        public event Action<EnergyListResponse> GrantCompleted;

        public async Task<EnergyListResponse> GetAsync(CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Energy.GetAsync(await SessionAsync(cancellationToken), cancellationToken);
            GetCompleted?.Invoke(r);
            return r;
        }

        public async Task<EnergyListResponse> SpendAsync(
            EnergySpendGrantRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Energy.SpendAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            SpendCompleted?.Invoke(r);
            return r;
        }

        public async Task<EnergyListResponse> GrantAsync(
            EnergySpendGrantRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Energy.GrantAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            GrantCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroChallengesSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<ChallengeView> CreateCompleted;
        public event Action<ChallengeListResponse> ListCompleted;
        public event Action<ChallengeSearchResponse> SearchCompleted;
        public event Action<ChallengeView> GetCompleted;
        public event Action<ChallengeView> InviteCompleted;
        public event Action<ChallengeView> SubmitScoreCompleted;
        public event Action<ChallengeView> JoinCompleted;
        public event Action<ChallengeView> LeaveCompleted;
        public event Action<ChallengeView> ClaimCompleted;

        public async Task<ChallengeView> CreateAsync(
            ChallengeCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Challenges.CreateAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            CreateCompleted?.Invoke(r);
            return r;
        }

        public async Task<ChallengeListResponse> ListAsync(
            ChallengeListRequest request = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Challenges.ListAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ListCompleted?.Invoke(r);
            return r;
        }

        public async Task<ChallengeSearchResponse> SearchAsync(
            ChallengeSearchRequest request = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Challenges.SearchAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            SearchCompleted?.Invoke(r);
            return r;
        }

        public async Task<ChallengeView> GetAsync(
            ChallengeGetRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Challenges.GetAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            GetCompleted?.Invoke(r);
            return r;
        }

        public async Task<ChallengeView> InviteAsync(
            ChallengeInviteRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Challenges.InviteAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            InviteCompleted?.Invoke(r);
            return r;
        }

        public async Task<ChallengeView> SubmitScoreAsync(
            ChallengeSubmitScoreRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Challenges.SubmitScoreAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            SubmitScoreCompleted?.Invoke(r);
            return r;
        }

        public async Task<ChallengeView> JoinAsync(
            ChallengeIdOnlyRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Challenges.JoinAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            JoinCompleted?.Invoke(r);
            return r;
        }

        public async Task<ChallengeView> LeaveAsync(
            ChallengeIdOnlyRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Challenges.LeaveAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            LeaveCompleted?.Invoke(r);
            return r;
        }

        public async Task<ChallengeView> ClaimAsync(
            ChallengeIdOnlyRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Challenges.ClaimAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ClaimCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroUnlockablesSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<UnlockablesGetResponse> GetCompleted;
        public event Action<UnlockablesGetResponse> CreateCompleted;
        public event Action<UnlockablesGetResponse> UnlockStartCompleted;
        public event Action<UnlockablesGetResponse> UnlockAdvanceCompleted;
        public event Action<UnlockablesGetResponse> PurchaseUnlockCompleted;
        public event Action<UnlockablesGetResponse> PurchaseSlotCompleted;
        public event Action<UnlockablesGetResponse> ClaimCompleted;
        public event Action<UnlockablesGetResponse> QueueAddCompleted;
        public event Action<UnlockablesGetResponse> QueueRemoveCompleted;
        public event Action<UnlockablesGetResponse> QueueSetCompleted;

        public async Task<UnlockablesGetResponse> GetAsync(CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.GetAsync(await SessionAsync(cancellationToken), cancellationToken);
            GetCompleted?.Invoke(r);
            return r;
        }

        public async Task<UnlockablesGetResponse> CreateAsync(
            UnlockablesCreateRequest request = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.CreateAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            CreateCompleted?.Invoke(r);
            return r;
        }

        public async Task<UnlockablesGetResponse> UnlockStartAsync(
            UnlockablesInstanceIdRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.UnlockStartAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            UnlockStartCompleted?.Invoke(r);
            return r;
        }

        public async Task<UnlockablesGetResponse> UnlockAdvanceAsync(
            UnlockablesUnlockAdvanceRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.UnlockAdvanceAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            UnlockAdvanceCompleted?.Invoke(r);
            return r;
        }

        public async Task<UnlockablesGetResponse> PurchaseUnlockAsync(
            UnlockablesInstanceIdRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.PurchaseUnlockAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            PurchaseUnlockCompleted?.Invoke(r);
            return r;
        }

        public async Task<UnlockablesGetResponse> PurchaseSlotAsync(CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.PurchaseSlotAsync(await SessionAsync(cancellationToken), cancellationToken);
            PurchaseSlotCompleted?.Invoke(r);
            return r;
        }

        public async Task<UnlockablesGetResponse> ClaimAsync(
            UnlockablesInstanceIdRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.ClaimAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            ClaimCompleted?.Invoke(r);
            return r;
        }

        public async Task<UnlockablesGetResponse> QueueAddAsync(
            UnlockablesQueueIdsRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.QueueAddAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            QueueAddCompleted?.Invoke(r);
            return r;
        }

        public async Task<UnlockablesGetResponse> QueueRemoveAsync(
            UnlockablesQueueIdsRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.QueueRemoveAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            QueueRemoveCompleted?.Invoke(r);
            return r;
        }

        public async Task<UnlockablesGetResponse> QueueSetAsync(
            UnlockablesQueueSetRequest request = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.Unlockables.QueueSetAsync(await SessionAsync(cancellationToken), request, cancellationToken);
            QueueSetCompleted?.Invoke(r);
            return r;
        }
    }

    public sealed class NakamaHiroServerLoadBalancerSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<ServerLbRegionsListResponse> RegionsListCompleted;

        public async Task<ServerLbRegionsListResponse> RegionsListAsync(
            ServerLbRegionsListRequest request = null,
            CancellationToken cancellationToken = default)
        {
            var r = await Hiro.ServerLoadBalancer.RegionsListAsync(
                await SessionAsync(cancellationToken),
                request,
                cancellationToken);
            RegionsListCompleted?.Invoke(r);
            return r;
        }
    }
}
