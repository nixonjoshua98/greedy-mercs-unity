
using UnityEngine;

using DamageTextPopup = GM.UI.DamageTextPopup;


namespace GM.Units
{
    [RequireComponent(typeof(HealthController))]
    public class EnemyController : UnitController
    {
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

            DamageTextPopup popup = CanvasUtils.Instantiate<DamageTextPopup>(TextPopupObject);

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
    }
}