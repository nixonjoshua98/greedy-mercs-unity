using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SRC.Units
{
    public sealed class UnitAvatar : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D Collider;

        public Animator Animator;
        public AnimationStrings Animations;

        [HideInInspector] public UnityEvent E_Anim_MeleeAttackImpact = new();
        [HideInInspector] public UnityEvent E_Anim_MeleeAttackFinished = new();

        [HideInInspector] public UnityEvent E_Anim_OnDefeat = new();
        [HideInInspector] public UnityEvent E_Anim_OnHurt = new();

        public Bounds Bounds => Collider.bounds;

        public void Fade(float duration, Action callback)
        {
            StartCoroutine(FadeEnumerator(duration, callback));
        }

        IEnumerator FadeEnumerator(float duration, Action callback)
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            var colors = renderers.Select(sr => sr.color).ToArray();

            Vector3 initialScale = transform.localScale;

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                for (int i = 0; i < renderers.Length; ++i)
                {
                    SpriteRenderer sr = renderers[i];

                    sr.color = new Color(colors[i].r, colors[i].g, colors[i].b, colors[i].a * (1 - progress));
                }

                transform.localScale = initialScale * (1 - progress);

                progress += Time.fixedDeltaTime / duration;

                yield return new WaitForFixedUpdate();
            }

            callback.Invoke();
        }

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
            E_Anim_MeleeAttackImpact?.Invoke();
        }

        public void Animation_MeleeAttackFinished()
        {
            E_Anim_MeleeAttackFinished?.Invoke();
        }

        public void Animation_OnDefeat()
        {
            E_Anim_OnDefeat?.Invoke();
        }

        public void Animation_OnHurt()
        {
            E_Anim_OnHurt?.Invoke();
        }
    }
}
