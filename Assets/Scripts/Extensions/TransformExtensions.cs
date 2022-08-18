using UnityEngine;

namespace SRC
{
    public static class TransformExtensions
    {
        public static void AddPositionVector(this Transform transform, Vector3 position)
        {
            transform.position = new Vector3(
                transform.position.x + position.x,
                transform.position.y + position.y,
                transform.position.z + position.z);
        }

        public static void ShuffleChildren(this Transform trans)
        {
            var rnd = SRC.Common.Utility.SeededRandom(trans.GetInstanceID());

            for (int i = 0; i < trans.childCount; i++)
            {
                var child = trans.GetChild(i);

                child.SetSiblingIndex(rnd.Next(0, child.childCount));
            }
        }
    }
}
