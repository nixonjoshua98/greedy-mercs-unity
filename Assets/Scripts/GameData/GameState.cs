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
        if (State == null)
            State = RestoreFromLocalData(local);
    }

    public static void Restore(JSONNode local, JSONNode server)
    {
        if (State == null)
        {
            State = RestoreFromLocalData(local);

            UpdateWithServerData(server);
        }
    }

    public static string ToJson()
    {
        return JsonUtility.ToJson(State);
    }

    // === Internal Methods ===

    static _GameState RestoreFromLocalData(JSONNode local)
    {
        return JsonUtility.FromJson<_GameState>(local.ToString());
    }

    static void UpdateWithServerData(JSONNode server)
    {
        player.maxStageReward = server["maxStageReward"].AsInt;

        Dictionary<HeroID, HeroState> serverHeroDataDict = new Dictionary<HeroID, HeroState>();

        // Add heroes from server which are not in the local game state
        foreach (JSONNode serverHero in server["heroes"].AsArray)
        {
            HeroID heroId = (HeroID)serverHero["heroId"].AsInt;

            HeroState heroState = JsonUtility.FromJson<HeroState>(serverHero.ToString());

            if (!GameState.TryGetHeroState(heroId, out HeroState _))
                GameState.heroes.Add(heroState);

            serverHeroDataDict[heroId] = heroState;
        }

        // Remove heroes which are stored locally but not on the server
        for (int i = 0; i < GameState.heroes.Count;)
        {
            HeroState localHeroData = GameState.heroes[i];

            // Player has a hero which is not on the server
            if (!serverHeroDataDict.TryGetValue(localHeroData.heroId, out HeroState _))
                GameState.heroes.RemoveAt(i);

            else
                ++i;
        }
    }

    // === Helper Methods ===
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