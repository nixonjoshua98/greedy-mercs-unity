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

        [HideInInspector] public UnityEvent OnDefeatAnimationEvent { get; set; } = new UnityEvent();
        [HideInInspector] public UnityEvent OnHurtAnimationEvent { get; set; } = new UnityEvent();

        public Vector3 AvatarCenter => transform.position + (AvatarRect.position * transform.localScale).ToVector3();

        public void OnDefeatAnimation() => OnDefeatAnimationEvent.Invoke();
        public void OnHurtAnimation() => OnHurtAnimationEvent.Invoke();


        public void PlayAnimation(string anim)
        {
            Animator.Play(anim);
        }

        void OnDrawGizmos()
        {
            AvatarRect.DrawGizmos(transform.position, transform.localScale);
        }
    }
}
