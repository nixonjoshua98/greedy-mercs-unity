using TMPro;
using UnityEngine;

namespace SRC.UI
{
    public class PillBadge : MonoBehaviour
    {
        [SerializeField] private GameObject InnerObject;
        [SerializeField] private TMP_Text Text;

        public void Show(string txt = "!")
        {
            _Show(txt);
        }

        public void Show(int value)
        {
            _Show(value > 9 ? "9+" : value.ToString());
        }

        private void _Show(string txt)
        {
            Text.text = txt;
            InnerObject.SetActive(true);
        }

        public void Hide()
        {
            InnerObject.SetActive(false);
        }
    }
}
