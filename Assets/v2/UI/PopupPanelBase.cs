using System.Collections;
using UnityEngine;

namespace GM.UI
{
    public abstract class PopupPanelBase : GM.Core.GMMonoBehaviour
    {
        [Header("(PanelPopupBase) References")]
        [SerializeField] protected GameObject InnerPanel;

        protected void ShowInnerPanel()
        {
            StartCoroutine(IShowInnerPanel());
        }

        private IEnumerator IShowInnerPanel()
        {
            yield return new WaitForSecondsRealtime(0.1f);

            InnerPanel.SetActive(true);
        }
    }
}
