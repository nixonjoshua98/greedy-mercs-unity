using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI_
{
    public class VStackedButton : MonoBehaviour
    {
        public TMP_Text TopText;
        public TMP_Text BtmText;
        [Space]
        public Button UIButton; // Avoid clash of Button

        public void SetText(string top, string btm)
        {
            TopText.text = top;
            BtmText.text = btm;
        }

        public bool interactable
        {
            get => UIButton.interactable;
            set => UIButton.interactable = value;
        }
    }
}
