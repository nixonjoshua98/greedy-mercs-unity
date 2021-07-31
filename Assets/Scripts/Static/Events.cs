
using UnityEngine;
using UnityEngine.Events;

namespace GM.Events
{
    public class BigDoubleEvent : UnityEvent<BigDouble> { }
    public class IntegerEvent : UnityEvent<int> { }
    public class GameObjectEvent : UnityEvent<GameObject> { }
    public class CharacterEvent : UnityEvent<MercID> { }
}


namespace GM.Events
{
    public static class GlobalEvents
    {
        public static CharacterEvent E_OnMercUnlocked { get; set; } = new CharacterEvent();
        public static CharacterEvent E_OnMercLevelUp { get; set; } = new CharacterEvent();
    }
}