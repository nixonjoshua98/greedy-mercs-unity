
using UnityEngine;
using UnityEngine.Events;

namespace GM.Events
{
    [System.Serializable]
    public class IntegerEvent : UnityEvent<int> { }

    public class GameObjectEvent : UnityEvent<GameObject> { }
}

namespace GreedyMercs.CustomEvents
{
    public class GameObjectEvent : UnityEvent<GameObject> { }

    public class HealthEvent : UnityEvent<Health> { }

    public class CharacterEvent : UnityEvent<CharacterID> { }
}

namespace GreedyMercs
{
    using CustomEvents;

    public class Events : MonoBehaviour
    {
        public static HealthEvent OnEnemyHurt = new HealthEvent();

        public static CharacterEvent OnCharacterUnlocked = new CharacterEvent();

        public static CharacterEvent OnCharacterLevelUp = new CharacterEvent();

        // === Standard Events ===

        public static UnityEvent OnSkillActivated = new UnityEvent();
        public static UnityEvent OnPlayerPrestige = new UnityEvent();

        public static UnityEvent OnPlayerClick = new UnityEvent();

        public static UnityEvent OnStageUpdate = new UnityEvent();

        public static UnityEvent OnNewStageStarted = new UnityEvent();

        public static UnityEvent OnEnemySpawned = new UnityEvent();

        public static UnityEvent OnKillEnemy = new UnityEvent();
    }
}