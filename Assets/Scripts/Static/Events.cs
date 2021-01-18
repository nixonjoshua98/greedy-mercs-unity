
using UnityEngine;
using UnityEngine.Events;

using CustomEvents;

namespace CustomEvents
{
    using Data.Characters;

    public class GameObjectEvent : UnityEvent<GameObject> { }

    public class HealthEvent : UnityEvent<Health> { }

    public class CharacterEvent : UnityEvent<CharacterID> { }

    public class ScriptableCharacterEvent : UnityEvent<CharacterSO, int> { }
}


public class Events : MonoBehaviour
{
    public static GameObjectEvent               OnBossSpawned       = new GameObjectEvent();

    public static HealthEvent                   OnEnemyHurt         = new HealthEvent();

    public static CharacterEvent                OnCharacterUnlocked = new CharacterEvent();

    public static ScriptableCharacterEvent      OnWeaponBought      = new ScriptableCharacterEvent();

    public static CharacterEvent                OnCharacterLevelUp  = new CharacterEvent();

    // === Standard Events ===

    public static UnityEvent                    OnPlayerTap             = new UnityEvent();

    public static UnityEvent                    OnStageUpdate           = new UnityEvent();

    public static UnityEvent                    OnNewStageStarted       = new UnityEvent();

    public static UnityEvent                    OnFailedToKillBoss      = new UnityEvent();

    public static UnityEvent                    OnEnemySpawned          = new UnityEvent();

    public static UnityEvent                    OnKilledBoss            = new UnityEvent();

    public static UnityEvent                    OnKillEnemy             = new UnityEvent();
}
