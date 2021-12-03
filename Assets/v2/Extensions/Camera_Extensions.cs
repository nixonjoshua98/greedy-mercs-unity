using UnityEngine;

namespace GM
{
    public static class Camera_Extensions
    {
        public static Vector2 MinBounds(this Camera camera)
        {
            Vector2 v2 = camera.Extents();

            return camera.transform.position - new Vector3(v2.x, v2.y);
        }

        public static Vector2 MaxBounds(this Camera camera)
        {
            Vector2 v2 = camera.Extents();

            return camera.transform.position + new Vector3(v2.x, v2.y);
        }

        public static Vector2 Extents(this Camera camera)
        {
            return new Vector2(camera.orthographicSize * Screen.width / Screen.height, camera.orthographicSize);
        }
    }
}
