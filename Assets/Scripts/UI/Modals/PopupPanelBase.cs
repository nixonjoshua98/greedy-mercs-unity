using UnityEngine;

namespace GM.UI
{
    public abstract class PopupPanelBase : GM.Core.GMMonoBehaviour
    {
        [Header("(PanelPopupBase) References")]
        [SerializeField] GameObject InnerPanel;

        protected void ShowInnerPanel()
        {
            InnerPanel.SetActive(true);
        }
    }
}