using UnityEngine;
using System.Collections.Generic;


namespace GM.UI
{
    enum PanelToggleType
    {
        ACTIVE = 0,
        CANVAS = 1
    }


    public class CloseablePanel : ExtendedMonoBehaviour
    {
        [Header("Closeable Panel")]
        [SerializeField] PanelToggleType toggleType = PanelToggleType.ACTIVE;

        [ConditionalAttribute("toggleType", PanelToggleType.CANVAS)]
        [SerializeField] Canvas canvasToToggle;

        [SerializeField] List<CloseablePanel> childPanels;

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
            OnShown();

            foreach (CloseablePanel panel in childPanels) panel.ShowPanel();
        }


        void HidePanel()
        {
            OnHidden();

            foreach (CloseablePanel panel in childPanels) panel.HidePanel();
        }



        // Overrideable
        protected virtual void OnShown() { }
        protected virtual void OnHidden() { }

    }
}