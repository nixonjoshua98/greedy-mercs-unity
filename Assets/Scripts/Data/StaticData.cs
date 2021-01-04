
using SimpleJSON;

using Characters        = CharacterData.Characters;
using Passives          = PassivesData.Passives;
using Weapons           = WeaponData.Weapons;
using Bounties          = BountyData.Bounties;
using Relics            = RelicData.Relics;



public class StaticData
{
    static _StaticData Instance = null;

    public const int MAX_CHAR_LEVEL         = 1_000;
    public const int MAX_TAP_UPGRADE_LEVEL  = 1_000;

    public const float BASE_CRIT_CHANCE     = 0.01f;
    public const float BASE_CRIT_MULTIPLIER = 3.0f;

    // === Accessors ===
    public static Relics Relics { get { return Instance.relics; } }
    public static Weapons Weapons { get { return Instance.weapons; } }
    public static Bounties Bounties { get { return Instance.bounties; } }
    public static Passives Passives { get { return Instance.passives; } }
    public static Characters Characters {  get { return Instance.characters; } }

    public static void Restore(JSONNode json)
    {
        if (Instance == null)
        {
            Instance = new _StaticData(json);
        }
    }

    class _StaticData
    {
        // === Attributes ===
        public Characters   characters;
        public Passives     passives;
        public Bounties     bounties;
        public Weapons      weapons;
        public Relics       relics;

        public _StaticData(JSONNode json)
        {
            weapons     = new Weapons(json["weapons"]);
            relics      = new Relics(json["relics"]);
            bounties    = new Bounties(json["bounties"]);
            passives    = new Passives(json["characterPassives"]);
            characters  = new Characters(json["characters"]);
        }
    }
}
