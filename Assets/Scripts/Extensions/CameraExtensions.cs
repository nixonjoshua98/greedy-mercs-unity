using UnityEngine;

namespace GM
{
    public static class CameraExtensions
    {
        public static bool IsVisible(this Camera cam, Vector2 pos)
        {
            return pos.x >= cam.MinBounds().x && cam.MaxBounds().x >= pos.x;
        }

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
