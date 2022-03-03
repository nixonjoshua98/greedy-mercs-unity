using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.UI.Common
{
    public class PanelPopupBase : GM.Core.GMMonoBehaviour
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
