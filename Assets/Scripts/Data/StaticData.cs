
using SimpleJSON;

using LootData;
using SkillData;
using WeaponData;
using BountyData;
using PassivesData;
using CharacterData;

public class StaticData
{
    public const int MAX_CHAR_LEVEL         = 1_000;
    public const int MAX_TAP_UPGRADE_LEVEL  = 1_000;
    public const int MAX_AUTO_TAP_LEVEL     = 1_000;

    public const float BASE_CRIT_CHANCE     = 0.01f;
    public const float BASE_CRIT_MULTIPLIER = 3.0f;

    public static Characters Characters;
    public static Passives Passives;
    public static Weapons Weapons;

    public static SkillListSO SkillList;
    public static BountyListSO BountyList;
    public static LootItemListSO LootList;
    public static CharacterListSO CharacterList;

    public static void Restore(JSONNode node)
    {
        Weapons     = new Weapons(node["weapons"]);
        Passives    = new Passives(node["characterPassives"]);
        Characters  = new Characters(node["characters"]);

        SkillList.Init();

        LootList.Restore(node["loot"]);
        CharacterList.Restore(node["characters"]);
        BountyList.Restore(node["bounties"]);
    }

    public static void AssignScriptables(SkillListSO skills, BountyListSO bounties, CharacterListSO chars, LootItemListSO loot)
    {
        CharacterList   = chars;
        LootList        = loot;
        SkillList       = skills;
        BountyList      = bounties;
    }
}
