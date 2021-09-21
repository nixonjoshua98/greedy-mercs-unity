using UnityEngine;

namespace GM.Core
{
    public abstract class GMMonoBehaviour : MonoBehaviour
    {
        protected GMApplication App => GMApplication.Instance;
    }
}
