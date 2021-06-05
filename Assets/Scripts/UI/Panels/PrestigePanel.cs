using System.Numerics;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;


namespace GM
{
    using GM.StageDM.Prestige;

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
            C_GameState state = GameManager.Instance.GetState();

            prestigePointText.text = Utils.Format.FormatNumber(StatsCache.GetPrestigePoints(state.currentStage));
        }

        public void Prestige()
        {
            C_GameState state = GameManager.Instance.GetState();

            if (currentlyPrestiging || state.currentStage < StaticData.MIN_PRESTIGE_STAGE)
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