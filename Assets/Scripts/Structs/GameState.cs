﻿using UnityEngine;

using SimpleJSON;


[System.Serializable]
public class GameState
{
    static _GameState Instance = null;

    class _GameState
    {
        public StageState stage;

        public PlayerState player;

        public RelicContainer relics;
        public BountyContainer bounties;
        public CharacterContainer characters;
        public UpgradesContainer playerUpgrades;
    }

    public static StageState Stage { get { return Instance.stage; } }
    public static PlayerState Player { get { return Instance.player; } }
    public static RelicContainer Relics { get { return Instance.relics; } }
    public static BountyContainer Bounties { get { return Instance.bounties; } }
    public static CharacterContainer Characters { get { return Instance.characters; } }
    public static UpgradesContainer PlayerUpgrades { get { return Instance.playerUpgrades; } }

    public static void Restore(JSONNode node)
    {
        Instance = JsonUtility.FromJson<_GameState>(node.ToString());

        Instance.relics = new RelicContainer(node);
        Instance.bounties = new BountyContainer(node);
        Instance.characters = new CharacterContainer(node);
        Instance.playerUpgrades = new UpgradesContainer(node);

        Instance.player.OnRestore(node);
    }

    public static void Update(JSONNode node)
    {
        Instance.player.Update(node);
        Instance.relics.Update(node);
        Instance.bounties.Update(node);
    }

    public static JSONNode ToJson()
    {
        JSONNode node = JSON.Parse(JsonUtility.ToJson(Instance));

        node["player"] = Instance.player.ToJson();

        node.Add("relics", Relics.ToJson());
        node.Add("bounties", Bounties.ToJson());
        node.Add("characters", Characters.ToJson());
        node.Add("playerUpgrades", PlayerUpgrades.ToJson());

        return node;
    }

    public static bool IsRestored() { return Instance != null; }
}