using System;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using Inventory;

    using GreedyMercs.BountyShop.Data;

    public static class GameState
    {
        public static PlayerQuestData Quests;

        public static PlayerBountyShopData BountyShop;
        public static PlayerLifetimeStats LifetimeStats;

        public static LootState Loot;
        public static StageState Stage;
        public static PlayerState Player;
        public static SkillsState Skills;

        public static UpgradesContainer Upgrades;
        public static CharacterContainer Characters;

        public static PlayerInventory Inventory;

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
            Loot = new LootState();

            Quests = new PlayerQuestData(new JSONObject());

            Inventory = new PlayerInventory(node["inventory"]);

            Stage           = new StageState(node);
            Skills          = new SkillsState(node);
            Player          = new PlayerState(node["player"]);
            LifetimeStats   = new PlayerLifetimeStats(node["lifetimeStats"]);

            BountyShop  = new PlayerBountyShopData(node["bountyShop"]);

            Upgrades    = new UpgradesContainer(node);
            Characters  = new CharacterContainer(node);

            LastLoginDate = node.HasKey("lastLoginDate") ? node["lastLoginDate"].AsLong.ToUnixDatetime() : DateTime.UtcNow;
        }

        public static void Prestige()
        {
            Stage.Reset();
            Player.Reset();

            Skills.Clear();
            Upgrades.Clear();
            Characters.Clear();
        }
        
        public static void UpdateWithServerData(JSONNode node)
        {
            Player.Update(node["player"]);
            BountyShop.Update(node["bountyShop"]);
            LifetimeStats.Update(node["lifetimeStats"]);

            Loot.UpdateWithServerData(node["loot"]);

            Quests.UpdateQuestsClaimed(node["questsClaimed"]);

            Inventory.Update(node["inventory"]);
        }

        public static JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            node.Add("inventory",       Inventory.ToJson());
            node.Add("stage",           Stage.ToJson());
            node.Add("player",          Player.ToJson());
            node.Add("skills",          Skills.ToJson());
            node.Add("upgrades",        Upgrades.ToJson());
            node.Add("bountyShop",      BountyShop.ToJson());
            node.Add("characters",      Characters.ToJson());
            node.Add("lifetimeStats",   LifetimeStats.ToJson());

            node.Add("lastLoginDate", DateTime.UtcNow.ToUnixMilliseconds());

            return node;
        }

        public static void Save()
        {
            SaveLocalDataOnly();

            Loot.Save();
        }

        public static void SaveLocalDataOnly()
        {
            JSONNode node = new JSONObject();

            node.Add("quests", Quests.ToJson());
        }
    }
}