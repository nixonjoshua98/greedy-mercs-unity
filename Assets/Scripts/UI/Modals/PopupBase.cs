using UnityEngine;
using UnityEngine.Events;

namespace SRC.UI
{
    public abstract class PopupBase : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private GameObject InnerPanel;

        [HideInInspector] public readonly UnityEvent E_OnDestroyed = new();

        private void OnDestroy()
        {
            E_OnDestroyed.Invoke();
        }

        protected void ShowInnerPanel()
        {
            InnerPanel.SetActive(true);
        }
    }
}