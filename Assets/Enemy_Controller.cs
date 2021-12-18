using GM.Controllers;
using GM.Units;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GM.Enemies.Controllers
{
    public class Enemy_Controller : MonoBehaviour
    {
        [Header("Components")]
        public UnitAvatar UnitAvatar;
        [Space]
        [SerializeField] HealthController healthController;

        [Header("References")]
        [SerializeField] GameObject DamageTakenPS;

        void Start()
        {
            SubscribeToEvents();
        }

        void SubscribeToEvents()
        {
            UnitAvatar.OnDefeatAnimationEvent.AddListener(OnDefeatAnimation);

            healthController.E_OnZeroHealth.AddListener(OnZeroHealth);
            healthController.E_OnDamageTaken.AddListener(OnDamageTaken);
        }

        public void OnZeroHealth()
        {
            UnitAvatar.Animator.Play(UnitAvatar.AnimationStrings.Defeat);
        }

        public void OnDamageTaken(BigDouble damageTaken)
        {
            UnitAvatar.Animator.Play(UnitAvatar.AnimationStrings.Hurt);
        }

        public void OnDefeatAnimation()
        {
            UnitAvatar.Animator.enabled = false;

            ProcessLoot();

            FadeOut(0.5f, () => { Destroy(gameObject); });
        }

        void ProcessLoot()
        {
            if (TryGetComponent(out ILootDrop loot))
            {
                loot.Process();
            }
        }

        protected void FadeOut(float duration, Action action)
        {
            StartCoroutine(Fade(duration, action, fadeIn: false));
        }


        protected void FadeIn(float duration, Action action)
        {
            StartCoroutine(Fade(duration, action, fadeIn: true));
        }


        IEnumerator Fade(float duration, Action action, bool fadeIn)
        {
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

            Color[] colors = renderers.Select(sr => sr.color).ToArray();

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                for (int i = 0; i < renderers.Length; ++i)
                {
                    SpriteRenderer sr = renderers[i];

                    sr.color = new Color(colors[i].r, colors[i].g, colors[i].b, colors[i].a * (fadeIn ? progress : 1 - progress));
                }

                progress += Time.fixedDeltaTime / duration;

                yield return new WaitForFixedUpdate();
            }

            action.Invoke();
        }
    }
}
