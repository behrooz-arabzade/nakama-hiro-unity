namespace NakamaHiro.Client
{
    /// <summary>
    /// RPC ids registered by nakama-hiro (<c>registerRpc</c>). Keep in sync with sibling repo
    /// <c>src/features/*/register*Rpcs.ts</c> and <c>src/main.ts</c>.
    /// </summary>
    public static class HiroRpcIds
    {
        public const string AchievementsGet = "achievements_get";
        public const string AchievementsUpdate = "achievements_update";
        public const string AchievementsClaim = "achievements_claim";

        public const string InventoryList = "inventory_list";
        public const string InventoryListOwned = "inventory_list_owned";
        public const string InventoryGrant = "inventory_grant";
        public const string InventoryConsume = "inventory_consume";
        public const string InventoryUpdate = "inventory_update";

        public const string EconomyStoreGet = "economy_store_get";
        public const string EconomyWalletGet = "economy_wallet_get";
        public const string EconomyGrant = "economy_grant";
        public const string EconomyPurchaseIntent = "economy_purchase_intent";
        public const string EconomyPurchaseItem = "economy_purchase_item";

        public const string AuctionsTemplatesGet = "auctions_templates_get";
        public const string AuctionsList = "auctions_list";
        public const string AuctionsCreate = "auctions_create";
        public const string AuctionsBid = "auctions_bid";
        public const string AuctionsCancel = "auctions_cancel";
        public const string AuctionsClaimBid = "auctions_claim_bid";
        public const string AuctionsClaimCreated = "auctions_claim_created";
        public const string AuctionsListCreated = "auctions_list_created";
        public const string AuctionsListBids = "auctions_list_bids";

        public const string ProgressionsGet = "progressions_get";
        public const string ProgressionsPurchase = "progressions_purchase";
        public const string ProgressionsUpdate = "progressions_update";
        public const string ProgressionsReset = "progressions_reset";

        public const string StatsGet = "stats_get";
        public const string StatsUpdate = "stats_update";

        public const string StreaksList = "streaks_list";
        public const string StreaksUpdate = "streaks_update";
        public const string StreaksClaim = "streaks_claim";
        public const string StreaksReset = "streaks_reset";

        public const string LeaderboardsConfigGet = "leaderboards_config_get";
        public const string LeaderboardsWriteScore = "leaderboards_write_score";
        public const string LeaderboardsRecordGet = "leaderboards_record_get";
        public const string LeaderboardsRecordsGet = "leaderboards_records_get";
        public const string LeaderboardsRecordsList = "leaderboards_records_list";
        public const string LeaderboardsRecordsHaystack = "leaderboards_records_haystack";

        public const string EventLeaderboardList = "event_leaderboard_list";
        public const string EventLeaderboardGet = "event_leaderboard_get";
        public const string EventLeaderboardUpdate = "event_leaderboard_update";
        public const string EventLeaderboardClaim = "event_leaderboard_claim";
        public const string EventLeaderboardRoll = "event_leaderboard_roll";

        public const string EnergyGet = "energy_get";
        public const string EnergySpend = "energy_spend";
        public const string EnergyGrant = "energy_grant";

        public const string ChallengeCreate = "challenge_create";
        public const string ChallengeList = "challenge_list";
        public const string ChallengeSearch = "challenge_search";
        public const string ChallengeGet = "challenge_get";
        public const string ChallengeInvite = "challenge_invite";
        public const string ChallengeSubmitScore = "challenge_submit_score";
        public const string ChallengeJoin = "challenge_join";
        public const string ChallengeLeave = "challenge_leave";
        public const string ChallengeClaim = "challenge_claim";

        public const string UnlockablesGet = "unlockables_get";
        public const string UnlockablesCreate = "unlockables_create";
        public const string UnlockablesUnlockStart = "unlockables_unlock_start";
        public const string UnlockablesUnlockAdvance = "unlockables_unlock_advance";
        public const string UnlockablesPurchaseUnlock = "unlockables_purchase_unlock";
        public const string UnlockablesPurchaseSlot = "unlockables_purchase_slot";
        public const string UnlockablesClaim = "unlockables_claim";
        public const string UnlockablesQueueAdd = "unlockables_queue_add";
        public const string UnlockablesQueueRemove = "unlockables_queue_remove";
        public const string UnlockablesQueueSet = "unlockables_queue_set";

        public const string ServerLbRegionsList = "server_lb_regions_list";
    }
}
