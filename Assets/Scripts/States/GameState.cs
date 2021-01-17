using UnityEngine;

using SimpleJSON;

using LootData;
using BountyData;


[System.Serializable]
public class GameState
{
    static _GameState Instance = null;

    class _GameState
    {
        public LootState loot;
        public StageState stage;
        public PlayerState player;
        public SkillsState skills;

        public WeaponContainer weapons;
        public BountyContainer bounties;
        public UpgradesContainer upgrades;
        public CharacterContainer characters;
    }

    public static SkillsState Skills { get { return Instance.skills; } }

    public static StageState Stage { get { return Instance.stage; } }
    public static PlayerState Player { get { return Instance.player; } }
    public static LootState Loot { get { return Instance.loot; } }
    public static WeaponContainer Weapons { get { return Instance.weapons; } }
    public static BountyContainer Bounties { get { return Instance.bounties; } }
    public static UpgradesContainer Upgrades { get { return Instance.upgrades; } }
    public static CharacterContainer Characters { get { return Instance.characters; } }

    public static void Restore(JSONNode node)
    {
        Instance = JsonUtility.FromJson<_GameState>(node.ToString());

        Instance.loot       = new LootState(node);
        Instance.weapons    = new WeaponContainer(node);
        Instance.bounties   = new BountyContainer(node);
        Instance.upgrades   = new UpgradesContainer(node);
        Instance.characters = new CharacterContainer(node);

        Instance.skills = new SkillsState(node);

        Instance.player.OnRestore(node);
    }

    public static void Update(JSONNode node)
    {
        Instance.player.Update(node);
        Instance.weapons.Update(node);
        Instance.loot.Update(node);
        Instance.bounties.Update(node);
    }

    public static JSONNode ToJson()
    {
        JSONNode node = JSON.Parse(JsonUtility.ToJson(Instance));

        node["player"] = Instance.player.ToJson();

        node.Add("skills", Skills.ToJson());
        node.Add("loot", Loot.ToJson());
        node.Add("weapons", Weapons.ToJson());
        node.Add("upgrades", Upgrades.ToJson());
        node.Add("bounties", Bounties.ToJson());
        node.Add("characters", Characters.ToJson());

        return node;
    }

    public static bool IsRestored() { return Instance != null; }
}