using UnityEngine;

namespace SRC
{
    public static class BoundsExtensions
    {
        public static Vector3 RandomPosition(this Bounds source)
        {
            float width = source.size.x * 0.25f;
            float height = source.size.y * 0.1f;

            float xPos = Random.Range(-width, width);
            float yPos = Random.Range(-height, height);

            return source.center + new Vector3(xPos, yPos);
        }
    }
}
