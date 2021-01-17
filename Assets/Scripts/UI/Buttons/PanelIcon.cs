
using UnityEngine;

namespace UI.Icons
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
