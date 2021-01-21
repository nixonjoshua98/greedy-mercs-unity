using System;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using GreedyMercs.BountyShop.Data;

    public static class GameState
    {
        public static PlayerBountyShopData BountyShop;

        public static LootState Loot;
        public static StageState Stage;
        public static PlayerState Player;
        public static SkillsState Skills;

        public static WeaponContainer Weapons;
        public static BountyContainer Bounties;
        public static UpgradesContainer Upgrades;
        public static CharacterContainer Characters;

        public static DateTime LastLoginDate;

        public static void Restore(JSONNode node)
        {
            Loot    = new LootState(node);
            Stage   = new StageState(node);
            Skills  = new SkillsState(node);
            Player  = new PlayerState(node);

            BountyShop = new PlayerBountyShopData(node["userBountyShop"]);

            Weapons     = new WeaponContainer(node["weapons"]);
            Bounties    = new BountyContainer(node);

            Upgrades    = new UpgradesContainer(node);
            Characters  = new CharacterContainer(node);

            LastLoginDate = node.HasKey("lastLoginDate") ? DateTimeOffset.FromUnixTimeMilliseconds(node["lastLoginDate"].AsLong).DateTime : DateTime.UtcNow;
        }

        public static JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            node.Add("loot",        Loot.ToJson());
            node.Add("stage",       Stage.ToJson());
            node.Add("player",      Player.ToJson());
            node.Add("skills",      Skills.ToJson());
            node.Add("weapons",     Weapons.ToJson());
            node.Add("upgrades",    Upgrades.ToJson());
            node.Add("bounties",    Bounties.ToJson());
            node.Add("characters",  Characters.ToJson());

            node.Add("lastLoginDate", DateTime.UtcNow.ToUnixMilliseconds());

            return node;
        }
    }
}