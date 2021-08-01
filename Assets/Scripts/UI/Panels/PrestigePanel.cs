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
            CurrentStageState state = GameManager.Instance.State();

            prestigePointText.text = FormatString.Number(StatsCache.GetPrestigePoints(state.Stage));
        }


        public void Prestige()
        {
            CurrentStageState state = GameManager.Instance.State();

            if (currentlyPrestiging || state.Stage < StaticData.MIN_PRESTIGE_STAGE)
                return;

            currentlyPrestiging = true;

            GameObject o = Instantiate(PrestigeControllerObject);

            if (o.TryGetComponent(out PrestigeController controller))
            {
                controller.Prestige((_) => { });
            }
        }
    }
}