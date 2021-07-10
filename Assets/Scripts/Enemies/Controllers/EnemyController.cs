

using UnityEngine;

namespace GM.Units
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] Animator anim;

        AbstractHealthController healthController;

        void Awake()
        {
            healthController = GetComponent<AbstractHealthController>();

            healthController.E_OnDeath.AddListener(OnZeroHealth);
            healthController.E_OnDamageTaken.AddListener(OnDamageTaken);
        }

        public void OnZeroHealth(GameObject obj)
        {
            tag = "Dead";

            Destroy(gameObject, 0.1f);
        }

        public void OnDamageTaken()
        {
            anim.Play("Hurt");
        }
    }
}