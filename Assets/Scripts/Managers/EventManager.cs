using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class StageUpdateEvent : UnityEvent<int, int> { }

public class EventManager : MonoBehaviour
{
    public static StageUpdateEvent OnStageUpdate = new StageUpdateEvent();
}
