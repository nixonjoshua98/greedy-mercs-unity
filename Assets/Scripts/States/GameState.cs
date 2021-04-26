using System;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using GM.Armoury;

    using Inventory;

    using GreedyMercs.BountyShop.Data;
    using GreedyMercs.Armoury.Data;
    using GreedyMercs.Perks.Data;

    public static class GameState
    {
        public static PlayerQuestData Quests;
        public static PlayerPerkData Perks;

        public static PlayerBountyShopData BountyShop;
        public static ArmouryData Armoury;
        public static PlayerLifetimeStats LifetimeStats;

        public static LootState Loot;
        public static StageState Stage;
        public static PlayerState Player;
        public static SkillsState Skills;

        public static BountyContainer Bounties;
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

            Inventory = new PlayerInventory(node["inventory"]);

            Stage           = new StageState(node);
            Skills          = new SkillsState(node);
            Player          = new PlayerState(node["player"]);
            LifetimeStats   = new PlayerLifetimeStats(node["lifetimeStats"]);

            Armoury = ArmouryData.FromJsonNode(node["armoury"]);

            BountyShop  = new PlayerBountyShopData(node["bountyShop"]);

            Bounties    = new BountyContainer(node["bounties"]);
            Upgrades    = new UpgradesContainer(node);
            Characters  = new CharacterContainer(node);

            LastLoginDate = node.HasKey("lastLoginDate") ? node["lastLoginDate"].AsLong.ToUnixDatetime() : DateTime.UtcNow;

            UpdateWithLocalData();
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
            Armoury = ArmouryData.FromJsonNode(node["armoury"]);

            Player.Update(node["player"]);
            Bounties.Update(node["bounties"]);
            BountyShop.Update(node["bountyShop"]);
            LifetimeStats.Update(node["lifetimeStats"]);

            Loot.UpdateWithServerData(node["loot"]);

            Quests.UpdateQuestsClaimed(node["questsClaimed"]);

            Inventory.Update(node["inventory"]);
        }

        public static void UpdateWithLocalData()
        {
            if (!Utils.File.ReadJson(DataManager.LOCAL_ONLY_FILE, out JSONNode node))
                node = new JSONObject();

            Quests  = new PlayerQuestData(node["quests"]);
            Perks   = new PlayerPerkData(node["perks"]);

            Loot.UpdateWithLocalData();
        }

        public static JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            node.Add("inventory",       Inventory.ToJson());
            node.Add("stage",           Stage.ToJson());
            node.Add("player",          Player.ToJson());
            node.Add("skills",          Skills.ToJson());
            node.Add("armoury",         Armoury.Serialize());
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

            SaveLocalDataOnly();

            Loot.Save();
        }

        public static void SaveLocalDataOnly()
        {
            JSONNode node = new JSONObject();

            node.Add("quests", Quests.ToJson());
            node.Add("perks", Perks.ToJson());

            Utils.File.WriteJson(DataManager.LOCAL_ONLY_FILE, node);
        }
    }
}