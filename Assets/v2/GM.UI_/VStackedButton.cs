using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GM.UI_
{
    public class VStackedButton : MonoBehaviour
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
