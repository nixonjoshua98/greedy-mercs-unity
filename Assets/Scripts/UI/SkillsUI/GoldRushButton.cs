using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class GoldRushButton : MonoBehaviour
    {
        [SerializeField] SkillID skill;

        [Header("Components")]
        [SerializeField] Button ActivateButton;

        [Header("Prefabs")]
        [SerializeField] GameObject CoinShowerPS;

        public void OnClick()
        {
            SkillState state = GameState.Skills.Get(skill);

            if (!state.IsActive)
            {
                SkillLevel levelData = state.LevelData;

                if (GameState.Player.currentEnergy >= levelData.EnergyCost)
                {
                    GameState.Player.currentEnergy -= levelData.EnergyCost;

                    GameState.Skills.ActivateSkill(skill);

                    Activate();
                }
            }
        }

        void Activate()
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