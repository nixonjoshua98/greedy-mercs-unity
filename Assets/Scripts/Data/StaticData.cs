
using SimpleJSON;
using UnityEngine;


namespace GreedyMercs
{
    using GreedyMercs.BountyShop.Data;
    using GreedyMercs.Armoury.Data;

    public class StaticData
    {
        public const int MAX_CHAR_LEVEL = 1_000;
        public const int MAX_TAP_UPGRADE_LEVEL = 1_000;
        public const int MAX_AUTO_TAP_LEVEL = 1_000;

        public const float BASE_CRIT_CHANCE = 0.01f;
        public const float BASE_CRIT_MULTIPLIER = 3.0f;

        public static SkillListSO SkillList;
        public static BountyListSO BountyList;
        public static LootItemListSO LootList;
        public static CharacterListSO CharacterList;

        public static BountyShopListSO BountyShop;

        public static ArmourySO Armoury;

        public static void Restore(JSONNode node)
        {
            SkillList.Init();

            LootList.Init(node["loot"]);
            Armoury.Init(node["armoury"]);
            BountyList.Init(node["bounties"]);
            BountyShop.Init(node["bountyShopItems"]);
            CharacterList.Init(node["characters"], node["characterPassives"]);
        }

        public static void AssignScriptables(SkillListSO skills, BountyListSO bounties, CharacterListSO chars, LootItemListSO loot, BountyShopListSO bountyShop, ArmourySO armoury)
        {
            BountyShop      = bountyShop;
            Armoury         = armoury;
            CharacterList   = chars;
            LootList        = loot;
            SkillList       = skills;
            BountyList      = bounties;
        }
    }
}