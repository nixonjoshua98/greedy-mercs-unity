using UnityEngine;


namespace GM.UI
{
    enum PanelToggleType
    {
        ACTIVE = 0,
        CANVAS = 1
    }


    public class CloseablePanel : ExtendedMonoBehaviour
    {
        [Header("Closeable Panel")]
        [SerializeField] PanelToggleType toggleType = PanelToggleType.ACTIVE;

        [ConditionalAttribute("toggleType", PanelToggleType.CANVAS)]
        public Canvas canvasToToggle;

        public void Toggle(bool val)
        {
            gameObject.SetActive(true);

            if (toggleType == PanelToggleType.ACTIVE)
                gameObject.SetActive(val);

            else if (toggleType == PanelToggleType.CANVAS)
                canvasToToggle.enabled = val;

            if (val)
                OnShown();

            else
                OnHidden();
        }


        protected virtual void OnShown()
        {

        }


        protected virtual void OnHidden()
        {

        }

    }
}