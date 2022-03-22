using UnityEngine;

namespace GM
{
    public static class TransformExtensions
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
