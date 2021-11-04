using System.Collections.Generic;
using UnityEngine;

namespace GM.UI.Panels
{
    class PanelController : TogglablePanel
    {
        [SerializeField] List<Panel> childPanels;

        protected virtual void Awake()
        {
            UpdateLoop(); // VS stops crying about no references

            InvokeRepeating("UpdateLoop", 0.0f, 0.5f);
        }

        private void OnDestroy()
        {
            CancelInvoke();
        }

        public override void OnHidden()
        {
            childPanels.ForEach(x => x.OnHidden());
            UpdateLoop();
        }

        public override void OnShown()
        {
            childPanels.ForEach(x => x.OnShown());
            UpdateLoop();
        }

        private void UpdateLoop()
        {
            if (IsShown)
            {
                WhileShownUpdate();

                childPanels.ForEach(x => x.WhileShownUpdate());
            }
        }
    }
}
