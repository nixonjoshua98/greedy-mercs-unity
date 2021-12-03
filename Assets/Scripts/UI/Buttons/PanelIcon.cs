
using UnityEngine;

namespace GM
{
    public class PanelIcon : Core.GMMonoBehaviour
    {
        [SerializeField] GameObject PanelObject;

        public virtual void OnClick()
        {
            InstantiateUI<RectTransform>(PanelObject);
        }
    }
}
