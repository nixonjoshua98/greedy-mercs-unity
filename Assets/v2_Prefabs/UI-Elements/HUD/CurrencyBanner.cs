using TMPro;
using UnityEngine;
using System.Collections;

namespace GM.UI_
{
    public class CurrencyBanner : Core.GMMonoBehaviour
    {
        public TMP_Text GoldText;
        public TMP_Text PrestigePointsText;

        IEnumerator Start()
        {
            while (Application.isPlaying)
            {
                GoldText.text = FormatString.Number(App.Data.Inv.Gold);
                PrestigePointsText.text = FormatString.Number(App.Data.Inv.PrestigePoints);

                yield return new WaitForSecondsRealtime(1.0f);
            }
        }
    }
}
