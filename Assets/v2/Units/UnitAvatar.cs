using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public class UnitAvatar : MonoBehaviour
    {
        public Animator Animator;
        public Rect AvatarRect;
        public Animation_Strings AnimationStrings;

        [HideInInspector] public UnityEvent OnDefeatAnimationEvent = new UnityEvent();
        [HideInInspector] public UnityEvent OnHurtAnimationEvent = new UnityEvent();
        [HideInInspector] public UnityEvent OnAttackAnimationEvent = new UnityEvent();

        public Vector3 AvatarCenter => transform.position + (AvatarRect.position * transform.localScale).ToVector3();

        public void OnDefeatAnimation() => OnDefeatAnimationEvent.Invoke();
        public void OnHurtAnimation() => OnHurtAnimationEvent.Invoke();
        public void OnAttackAnimation() => OnAttackAnimationEvent.Invoke();

        void OnDrawGizmos()
        {
            AvatarRect.DrawGizmos(transform.position, transform.localScale);
        }
    }
}
