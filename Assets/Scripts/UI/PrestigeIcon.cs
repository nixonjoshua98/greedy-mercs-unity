using UnityEngine;

namespace GM
{
    public class PrestigeIcon : Core.GMMonoBehaviour
    {
        [SerializeField] GameObject PanelObject;

        public void OnClick()
        {
            CurrentStageState state = GameManager.Instance.State;

            if (state.Stage >= Common.Constants.MIN_PRESTIGE_STAGE)
                InstantiateUI<RectTransform>(PanelObject);
        }
    }
}