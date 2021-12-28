using UnityEngine;

namespace GM.UI.Panels
{
    public abstract class Panel : Core.GMMonoBehaviour
    {
        public bool IsShown { get; protected set; }

        public virtual void OnShown()
        {

        }

        public virtual void OnHidden()
        {

        }
    }
}