using TMPro;
using UnityEngine.UI;

namespace GM.UI
{
    public class VStackedButton : Button
    {
        public TMP_Text TopText;
        public TMP_Text BtmText;

        public void SetText(string top, string btm)
        {
            TopText.text = top;
            BtmText.text = btm;
        }
    }
}
