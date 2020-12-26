﻿
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents
{
   public class GameObjectEvent : UnityEvent<GameObject> { }

    public class HealthEvent : UnityEvent<Health> { }

    public class HeroEvent : UnityEvent<CharacterID> { }
}


public class EventManager : MonoBehaviour
{
    public static CustomEvents.GameObjectEvent  OnBossSpawned           = new CustomEvents.GameObjectEvent();

    public static CustomEvents.HealthEvent      OnEnemyHurt             = new CustomEvents.HealthEvent();

    public static CustomEvents.HeroEvent        OnHeroUnlocked          = new CustomEvents.HeroEvent();

    public static UnityEvent                    OnStageUpdate           = new UnityEvent();

    public static UnityEvent                    OnNewStageStarted       = new UnityEvent();

    public static UnityEvent                    OnFailedToKillBoss      = new UnityEvent();

    public static UnityEvent                    OnEnemySpawned          = new UnityEvent();

    public static UnityEvent                    OnKilledBoss            = new UnityEvent();
}
