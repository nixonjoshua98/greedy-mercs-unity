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

        public Dictionary<RelicID, UpgradeState> relics;

        public Dictionary<CharacterID, UpgradeState> characters;
    }

    public static PlayerState Player { get { return State.player; } }
    public static StageData Stage { get { return State.stage; } }

    // === Accessors ===
    public static int NumRelicsOwned { get { return State.relics.Count; } }
    // ===

    public static void Restore(JSONNode node)
    {
        State = JsonUtility.FromJson<_GameState>(node.ToString());

        State.characters    = CreateCharacterDictionary(node);
        State.relics        = CreateRelicDictionary(node);

        State.player.Restore(node["player"]);
    }

    public static JSONNode ToJson()
    {
        JSONNode node = JSON.Parse(JsonUtility.ToJson(State));

        node["player"] = State.player.ToJson();

        node.Add("characters", Utils.Json.CreateJSONArray("characterId", State.characters));

        return node;
    }

    // === Characters
    
    public static UpgradeState GetCharacter(CharacterID heroId) { return State.characters[heroId]; }

    public static bool TryGetHeroState(CharacterID heroId, out UpgradeState result) { return State.characters.TryGetValue(heroId, out result); }

    public static void AddHero(CharacterID charaId) { State.characters[charaId] = new UpgradeState { level = 1 }; }

    public static Dictionary<RelicID, UpgradeState> CreateRelicDictionary(JSONNode node)
    {
        return new Dictionary<RelicID, UpgradeState>();
    }

    public static Dictionary<CharacterID, UpgradeState> CreateCharacterDictionary(JSONNode node)
    {
        Dictionary<CharacterID, UpgradeState> characters = new Dictionary<CharacterID, UpgradeState>();

        foreach (JSONNode chara in node["characters"].AsArray)
        {
            characters[(CharacterID)int.Parse(chara["characterId"])] = JsonUtility.FromJson<UpgradeState>(chara.ToString());
        }

        return characters;
    }

    // ===

    public static bool IsRestored() { return State != null; }
}