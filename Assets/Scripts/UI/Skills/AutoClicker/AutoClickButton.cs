using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Skills.UI
{
    using GM.Targetting;

    public class AutoClickButton : SkillButton
    {
        [Header("Components")]
        [SerializeField] Button ActivateButton;

        [SerializeField] FriendlyTargetter targetter;

        void Awake()
        {
            skill = SkillID.AUTO_CLICK;
        }

        protected override void Activate()
        {
            StartCoroutine(AutoTap());
        }

        IEnumerator AutoTap()
        {
            float lastTap = 0.0f;

            ActivateButton.interactable = false;

            while (SkillsManager.Instance.IsUnlocked(skill) && SkillsManager.Instance.Get(skill).IsActive)
            {
                float seconds = Mathf.Min(1, (Time.timeSinceLevelLoad - lastTap) / 0.1f);

                GameObject target = targetter.GetTarget();

                if (target && target.TryGetComponent(out Health hp))
                {
                    BigDouble dmg = StatsCache.Skills.AutoClickDamage() * seconds;

                    hp.TakeDamage(dmg);
                }

                lastTap = Time.timeSinceLevelLoad;

                yield return new WaitForSeconds(0.1f);
            }

            ActivateButton.interactable = true;
        }
    }
}