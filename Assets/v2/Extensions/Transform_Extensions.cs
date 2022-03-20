using UnityEngine;

namespace GM
{
    public static class Transform_Extensions
    {
        public static void DestroyChildren(this Transform trans)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                UnityEngine.Object.Destroy(trans.GetChild(i));
            }
        }
    }
}
