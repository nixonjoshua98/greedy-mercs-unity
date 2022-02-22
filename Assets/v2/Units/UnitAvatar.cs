using UnityEngine;

namespace GM.Units
{
    public class UnitAvatar : MonoBehaviour
    {
        public BoxCollider2D Collider;

        public Animator Animator;
        public AnimationStrings AnimationStrings;

        public Vector3 MinBounds => Collider.bounds.min;
        public Vector3 MaxBounds => Collider.bounds.max;
        public Vector3 Size => Collider.bounds.size;
        public Vector3 Center => Collider.bounds.center;

        public void PlayAnimation(string anim)
        {
            Animator.Play(anim);
        }
    }
}
