﻿
using UnityEngine;
using UnityEngine.Events;

namespace GreedyMercs.CustomEvents
{
    public class GameObjectEvent : UnityEvent<GameObject> { }

    public class HealthEvent : UnityEvent<Health> { }

    public class CharacterEvent : UnityEvent<CharacterID> { }

    public class IntegerEvent : UnityEvent<int> { }
}

namespace GreedyMercs
{
    using CustomEvents;

    public class Events : MonoBehaviour
    {
        public static GameObjectEvent OnBossSpawned = new GameObjectEvent();

        public static HealthEvent OnEnemyHurt = new HealthEvent();

        public static CharacterEvent OnCharacterUnlocked = new CharacterEvent();

        public static CharacterEvent OnCharacterLevelUp = new CharacterEvent();

        // === Standard Events ===

        public static UnityEvent OnSkillActivated = new UnityEvent();
        public static UnityEvent OnPlayerPrestige = new UnityEvent();

        public static UnityEvent OnPlayerClick = new UnityEvent();

        public static UnityEvent OnStageUpdate = new UnityEvent();

        public static UnityEvent OnNewStageStarted = new UnityEvent();

        public static UnityEvent OnFailedToKillBoss = new UnityEvent();

        public static UnityEvent OnEnemySpawned = new UnityEvent();

        public static UnityEvent OnKilledBoss = new UnityEvent();

        public static UnityEvent OnKillEnemy = new UnityEvent();
    }
}