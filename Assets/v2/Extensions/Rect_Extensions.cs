using UnityEngine;

namespace GM
{
    public static class Rect_Extensions
    {
        public static void DrawGizmos(this Rect source, Vector3 pos, Vector2 scale)
        {
            float height = source.height * scale.y;
            float width = source.width * scale.x;

            Vector3 center = pos + (source.position * scale).ToVector3();

            Vector3 topLeft = new Vector3(center.x - (width / 2), center.y - (height / 2));
            Vector3 btmLeft = new Vector3(topLeft.x, center.y + (height / 2));

            Vector3 topRight = new Vector3(center.x + (width / 2), center.y - (height / 2));
            Vector3 btmRight = new Vector3(topRight.x, btmLeft.y);

            Gizmos.DrawLine(topLeft, btmLeft);
            Gizmos.DrawLine(topRight, btmRight);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(btmLeft, btmRight);
        }
    }
}
