using UnityEngine;
using UnityEngine.UI;

namespace GM.UI.Panels
{
    [RequireComponent(typeof(Toggle))]
    public class PanelToggle : MonoBehaviour
    {
        public TogglablePanel targetPanel;

        void Start()
        {
            Toggle t = GetComponent<Toggle>();

            targetPanel.Setup(t.isOn);

            t.onValueChanged.AddListener(OnValueChanged);
        }


        void OnValueChanged(bool val)
        {
            targetPanel.Toggle(val);
        }
    }
}
