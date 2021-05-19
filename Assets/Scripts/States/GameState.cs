using System;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using Inventory;

    public static class GameState
    {
        public static PlayerQuestData Quests;

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
            LifetimeStats.Update(node["lifetimeStats"]);

            Loot.UpdateWithServerData(node["loot"]);

            Quests.UpdateQuestsClaimed(node["questsClaimed"]);

            Inventory.Update(node["inventory"]);
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