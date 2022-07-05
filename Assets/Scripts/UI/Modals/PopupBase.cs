using UnityEngine;

namespace GM.UI
{
    public abstract class PopupBase : GM.Core.GMMonoBehaviour
    {
        [SerializeField] GameObject InnerPanel;
        protected void ShowInnerPanel()
        {
            InnerPanel.SetActive(true);
        }
    }
}