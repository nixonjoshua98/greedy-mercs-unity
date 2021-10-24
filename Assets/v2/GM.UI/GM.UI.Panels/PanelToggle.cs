using UnityEngine;
using UnityEngine.UI;

namespace GM.UI.Panels
{
    [RequireComponent(typeof(Toggle))]
    public class PanelToggle: MonoBehaviour
    {
        public Panel targetPanel;

        void Awake()
        {
            Toggle t = GetComponent<Toggle>();

            if (t.isOn)
            {
                targetPanel.Toggle(true);
            }

            t.onValueChanged.AddListener(OnValueChanged);
        }


        void OnValueChanged(bool val)
        {
            targetPanel.Toggle(val);
        }
    }
}
