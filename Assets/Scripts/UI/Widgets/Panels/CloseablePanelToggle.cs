
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class CloseablePanelToggle : MonoBehaviour
    {
        public CloseablePanel panel;

        void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
        }


        void OnValueChanged(bool val)
        {
            panel.Toggle(val);
        }
    }
}
