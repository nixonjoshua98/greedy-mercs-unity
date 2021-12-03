
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DamageTextPopup = GM.UI.DamageTextPopup;
using HealthController = GM.Controllers.HealthController;
using GM.Targets;

namespace GM.Units
{
    [RequireComponent(typeof(HealthController))]
    public class EnemyController : UnitController
    {
        protected override TargetList CurrentTargetList => throw new System.NotImplementedException();

        [Header("Prefabs")]
        [SerializeField] GameObject TextPopupObject;

        [SerializeField] Animator anim;

        [Header("Animation Strings")]
        public string hurtAnimation = "Hurt";
        public string deathAnimation = "Dying";

        void Awake()
        {
            FadeIn(0.25f, () => { });
        }


        void Start()
        {
            SubscribeToEvents();
        }


        void SubscribeToEvents()
        {
            HealthController hp = GetComponent<HealthController>(); // Required

            hp.E_OnZeroHealth.AddListener(OnZeroHealth);
            hp.E_OnDamageTaken.AddListener(OnDamageTaken);
        }


        public void OnZeroHealth()
        {
            tag = "Dead";

            anim.Play(deathAnimation);
        }


        public void OnDamageTaken(BigDouble damageTaken)
        {
            anim.Play(hurtAnimation);

            DamageTextPopup popup = InstantiateUI<DamageTextPopup>(TextPopupObject);

            popup.Setup(Format.Number(damageTaken), Camera.main.WorldToScreenPoint(transform.position));
        }


        public void OnDeathAnimation()
        {
            anim.enabled = false;

            ProcessLoot();

            FadeOut(0.5f, () => { Destroy(gameObject); });
        }


        // === //
        void ProcessLoot()
        {
            if (TryGetComponent(out ILootDrop loot))
            {
                loot.Process();
            }
            else
                Debug.LogWarning($"Object {name} did not have a Loot component");
        }

        protected void FadeOut(float duration, UnityAction action)
        {
            StartCoroutine(Fade(duration, action, fadeIn: false));
        }


        protected void FadeIn(float duration, UnityAction action)
        {
            StartCoroutine(Fade(duration, action, fadeIn: true));
        }


        IEnumerator Fade(float duration, UnityAction action, bool fadeIn)
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

        protected override Target GetTargetFromTargetList()
        {
            throw new System.NotImplementedException();
        }
    }
}