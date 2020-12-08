using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class StageUpdateEvent : UnityEvent<int, int> { }

public class PlayerCurrencyUpdate : UnityEvent<float, int> { }

public class EventManager : MonoBehaviour
{
    public static UnityEvent OnEnemyDeathFinished = new UnityEvent();

    public static StageUpdateEvent OnStageUpdate = new StageUpdateEvent();

    public static PlayerCurrencyUpdate OnPlayerCurrencyChange = new PlayerCurrencyUpdate();
}
