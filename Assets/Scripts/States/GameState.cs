using System;

using SimpleJSON;

namespace GreedyMercs
{
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

        public static DateTime LastLoginDate;

        public static void Restore(JSONNode node)
        {
            Loot = new LootState();

            Quests = new PlayerQuestData(new JSONObject());

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