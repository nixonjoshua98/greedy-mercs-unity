using System;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    using GreedyMercs.Data.BountyShop;

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
            Stage = new StageState(node);

            BountyShop = new PlayerBountyShopData(node["userBountyShop"]);

            Loot        = new LootState(node);
            Weapons     = new WeaponContainer(node);
            Bounties    = new BountyContainer(node);

            Upgrades    = new UpgradesContainer(node);
            Characters  = new CharacterContainer(node);

            Skills = new SkillsState(node);
            Player = new PlayerState(node);

            LastLoginDate = node.HasKey("lastLoginDate") ? DateTimeOffset.FromUnixTimeMilliseconds(node["lastLoginDate"].AsLong).DateTime : DateTime.UtcNow;
        }

        public static void Update(JSONNode node)
        {
            BountyShop.Update(node);

            Player.Update(node);
            Weapons.Update(node);
            Loot.Update(node);
            Bounties.Update(node);
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