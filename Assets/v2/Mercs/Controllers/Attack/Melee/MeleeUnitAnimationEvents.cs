using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public class MeleeUnitAnimationEvents : MonoBehaviour
    {
        [HideInInspector] public UnityEvent AttackImpact = new UnityEvent();

        public void InvokeAttackImpact(AnimationEvent e) => AttackImpact.Invoke();
    }
}
