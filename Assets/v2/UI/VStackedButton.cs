using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace GM.UI
{
    public class VStackedButton : MonoBehaviour
    {
        public TMP_Text TopText;
        public TMP_Text BtmText;

        Button Button;

        void Awake()
        {
            this.Button = GetComponent<Button>();
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