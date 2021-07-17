using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    using GM.UI;

    public class EnemyController : UnitController
    {
        [SerializeField] Animator anim;

        [SerializeField] HealthController health;

        [Header("Animation Strings")]
        public string hurtAnimation = "Hurt";
        public string deathAnimation = "Dying";


        void Start()
        {
            SubscribeToEvents();
        }


        void SubscribeToEvents()
        {
            health.E_OnDeath.AddListener(OnZeroHealth);
            health.E_OnDamageTaken.AddListener(OnDamageTaken);
        }


        public void OnZeroHealth()
        {
            tag = "Dead";

            anim.Play(deathAnimation);
        }


        public void OnDamageTaken(BigDouble damageTaken)
        {
            anim.Play(hurtAnimation);

            DamagePopup.Create(transform.position, FormatString.Number(damageTaken), worldPosition: true);
        }


        public void OnDeathAnimation()
        {
            anim.enabled = false;

            FadeOut(0.5f, () => { Destroy(gameObject); });
        }
    }
}