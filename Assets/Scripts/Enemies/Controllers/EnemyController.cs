

using UnityEngine;

namespace GM.Characters
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] Animator anim;

        void Awake()
        {
            AbstractHealthController hp = GetComponent<AbstractHealthController>();

            hp.E_OnDeath.AddListener(OnZeroHealth);
            hp.E_OnDamageTaken.AddListener(OnDamageTaken);
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