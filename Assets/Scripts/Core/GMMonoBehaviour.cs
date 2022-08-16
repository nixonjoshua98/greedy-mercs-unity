using SRC.UI;
using UnityEngine;

namespace SRC.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected GMApplication App => GMApplication.Instance;
        protected UIManager UI => UIManager.Instance;
    }
}
