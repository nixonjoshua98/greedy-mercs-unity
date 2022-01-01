using System.Collections.Generic;
using UnityEngine;

namespace GM.UI.Panels
{
    public class PanelController : TogglablePanel
    {
        [SerializeField] List<Panel> childPanels;

        public override void OnHidden()
        {
            IsShown = false;
            childPanels.ForEach(x => x.OnHidden());
        }

        public override void OnShown()
        {
            IsShown = true;
            childPanels.ForEach(x => x.OnShown());
        }
    }
}
