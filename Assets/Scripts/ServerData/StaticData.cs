using System;

using SimpleJSON;

using GM;

public static class StaticData
{
    public const int MIN_PRESTIGE_STAGE = 60;

    public const int MAX_CHAR_LEVEL = 1_000;
    public const int MAX_TAP_UPGRADE_LEVEL = 1_000;
    public const int MAX_AUTO_TAP_LEVEL = 1_000;

    public const float BASE_CRIT_CHANCE = 0.01f;
    public const float BASE_CRIT_MULTIPLIER = 3.0f;

    public static SkillListSO SkillList;

    public static DateTime NextDailyReset;

    public static void Restore(JSONNode node)
    {
        SkillList.Init();

        NextDailyReset = Funcs.ToDateTime(node["nextDailyReset"]);
    }

    public static void AssignScriptables(SkillListSO skills)
    {
        SkillList = skills;
    }
}
