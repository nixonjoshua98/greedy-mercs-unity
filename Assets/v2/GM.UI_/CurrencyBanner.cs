using TMPro;
using UnityEngine;
using System;
using System.Collections;

namespace GM.UI_
{
    public class CurrencyBanner : Core.GMMonoBehaviour
    {
        public TMP_Text GoldText;
        public TMP_Text PrestigePointsText;

        void Start()
        {
            StartTextUpdate(GoldText, () => FormatString.Number(App.Data.Inv.Gold), () => true);
            StartTextUpdate(PrestigePointsText, () => FormatString.Number(App.Data.Inv.PrestigePoints), () => true);
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
