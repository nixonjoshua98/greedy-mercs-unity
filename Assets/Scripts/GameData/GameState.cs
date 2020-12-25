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

        public List<HeroState> heroes = new List<HeroState>();

        public StageData stage;
    }

    public static PlayerState Player { get { return State.player; } }
    public static StageData Stage { get { return State.stage; } }
    public static List<HeroState> Heroes { get { return State.heroes; } }

    public static void Restore(JSONNode local)
    {
        State = JsonUtility.FromJson<_GameState>(local.ToString());

        State.player.Restore(local["player"]);
    }

    // === Helper Methods ===

    public static JSONNode ToJson()
    {
        JSONNode node = JSON.Parse(JsonUtility.ToJson(State));

        node["player"] = State.player.ToJson();

        return node;
    }

    public static bool IsRestored() { return State != null; }

    public static HeroState GetHeroState(HeroID heroId)
    {
        foreach (HeroState state in State.heroes)
        {
            if (state.heroId == heroId)
                return state;
        }

        return null;
    }

    public static bool TryGetHeroState(HeroID heroId, out HeroState result)
    {
        result = GetHeroState(heroId);

        return result != null;
    }
}