using UnityEngine;

namespace GM
{
    public static class BoundsExtensions
    {
        public static Vector3 RandomCenterPosition(this Bounds source)
        {
            float width = source.size.x / 4;
            float height = source.size.y / 6;

            float xPos = Random.Range(-width, width);
            float yPos = Random.Range(-height, height);

            return source.center + new Vector3(xPos, yPos);
        }
    }
}
