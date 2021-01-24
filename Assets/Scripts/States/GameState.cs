using System;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using GreedyMercs.BountyShop.Data;
    using GreedyMercs.Armoury.Data;

    public static class GameState
    {
        public static PlayerBountyShopData BountyShop;
        public static PlayerArmouryData Armoury;

        public static LootState Loot;
        public static StageState Stage;
        public static PlayerState Player;
        public static SkillsState Skills;

        public static BountyContainer Bounties;
        public static UpgradesContainer Upgrades;
        public static CharacterContainer Characters;

        public static DateTime LastLoginDate;

        public static void Restore(JSONNode node)
        {
            Loot    = new LootState(node["loot"]);
            Stage   = new StageState(node);
            Skills  = new SkillsState(node);
            Player  = new PlayerState(node["player"]);

            Armoury = new PlayerArmouryData(node["weapons"]);

            BountyShop = new PlayerBountyShopData(node["bountyShop"]);

            Bounties    = new BountyContainer(node);

            Upgrades    = new UpgradesContainer(node);
            Characters  = new CharacterContainer(node);

            LastLoginDate = node.HasKey("lastLoginDate") ? DateTimeOffset.FromUnixTimeMilliseconds(node["lastLoginDate"].AsLong).DateTime : DateTime.UtcNow;
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
            node.Add("characters",      Characters.ToJson());
            node.Add("userBountyShop",  BountyShop.ToJson());

            node.Add("lastLoginDate", DateTime.UtcNow.ToUnixMilliseconds());

            return node;
        }
    }
}