
using UnityEngine;
using UnityEngine.Events;

namespace GM.Events
{
    public class BigDoubleEvent : UnityEvent<BigDouble> { }


    [System.Serializable]
    public class IntegerEvent : UnityEvent<int> { }

    public class GameObjectEvent : UnityEvent<GameObject> { }

    public class CharacterEvent : UnityEvent<MercID> { }

    public class GameStateEvent : UnityEvent<CurrentStageState> { }
}

namespace GM.Events
{
    public static class GlobalEvents
    {
        public static GameStateEvent E_OnWaveClear  = new GameStateEvent();

        public static CharacterEvent E_OnMercUnlocked = new CharacterEvent();

        public static CharacterEvent E_OnMercLevelUp = new CharacterEvent();

        // === Standard Events ===

        public static UnityEvent OnSkillActivated = new UnityEvent();
        public static UnityEvent OnPlayerPrestige = new UnityEvent();

        public static UnityEvent OnPlayerClick = new UnityEvent();

        public static UnityEvent OnStageUpdate = new UnityEvent();

        public static UnityEvent OnNewStageStarted = new UnityEvent();
    }
}