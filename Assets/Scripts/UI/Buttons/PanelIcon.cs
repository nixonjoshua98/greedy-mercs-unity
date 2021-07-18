
using UnityEngine;

namespace GM
{
    public class PanelIcon : MonoBehaviour
    {
        [SerializeField] GameObject PanelObject;

        public virtual void OnClick()
        {
            CanvasUtils.Instantiate(PanelObject);
        }
    }
}
