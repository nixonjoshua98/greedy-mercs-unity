using TMPro;
using UnityEngine;

namespace GM.UI.InfoPopups
{
    public class InfoModal : GM.UI.PopupBase
    {
        [Header("Components")]
        [SerializeField] TMP_Text Title;
        [SerializeField] TMP_Text Message;

        public void Set(string title, string message)
        {
            Title.text = title;
            Message.text = message;

            ShowInnerPanel();
        }

        public void ButtonUI_OnOkButton()
        {
            Destroy(gameObject);
        }
    }
}
