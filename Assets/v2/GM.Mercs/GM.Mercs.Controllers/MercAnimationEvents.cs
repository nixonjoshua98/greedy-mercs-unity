using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Mercs.Controllers
{
    public class MercAnimationEvents : MonoBehaviour
    {
        public UnityEvent E_OnAttackAnimation;

        public void InvokeAttackEvent() => E_OnAttackAnimation.Invoke();
    }
}
