using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;


[System.Serializable]
public class GameState
{
    static _GameState State = null;

    class _GameState
    {
        public PlayerState player = new PlayerState();

        public List<HeroState> characters = new List<HeroState>();

        public StageData stage;
    }

    public static PlayerState Player { get { return State.player; } }
    public static StageData Stage { get { return State.stage; } }
    public static List<HeroState> Heroes { get { return State.characters; } }

    public static void Restore(JSONNode node)
    {
        State = JsonUtility.FromJson<_GameState>(node.ToString());

        State.player.Restore(node["player"]);
    }

    // === Helper Methods ===

    public static JSONNode ToJson()
    {
        JSONNode node = JSON.Parse(JsonUtility.ToJson(State));

        node["player"] = State.player.ToJson();

        return node;
    }

    public static bool IsRestored() { return State != null; }

    public static HeroState GetHeroState(CharacterID heroId)
    {
        foreach (HeroState state in State.characters)
        {
            if (state.heroId == heroId)
                return state;
        }

        return null;
    }

    public static bool TryGetHeroState(CharacterID heroId, out HeroState result)
    {
        result = GetHeroState(heroId);

        return result != null;
    }
}