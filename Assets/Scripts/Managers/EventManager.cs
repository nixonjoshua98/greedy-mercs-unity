using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents
{
    public class StageUpdateEvent : UnityEvent<int, int> { }
    public class GameObjectEvent : UnityEvent<GameObject> { }
}

public class EventManager : MonoBehaviour
{
    public static CustomEvents.StageUpdateEvent OnStageUpdate = new CustomEvents.StageUpdateEvent();

    public static CustomEvents.GameObjectEvent OnBossSpawned = new CustomEvents.GameObjectEvent();

    public static UnityEvent OnFailedToKillBoss = new UnityEvent();
}
