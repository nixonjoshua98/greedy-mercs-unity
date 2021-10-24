using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GM.UI
{
    public class CurrencyBanner : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ItemTextPopup;

        [Header("References")]
        public TMP_Text GoldText;
        public TMP_Text BountyPointsText;
        public TMP_Text ArmouryPointsText;
        public TMP_Text PrestigePointsText;

        void Awake()
        {
            StartTextUpdate(GoldText, () => FormatString.Number(App.Data.Inv.Gold), () => true);
            StartTextUpdate(BountyPointsText, () => FormatString.Number(App.Data.Inv.BountyPoints), () => true);
            StartTextUpdate(ArmouryPointsText, () => FormatString.Number(App.Data.Inv.ArmouryPoints), () => true);
            StartTextUpdate(PrestigePointsText, () => FormatString.Number(App.Data.Inv.PrestigePoints), () => true);
        }

        void Start()
        {
            App.Events.E_BountyPointsChange.AddListener((change) => { Instantiate<QuantityPopup>(ItemTextPopup, BountyPointsText.transform.parent.position).Set(change); });
            App.Events.E_PrestigePointsChange.AddListener((change) => { Instantiate<QuantityPopup>(ItemTextPopup, PrestigePointsText.transform.parent.position).Set(change); });
        }

        void StartTextUpdate(TMP_Text txt, Func<string> action, Func<bool> check) => StartCoroutine(TextUpdateLoop(txt, action, check));

        IEnumerator TextUpdateLoop(TMP_Text txt, Func<string> action, Func<bool> check)
        {
            while (Application.isPlaying)
            {
                if (check())
                {
                    txt.text = action();
                }

                yield return new WaitForSecondsRealtime(1.0f);
            }
        }
    }
}
