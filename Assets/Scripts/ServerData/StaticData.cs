using System;

using SimpleJSON;

namespace GreedyMercs
{
    using GM.Bounty;
    using GM.Armoury;
    using GM.Artefacts;

    public static class StaticData
    {
        public const int MAX_CHAR_LEVEL         = 1_000;
        public const int MAX_TAP_UPGRADE_LEVEL  = 1_000;
        public const int MAX_AUTO_TAP_LEVEL     = 1_000;

        public const float BASE_CRIT_CHANCE     = 0.01f;
        public const float BASE_CRIT_MULTIPLIER = 3.0f;

        public static SkillListSO SkillList;
        public static CharacterListSO CharacterList;

        public static ServerBountyData Bounty;
        public static ServerArmouryData Armoury;
        public static ServerArtefactData Artefacts;

        public static DateTime NextDailyReset;

        public static void Restore(JSONNode node)
        {
            SkillList.Init();

            CharacterList.Init(node["characters"], node["characterPassives"]);

            Bounty          = new ServerBountyData(node["bounties"]);
            Armoury         = new ServerArmouryData(node["armouryItems"]);
            Artefacts       = new ServerArtefactData(node["artefacts"]);

            NextDailyReset = Funcs.ToDateTime(node["nextDailyReset"]);
        }

        public static void AssignScriptables(SkillListSO skills, CharacterListSO chars)
        {
            CharacterList   = chars;
            SkillList       = skills;
        }
    }
}