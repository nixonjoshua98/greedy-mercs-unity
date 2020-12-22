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

    public static PlayerState player { get { return State.player; } }
    public static StageData stage { get { return State.stage; } }
    public static List<HeroState> heroes { get { return State.heroes; } }

    public static void Restore(JSONNode local)
    {
        State = JsonUtility.FromJson<_GameState>(local.ToString());

        State.player.OnRestored();
    }

    // === Helper Methods ===

    public static string ToJson() { return JsonUtility.ToJson(State); }

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