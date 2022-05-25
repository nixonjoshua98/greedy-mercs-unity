using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public class UnitAvatar : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D Collider;

        public Animator Animator;
        public AnimationStrings Animations;

        [HideInInspector] public UnityEvent E_Anim_MeleeAttackImpact = new UnityEvent();
        [HideInInspector] public UnityEvent E_Anim_MeleeAttackFinished = new UnityEvent();

        [HideInInspector] public UnityEvent E_Anim_OnDefeat = new UnityEvent();
        [HideInInspector] public UnityEvent E_Anim_OnHurt = new UnityEvent();

        public Bounds Bounds => Collider.bounds;

        public void PlayAnimation(string anim)
        {
            if (!Animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
            {
                Animator.Play(anim);
            }
        }

        // = Animation Callbacks = //

        public void Animation_MeleeAttackImpact()
        {
            E_Anim_MeleeAttackImpact.Invoke();
        }

        public void Animation_MeleeAttackFinished()
        {
            E_Anim_MeleeAttackFinished.Invoke();
        }

        public void Animation_OnDefeat()
        {
            E_Anim_OnDefeat.Invoke();
        }

        public void Animation_OnHurt()
        {
            E_Anim_OnHurt.Invoke();
        }

        // = Helper = //

        public Vector3 GetClosestBounds(UnitAvatar other)
        {
            // Target is LEFT
            if (Bounds.min.x > other.Bounds.max.x)
            {
                return new Vector3(other.Bounds.max.x, transform.position.y);
            }
            // Target is RIGHT
            else
            {
                return new Vector3(other.Bounds.min.x, transform.position.y);
            }
        }

        public float DistanceBetweenAvatar(UnitAvatar other)
        {
            return Vector2.Distance(GetClosestBounds(other), other.GetClosestBounds(this));
        }

        public float DistanceXBetweenAvatar(UnitAvatar other)
        {
            return Mathf.Abs(GetClosestBounds(other).x - other.GetClosestBounds(this).x);
        }

    }
}
