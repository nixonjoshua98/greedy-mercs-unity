using System;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using GreedyMercs.BountyShop.Data;
    using GreedyMercs.Armoury.Data;

    public static class GameState
    {
        public static PlayerQuestData Quests;

        public static PlayerBountyShopData BountyShop;
        public static PlayerArmouryData Armoury;
        public static PlayerLifetimeStats LifetimeStats;

        public static LootState Loot;
        public static StageState Stage;
        public static PlayerState Player;
        public static SkillsState Skills;

        public static BountyContainer Bounties;
        public static UpgradesContainer Upgrades;
        public static CharacterContainer Characters;

        public static DateTime LastLoginDate;

        public static DateTime LastDailyReset
        {
            get
            {
                DateTime now = DateTime.UtcNow;

                DateTime resetTime = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);

                return now <= resetTime ? resetTime.AddDays(-1) : resetTime; 
            }
        }

        public static int TimeUntilNextReset { get { return (int)(LastDailyReset.AddDays(1) - DateTime.UtcNow).TotalSeconds; } }

        public static void Restore(JSONNode node)
        {
            Stage           = new StageState(node);
            Skills          = new SkillsState(node);
            Loot            = new LootState(node["loot"]);
            Player          = new PlayerState(node["player"]);
            LifetimeStats   = new PlayerLifetimeStats(node["lifetimeStats"]);

            Armoury     = new PlayerArmouryData(node["weapons"]);
            BountyShop  = new PlayerBountyShopData(node["bountyShop"]);

            Bounties    = new BountyContainer(node);
            Upgrades    = new UpgradesContainer(node);
            Characters  = new CharacterContainer(node);

            LastLoginDate = node.HasKey("lastLoginDate") ? node["lastLoginDate"].AsLong.ToUnixDatetime() : DateTime.UtcNow;

            RestoreLocalOnlyData();
        }

        public static void UpdateWithServerData(JSONNode node)
        {
            Loot.Update(node["loot"]);
            Player.Update(node["player"]);
            Armoury.Update(node["weapons"]);
            Bounties.Update(node["bounties"]);
            BountyShop.Update(node["bountyShop"]);

            Quests.UpdateQuestsClaimed(node["questsClaimed"]);
        }

        public static void RestoreLocalOnlyData()
        {
            if (!Utils.File.ReadJson(DataManager.LOCAL_ONLY_FILE, out JSONNode node))
                node = new JSONObject();

            Quests = new PlayerQuestData(node["quests"]);
        }

        public static JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            node.Add("loot",            Loot.ToJson());
            node.Add("stage",           Stage.ToJson());
            node.Add("player",          Player.ToJson());
            node.Add("skills",          Skills.ToJson());
            node.Add("upgrades",        Upgrades.ToJson());
            node.Add("bounties",        Bounties.ToJson());
            node.Add("bountyShop",      BountyShop.ToJson());
            node.Add("characters",      Characters.ToJson());
            node.Add("lifetimeStats",   LifetimeStats.ToJson());

            node.Add("lastLoginDate", DateTime.UtcNow.ToUnixMilliseconds());

            return node;
        }

        public static void Save()
        {
            Utils.File.WriteJson(DataManager.DATA_FILE, ToJson());

            JSONNode node = new JSONObject();

            node.Add("quests", Quests.ToJson());

            Utils.File.WriteJson(DataManager.LOCAL_ONLY_FILE, node);
        }
    }
}