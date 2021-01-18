
using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class EnemyHealthbar : MonoBehaviour
    {
        [SerializeField] Slider healthbar;
        [SerializeField] Text healthText;

        void FixedUpdate()
        {
            Health hp = GameManager.CurrentEnemyHealth;

            healthText.text = "";
            healthbar.value = 0.0f;

            if (hp != null && hp.CurrentHealth > 0)
            {
                healthText.text = Utils.Format.FormatNumber(hp.CurrentHealth);

                healthbar.value = float.Parse((hp.CurrentHealth / hp.MaxHealth).ToString());
            }
        }
    }
}