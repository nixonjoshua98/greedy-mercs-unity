using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.UI.Panels
{
    public class TogglablePanel : Panel
    {
        [Header("Closeable Panel")]
        [SerializeField] PanelToggleType toggleType = PanelToggleType.CANVAS;

        [Conditional("toggleType", PanelToggleType.CANVAS)]
        [SerializeField] Canvas canvasToToggle;

        protected bool IsShown { get; private set; }

        public void Toggle(bool val)
        {
            if (!(IsShown && val))
            {
                ToggleObject(val);

                IsShown = val;

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

            if (toggleType == PanelToggleType.ACTIVE)
                gameObject.SetActive(val);

            else if (toggleType == PanelToggleType.CANVAS)
                canvasToToggle.enabled = val;
        }
    }
}
