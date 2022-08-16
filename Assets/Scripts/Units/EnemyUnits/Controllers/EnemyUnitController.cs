using SRC.Controllers;
using UnityEngine;

namespace SRC.Units.Controllers
{
    public class EnemyUnitController : SRC.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject DefeatPS;
        public MovementController Movement;

        [Header("Components")]
        public UnitAvatar UnitAvatar;

        [Space]
        [SerializeField] protected HealthController HealthController;

        void Awake()
        {
            Movement.LookAtPosition(Camera.main.transform.position);
        }

        private void Start()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            UnitAvatar.E_Anim_OnDefeat.AddListener(OnDefeatAnimation);

            HealthController.E_OnZeroHealth.AddListener(OnZeroHealth);
            HealthController.E_OnDamageTaken.AddListener(OnDamageTaken);
        }

        public void OnZeroHealth()
        {
            UnitAvatar.Animator.Play(UnitAvatar.Animations.Defeat);
        }

        public void OnDamageTaken(BigDouble damageTaken)
        {
            UnitAvatar.Animator.Play(UnitAvatar.Animations.Hurt);
        }

        public void OnDefeatAnimation()
        {
            UnitAvatar.Animator.enabled = false;

            Instantiate(DefeatPS, transform.position, Quaternion.identity);

            UnitAvatar.Fade(0.3f, () => Destroy(gameObject));
        }
    }
}