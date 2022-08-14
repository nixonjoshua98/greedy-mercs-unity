using UnityEngine;

namespace GM
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
    }
}
