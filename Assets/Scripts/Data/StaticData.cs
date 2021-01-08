
using SimpleJSON;

using Characters        = CharacterData.Characters;
using Passives          = PassivesData.Passives;
using Weapons           = WeaponData.Weapons;
using Relics            = RelicData.Relics;



public class StaticData
{
    public const int MAX_CHAR_LEVEL         = 1_000;
    public const int MAX_TAP_UPGRADE_LEVEL  = 1_000;

    public const float BASE_CRIT_CHANCE     = 0.01f;
    public const float BASE_CRIT_MULTIPLIER = 3.0f;

    public static Characters Characters;
    public static Passives Passives;
    public static Weapons Weapons;
    public static Relics Relics;

    public static BountyListSO Bounties;

    public static void Restore(JSONNode node)
    {
        Weapons     = new Weapons(node["weapons"]);
        Relics      = new Relics(node["relics"]);
        Passives    = new Passives(node["characterPassives"]);
        Characters  = new Characters(node["characters"]);
    }

    public static void AssignBounties(BountyListSO ls) => Bounties = ls;
}
