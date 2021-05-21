using System;

using SimpleJSON;

namespace GreedyMercs
{
    using GM.Bounty;
    using GM.Armoury;
    using GM.BountyShop;

    using GreedyMercs.Quests.Data;

    public static class StaticData
    {
        public const int MAX_CHAR_LEVEL         = 1_000;
        public const int MAX_TAP_UPGRADE_LEVEL  = 1_000;
        public const int MAX_AUTO_TAP_LEVEL     = 1_000;

        public const float BASE_CRIT_CHANCE     = 0.01f;
        public const float BASE_CRIT_MULTIPLIER = 3.0f;

        public static SkillListSO SkillList;
        public static LootItemListSO LootList;
        public static CharacterListSO CharacterList;

        public static ServerBountyData Bounty;
        public static ServerArmouryData Armoury;
        public static ServerBountyShopData BountyShop;

        public static StaticQuestData Quests;

        public static DateTime NextDailyReset;

        public static void Restore(JSONNode node)
        {
            Quests = new StaticQuestData(node["quests"]);

            SkillList.Init();

            LootList.Init(node["loot"]);
            CharacterList.Init(node["characters"], node["characterPassives"]);

            Bounty      = new ServerBountyData(node["bounties"]);
            Armoury     = new ServerArmouryData(node["armoury"]);
            BountyShop  = new ServerBountyShopData(node["bountyShop"]);

            NextDailyReset = Funcs.ToDateTime(node["nextDailyReset"]);
        }

        public static void AssignScriptables(SkillListSO skills, CharacterListSO chars, LootItemListSO loot)
        {
            CharacterList   = chars;
            LootList        = loot;
            SkillList       = skills;
        }
    }
}