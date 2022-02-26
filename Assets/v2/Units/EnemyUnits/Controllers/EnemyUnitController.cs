using GM.Controllers;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GM.Units.Controllers
{
    public class EnemyUnitController : Units.UnitBaseClass
    {
        [Header("Prefabs")]
        public GameObject DefeatPS;
        public GameObject CoinDropPS;
        [Space]
        public GameObject HealthBarObject;

        [Header("References")]
        public Transform HealthBarTargetTransform;

        [Header("Components")]
        public UnitAvatar UnitAvatar;
        [Space]
        [SerializeField] HealthController healthController;

        void Awake()
        {
            SubscribeToEvents();
        }

        void Start()
        {
            InstantiateHealthBar();
        }

        void InstantiateHealthBar()
        {
            GM.UI.HealthBarController controller = InstantiateUI<GM.UI.HealthBarController>(HealthBarObject);

            controller.AssignHealthController(healthController, HealthBarTargetTransform);
        }

        void SubscribeToEvents()
        {
            var events = GetComponentInChildren<UnitAvatarAnimationEvents>();

            events.Defeat.AddListener(OnDefeatAnimation);

            healthController.E_OnZeroHealth.AddListener(OnZeroHealth);
            healthController.E_OnDamageTaken.AddListener(OnDamageTaken);
        }

        public void OnZeroHealth()
        {
            UnitAvatar.Animator.Play(UnitAvatar.AnimationStrings.Defeat);
            InstantiateCoinDropPS();
        }

        public void OnDamageTaken(BigDouble damageTaken)
        {
            UnitAvatar.Animator.Play(UnitAvatar.AnimationStrings.Hurt);
        }

        public void OnDefeatAnimation()
        {
            UnitAvatar.Animator.enabled = false;
            
            ProcessLoot();
            InstantiateDefeatPS();

            FadeOut(1.0f, () => Destroy(gameObject));
        }

        void ProcessLoot()
        {
            if (TryGetComponent(out GM.Enemies.ILootDrop loot))
            {
                loot.Process();
            }
        }

        protected void FadeOut(float duration, Action action)
        {
            StartCoroutine(FadeEnumerator(duration, action));
        }

        IEnumerator FadeEnumerator(float duration, Action action)
        {
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

            Color[] colors = renderers.Select(sr => sr.color).ToArray();

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                for (int i = 0; i < renderers.Length; ++i)
                {
                    SpriteRenderer sr = renderers[i];

                    sr.color = new Color(colors[i].r, colors[i].g, colors[i].b, colors[i].a * (1 - progress));
                }

                progress += Time.fixedDeltaTime / duration;

                yield return new WaitForFixedUpdate();
            }

            action.Invoke();
        }

        void InstantiateDefeatPS() => Instantiate(DefeatPS, transform.position, Quaternion.identity, null);
        void InstantiateCoinDropPS() => Instantiate(CoinDropPS, Avatar.Bounds.center, Quaternion.identity, null);
    }
}