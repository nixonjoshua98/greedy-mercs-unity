using UnityEngine;
using System.Collections.Generic;


namespace GM.UI
{
    enum PanelToggleType
    {
        ACTIVE = 0,
        CANVAS = 1
    }


    public class PanelController : ExtendedMonoBehaviour
    {
        [Header("Closeable Panel")]
        [SerializeField] PanelToggleType toggleType = PanelToggleType.ACTIVE;

        [ConditionalAttribute("toggleType", PanelToggleType.CANVAS)]
        [SerializeField] Canvas canvasToToggle;

        [SerializeField] List<PanelController> childPanels;

        bool IsShown;
        bool IsPrevShown;

        public void Toggle(bool val)
        {
            gameObject.SetActive(true);

            if (toggleType == PanelToggleType.ACTIVE)
                gameObject.SetActive(val);

            else if (toggleType == PanelToggleType.CANVAS)
                canvasToToggle.enabled = val;

            if (val)
                ShowPanel();

            else 
                HidePanel();
        }


        void ShowPanel()
        {
            IsShown = true;

            OnShown();

            foreach (PanelController panel in childPanels)
            {
                // Re-show the panel if it was previously shown
                if (panel.IsPrevShown)
                {
                    panel.ShowPanel();
                }
            }
        }


        void HidePanel()
        {
            IsShown = false;

            OnHidden();

            foreach (PanelController panel in childPanels)
            {
                panel.IsPrevShown = false;

                // Hide the now-showing panel
                if (panel.IsShown)
                {
                    panel.HidePanel();

                    // Remember we closed a child so we can re-show it next time
                    panel.IsPrevShown = true;
                }
            }
        }



        // Overrideable
        protected virtual void OnShown() { }
        protected virtual void OnHidden() { }

    }
}