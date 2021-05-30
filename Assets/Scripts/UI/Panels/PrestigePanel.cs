using System.Numerics;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;


namespace GreedyMercs
{
    using GreedyMercs.StageDM.Prestige;

    public class PrestigePanel : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject PrestigeControllerObject;

        [SerializeField] Text prestigePointText;

        [SerializeField] RectTransform lootBagRect;

        bool currentlyPrestiging;

        void Awake()
        {
            currentlyPrestiging = false;
        }


        void FixedUpdate()
        {
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
                Animation(1.0f);
            }
        }

        void Animation(float duration)
        {
            StartCoroutine(Utils.Lerp.RectTransform(lootBagRect, lootBagRect.localScale, lootBagRect.localScale * 2, duration));
        }
    }
}