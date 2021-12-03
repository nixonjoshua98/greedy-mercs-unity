using UnityEngine;

namespace GM.UI.Panels
{
    public class TogglablePanel : Panel
    {
        [SerializeField] Canvas canvasToToggle;

        public void Toggle(bool val)
        {
            if (!(PanelIsShown && val))
            {
                ToggleObject(val);

                if (val)
                {
                    OnShown();
                }

                else
                {
                    OnHidden();
                }
            }
        }

        void ToggleObject(bool val)
        {
            gameObject.SetActive(true);

            canvasToToggle.enabled = val;
        }
    }
}
