using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class StackedButton : MonoBehaviour
    {
        public TMP_Text TopText;
        public TMP_Text BtmText;
        private Button _Button;

        [Tooltip("Optional")]
        public Image Icon;

        private Button Button
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