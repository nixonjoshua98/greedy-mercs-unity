using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class UnitAvatarAnimationEvents : MonoBehaviour
    {
        [HideInInspector] public UnityEvent Attack = new UnityEvent();

        public void InvokeAttackEvent() => Attack.Invoke();
    }
}
