using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public class UnitAvatar : MonoBehaviour
    {
        [SerializeField] BoxCollider2D Collider;

        public Animator Animator;
        public AnimationStrings Animations;

        [HideInInspector] public UnityEvent E_Anim_MeleeAttackImpact = new UnityEvent();
        [HideInInspector] public UnityEvent E_Anim_MeleeAttackFinished = new UnityEvent();

        [HideInInspector] public UnityEvent E_Anim_OnDefeat = new UnityEvent();
        [HideInInspector] public UnityEvent E_Anim_OnHurt = new UnityEvent();

        public Bounds Bounds {  get { return Collider.bounds; } }

        public void PlayAnimation(string anim)
        {
            Animator.Play(anim);
        }

        // = Animation Callbacks = //

        public void Animation_MeleeAttackImpact() => E_Anim_MeleeAttackImpact.Invoke();
        public void Animation_MeleeAttackFinished() => E_Anim_MeleeAttackFinished.Invoke();
        public void Animation_OnDefeat() => E_Anim_OnDefeat.Invoke();
        public void Animation_OnHurt() => E_Anim_OnHurt.Invoke();

    }
}
