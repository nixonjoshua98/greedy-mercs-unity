
using UnityEngine;

namespace UI
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
