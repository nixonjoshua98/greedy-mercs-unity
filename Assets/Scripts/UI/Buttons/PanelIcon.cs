
using UnityEngine;

namespace GM
{
    public class PanelIcon : MonoBehaviour
    {
        [SerializeField] GameObject PanelObject;

        public virtual void OnClick()
        {
            Utils.UI.Instantiate(PanelObject, Vector3.zero);
        }
    }
}
