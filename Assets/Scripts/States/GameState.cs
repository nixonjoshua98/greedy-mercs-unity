using System;

using SimpleJSON;

namespace GreedyMercs
{
    public static class GameState
    {
        public static StageState Stage;
        public static PlayerState Player;

        public static UpgradesContainer Upgrades;
        public static CharacterContainer Characters;

        public static DateTime LastLoginDate;

        public static void Restore(JSONNode node)
        {
            Stage           = new StageState(node);
            Player          = new PlayerState(node["player"]);

            Upgrades    = new UpgradesContainer(node);
            Characters  = new CharacterContainer(node);

            LastLoginDate = node.HasKey("lastLoginDate") ? node["lastLoginDate"].AsLong.ToUnixDatetime() : DateTime.UtcNow;
        }

        public static void Prestige()
        {
            Stage.Reset();
            Player.Reset();

            Upgrades.Clear();
            Characters.Clear();
        }
    }
}