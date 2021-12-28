using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace GM.UI
{
    public class VStackedButton : MonoBehaviour
    {
        public TMP_Text TopText;
        public TMP_Text BtmText;

        Button _Button;
        Button Button
        {
            get
            {
                if (_Button == null)
                    _Button = GetComponent<Button>();
                return _Button;
            }
        }

        public void SetText(string top, string btm)
        {
            TopText.text = top;
            BtmText.text = btm;
        }

        public bool interactable
        {
            get => Button.interactable;
            set => Button.interactable = value;
        }
    }
}