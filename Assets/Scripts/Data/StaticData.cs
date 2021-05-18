﻿
using SimpleJSON;

namespace GreedyMercs
{
    using GreedyMercs.BountyShop.Data;
    using GreedyMercs.Armoury.Data;
    using GreedyMercs.Quests.Data;

    public static class StaticData
    {
        public const int MAX_CHAR_LEVEL         = 1_000;
        public const int MAX_TAP_UPGRADE_LEVEL  = 1_000;
        public const int MAX_AUTO_TAP_LEVEL     = 1_000;

        public const float BASE_CRIT_CHANCE     = 0.01f;
        public const float BASE_CRIT_MULTIPLIER = 3.0f;

        public static SkillListSO SkillList;
        public static BountyListSO BountyList;
        public static LootItemListSO LootList;
        public static CharacterListSO CharacterList;

        public static BountyShopData BountyShop;

        public static ArmourySO Armoury;

        // Stores the data for the daily quests
        public static StaticQuestData Quests;

        public static void Restore(JSONNode node)
        {
            Quests = new StaticQuestData(node["quests"]);

            SkillList.Init();

            BountyShop = new BountyShopData(node["bountyShopItems"]);

            LootList.Init(node["loot"]);
            Armoury.Init(node["armoury"]);
            BountyList.Init(node["bounties"]);
            CharacterList.Init(node["characters"], node["characterPassives"]);
        }

        public static void AssignScriptables(SkillListSO skills, BountyListSO bounties, CharacterListSO chars, LootItemListSO loot, ArmourySO armoury)
        {
            Armoury         = armoury;
            CharacterList   = chars;
            LootList        = loot;
            SkillList       = skills;
            BountyList      = bounties;
        }
    }
}