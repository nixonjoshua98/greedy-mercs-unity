using UnityEngine;
using NaughtyAttributes;

namespace GM.UI.Panels
{
    public enum PanelToggleMode
    {
        Canvas,
        GameObject
    }

    public class TogglablePanel : Panel
    {
        [Header("Toggleable Objects")]
        [SerializeField] PanelToggleMode ToggleMode = PanelToggleMode.Canvas;

        [ShowIf("ToggleMode", PanelToggleMode.Canvas)]
        [SerializeField] Canvas canvasToToggle;

        [ShowIf("ToggleMode", PanelToggleMode.GameObject)]
        [SerializeField] GameObject ObjectToToggle;

        public void Setup(bool val)
        {
            Toggle(val);
        }

        public void Toggle(bool val)
        {
            if (!(IsShown && val))
            {
                ToggleObject(val);

                if (val)
                {
                    OnShown();
                }

                else
                {
                    OnHidden();
                }
            }
        }

        void ToggleObject(bool val)
        {
            gameObject.SetActive(true);

            switch (ToggleMode)
            {
                case PanelToggleMode.Canvas:
                    canvasToToggle.enabled = val;
                    break;
                case PanelToggleMode.GameObject:
                    ObjectToToggle.SetActive(val);
                    break;
            }
        }
    }
}
