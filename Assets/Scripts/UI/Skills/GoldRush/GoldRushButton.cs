using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Skills.UI
{
    public class GoldRushButton : SkillButton
    {
        [Header("Components")]
        [SerializeField] Button ActivateButton;

        [Header("Prefabs")]
        [SerializeField] GameObject CoinShowerPS;

        void Awake()
        {
            skill = SkillID.GOLD_RUSH;
        }

        protected override void Activate()
        {
            StartCoroutine(Loop());
        }

        IEnumerator Loop()
        {
            ActivateButton.interactable = false;

            ParticleSystem ps = Instantiate(CoinShowerPS).GetComponent<ParticleSystem>();

            yield return new WaitWhile(() => { return GameState.Skills.IsUnlocked(skill) && GameState.Skills.Get(skill).IsActive; });

            ps.Stop();

            yield return new WaitUntil(() => { return ps.particleCount == 0; });

            Destroy(ps.gameObject);

            ActivateButton.interactable = true;
        }
    }
}