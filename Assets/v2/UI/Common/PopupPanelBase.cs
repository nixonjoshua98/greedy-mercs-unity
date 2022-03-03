using System.Collections;
using UnityEngine;

namespace GM.UI.Common
{
    public abstract class PopupPanelBase : GM.Core.GMMonoBehaviour
    {
        [Header("[PanelPopupBase] References")]
        [SerializeField] protected GameObject InnerPanel;

        protected void ShowInnerPanel()
        {
            StartCoroutine(IShowInnerPanel());
        }

        IEnumerator IShowInnerPanel()
        {
            yield return new WaitForEndOfFrame();

            InnerPanel.SetActive(true);
        }
    }
}
