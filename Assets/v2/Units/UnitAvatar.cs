using UnityEngine;

namespace GM.Units
{
    public class UnitAvatar : MonoBehaviour
    {
        [SerializeField] BoxCollider2D Collider;

        public Animator Animator;
        public AnimationStrings Animations;

        public Bounds Bounds {  get { return Collider.bounds; } }

        public void PlayAnimation(string anim)
        {
            Animator.Play(anim);
        }
    }
}
