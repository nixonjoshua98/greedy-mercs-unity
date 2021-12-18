using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public class UnitAvatar : MonoBehaviour
    {
        public Animator Animator;
        public Animation_Strings AnimationStrings;

        [HideInInspector] public UnityEvent OnDefeatAnimationEvent = new UnityEvent();
        [HideInInspector] public UnityEvent OnHurtAnimationEvent = new UnityEvent();

        public void OnDefeatAnimation() => OnDefeatAnimationEvent.Invoke();
        public void OnHurtAnimation() => OnHurtAnimationEvent.Invoke();

    }
}
