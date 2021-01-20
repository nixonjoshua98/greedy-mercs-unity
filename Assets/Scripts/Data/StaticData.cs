
using SimpleJSON;
using UnityEngine;


namespace GreedyMercs
{
    using GreedyMercs.Data.BountyShop;

    public class StaticData
    {
        public const int MAX_CHAR_LEVEL = 1_000;
        public const int MAX_TAP_UPGRADE_LEVEL = 1_000;
        public const int MAX_AUTO_TAP_LEVEL = 1_000;

        public const float BASE_CRIT_CHANCE = 0.01f;
        public const float BASE_CRIT_MULTIPLIER = 3.0f;

        public static Weapons Weapons;

        public static SkillListSO SkillList;
        public static BountyListSO BountyList;
        public static LootItemListSO LootList;
        public static CharacterListSO CharacterList;

        public static BountyShopListSO BountyShop;

        public static void Restore(JSONNode node)
        {
            Weapons = new Weapons(node["weapons"]);

            SkillList.Init();

            BountyShop.Init(node["bountyShopItems"]);

            CharacterList.Restore(node["characters"], node["characterPassives"]);

            LootList.Restore(node["loot"]);
            BountyList.Restore(node["bounties"]);
        }

        public static void AssignScriptables(SkillListSO skills, BountyListSO bounties, CharacterListSO chars, LootItemListSO loot, BountyShopListSO bountyShop)
        {
            BountyShop = bountyShop;

            CharacterList = chars;
            LootList = loot;
            SkillList = skills;
            BountyList = bounties;
        }
    }
}