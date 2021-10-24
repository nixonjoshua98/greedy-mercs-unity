using UnityEngine;

namespace GM.UI.Panels
{
    enum PanelToggleType
    {
        ACTIVE = 0,
        CANVAS = 1,
        NOTHING = 2
    }

    public abstract class Panel : Core.GMMonoBehaviour
    {
        [Header("Closeable Panel")]
        [SerializeField] PanelToggleType toggleType = PanelToggleType.ACTIVE;

        [Conditional("toggleType", PanelToggleType.CANVAS)]
        [SerializeField] Canvas canvasToToggle;

        bool isShowing;

        public void Toggle(bool val)
        {
            if (!(isShowing && val))
            {
                ToggleObject(val);

                if (val)
                {
                    Dispatch_ShowPanel();
                    OnShown();
                }

                else
                {
                    Dispatch_HidePanel();
                    OnHidden();
                }
            }
        }

        void ToggleObject(bool val)
        {
            gameObject.SetActive(true);

            if (toggleType == PanelToggleType.ACTIVE)
                gameObject.SetActive(val);

            else if (toggleType == PanelToggleType.CANVAS)
                canvasToToggle.enabled = val;
        }

        protected virtual void Dispatch_ShowPanel()
        {
            Debug.Log($"Shown {name}");
            isShowing = true;
        }


        protected virtual void Dispatch_HidePanel()
        {
            Debug.Log($"Hidden {name}");
            isShowing = false;
        }

        protected abstract void OnHidden();
        protected abstract void OnShown();
    }
}