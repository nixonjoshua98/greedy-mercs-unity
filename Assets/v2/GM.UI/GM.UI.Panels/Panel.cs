using UnityEngine;

namespace GM.UI.Panels
{
    enum PanelToggleType
    {
        ACTIVE = 0,
        CANVAS = 1,
    }

    public abstract class Panel : Core.GMMonoBehaviour
    {
        public virtual void OnShown() { }
        public virtual void OnHidden() { }
        public virtual void WhileShownUpdate() { }
    }
}