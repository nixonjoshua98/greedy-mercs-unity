using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected static GMApplication App => GMApplication.Instance;
    }
}
