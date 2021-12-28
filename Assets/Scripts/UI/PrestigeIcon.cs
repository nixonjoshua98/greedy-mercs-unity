using UnityEngine;

namespace GM
{
    public class PrestigeIcon : Core.GMMonoBehaviour
    {
        [SerializeField] GameObject PanelObject;

        public void OnClick()
        {
            if (App.Data.GameState.Stage >= Common.Constants.MIN_PRESTIGE_STAGE)
                InstantiateUI<RectTransform>(PanelObject);
        }
    }
}