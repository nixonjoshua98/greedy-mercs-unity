
using SimpleJSON;

using Passives          = PassivesData.Passives;
using Weapons           = WeaponData.Weapons;

using RelicData;
using SkillData;
using BountyData;
using CharacterData;



public class StaticData
{
    public const int MAX_CHAR_LEVEL         = 1_000;
    public const int MAX_TAP_UPGRADE_LEVEL  = 1_000;

    public const float BASE_CRIT_CHANCE     = 0.01f;
    public const float BASE_CRIT_MULTIPLIER = 3.0f;

    public static Characters Characters;
    public static Passives Passives;
    public static Weapons Weapons;

    public static RelicListSO Relics;
    public static SkillListSO Skills;
    public static BountyListSO Bounties;
    public static CharacterListSO Chars;

    public static void Restore(JSONNode node)
    {
        Weapons     = new Weapons(node["weapons"]);
        Passives    = new Passives(node["characterPassives"]);
        Characters  = new Characters(node["characters"]);

        Skills.Init();

        Relics.Restore(node["relics"]);
        Chars.Restore(node["characters"]);
        Bounties.Restore(node["bounties"]);
    }

    public static void AssignScriptables(SkillListSO skills, BountyListSO bounties, CharacterListSO chars, RelicListSO relics)
    {
        Chars       = chars;
        Relics      = relics;
        Skills      = skills;
        Bounties    = bounties;
    }
}
