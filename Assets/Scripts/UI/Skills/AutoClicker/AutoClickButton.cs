using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Skills.UI
{
    public class AutoClickButton : SkillButton
    {
        [Header("Components")]
        [SerializeField] Button ActivateButton;

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

                GameManager.TryDealDamageToEnemy(StatsCache.Skills.AutoClickDamage() * seconds);

                lastTap = Time.timeSinceLevelLoad;

                yield return new WaitForSeconds(0.1f);
            }

            ActivateButton.interactable = true;
        }
    }
}