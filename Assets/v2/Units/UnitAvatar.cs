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
        public AnimationStrings AnimationStrings;

        public Vector3 AvatarCenter => transform.position + (AvatarRect.position * transform.localScale).ToVector3();


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
