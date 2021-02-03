using System.Numerics;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SimpleJSON;

namespace GreedyMercs
{
    using GreedyMercs.StageDM.Prestige;

    public class PrestigePanel : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject PrestigeControllerObject;

        [SerializeField] Text prestigePointText;
        [SerializeField] Text bountyLevelsText;

        [SerializeField] RectTransform lootBagRect;

        bool currentlyPrestiging;

        void Awake()
        {
            currentlyPrestiging = false;

            UpdatePanel();

            InvokeRepeating("UpdatePanel", 0.1f, 0.1f);
        }


        void UpdatePanel()
        {
            bountyLevelsText.text = GameState.Bounties.GetPrestigeBountyLevels().ToString();

            prestigePointText.text = Utils.Format.FormatNumber(StatsCache.GetPrestigePoints(GameState.Stage.stage));
        }

        public void Prestige()
        {
            if (currentlyPrestiging || GameState.Stage.stage < StageState.MIN_PRESTIGE_STAGE)
                return;

            currentlyPrestiging = true;

            GameObject o = Instantiate(PrestigeControllerObject);

            if (o.TryGetComponent(out PrestigeController controller))
            {
                controller.Prestige(OnPrestige);
            }
        }

        void OnPrestige(bool success)
        {
            if (success)
            {
                StartCoroutine(PanelAnimation(1.0f));
            }
        }

        IEnumerator PanelAnimation(float duration)
        {
            StartCoroutine(Utils.Lerp.RectTransform(lootBagRect, lootBagRect.localScale, lootBagRect.localScale * 2, duration));

            BigInteger coins = StatsCache.GetPrestigePoints(GameState.Stage.stage);

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                BigInteger temp = (coins.ToBigDouble() * (1 - progress)).ToBigInteger();

                prestigePointText.text = Utils.Format.FormatNumber(temp);

                progress += (Time.deltaTime / duration);

                yield return new WaitForEndOfFrame();
            }

            prestigePointText.text = "0";
        }
    }
}