using System.Collections.Generic;
using UnityEngine;

namespace GM.UI.Panels
{
    class PanelController : TogglablePanel
    {
        [SerializeField] List<Panel> childPanels;

        public override void OnHidden()
        {
            PanelIsShown = false;
            childPanels.ForEach(x => x.OnHidden());
        }

        public override void OnShown()
        {
            PanelIsShown = true;
            childPanels.ForEach(x => x.OnShown());
        }
    }
}
