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
            StartTextUpdate(GoldText, () => Format.Number(App.Data.Inv.Gold));
            StartTextUpdate(BountyPointsText, () => Format.Number(App.Data.Inv.BountyPoints));
            StartTextUpdate(ArmouryPointsText, () => Format.Number(App.Data.Inv.ArmouryPoints));
            StartTextUpdate(PrestigePointsText, () => Format.Number(App.Data.Inv.PrestigePoints));
        }

        void Start()
        {
            App.Data.Inv.E_BountyPointsChange.AddListener((change) => { Instantiate<QuantityPopup>(ItemTextPopup, BountyPointsText.transform.parent.position).Set(change); });
            App.Data.Inv.E_PrestigePointsChange.AddListener((change) => { Instantiate<QuantityPopup>(ItemTextPopup, PrestigePointsText.transform.parent.position).Set(change); });
        }

        void StartTextUpdate(TMP_Text txt, Func<string> action) => StartCoroutine(TextUpdateLoop(txt, action));

        IEnumerator TextUpdateLoop(TMP_Text txt, Func<string> action)
        {
            while (Application.isPlaying)
            {
                if (txt.gameObject.activeInHierarchy)
                {
                    txt.text = action();
                }

                yield return new WaitForSecondsRealtime(1.0f);
            }
        }
    }
}
