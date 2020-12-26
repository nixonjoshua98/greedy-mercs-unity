using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;


[System.Serializable]
public class GameState
{
    static _GameState State = null;

    class _GameState
    {
        public StageData stage;

        public PlayerState player = new PlayerState();

        public RelicContainer relics;
        public CharacterContainer characters;
        public PlayerUpgradesContainer playerUpgrades;
    }

    public static PlayerState Player { get { return State.player; } }
    public static StageData Stage { get { return State.stage; } }

    public static RelicContainer Relics { get { return State.relics; } }
    public static CharacterContainer Characters { get { return State.characters; } }
    public static PlayerUpgradesContainer PlayerUpgrades { get { return State.playerUpgrades; } }

    public static void Restore(JSONNode node)
    {
        State = JsonUtility.FromJson<_GameState>(node.ToString());

        State.relics            = new RelicContainer(node);
        State.characters        = new CharacterContainer(node);
        State.playerUpgrades    = new PlayerUpgradesContainer(node);
    }

    public static void Update(JSONNode node)
    {
        State.player.Update(node);

        State.relics.Update(node);
    }

    public static JSONNode ToJson()
    {
        JSONNode node = JSON.Parse(JsonUtility.ToJson(State));

        node["player"] = State.player.ToJson();

        node.Add("relics", Relics.ToJson());
        node.Add("characters", Characters.ToJson());
        node.Add("playerUpgrades", PlayerUpgrades.ToJson());

        return node;
    }

    public static bool IsRestored() { return State != null; }
}