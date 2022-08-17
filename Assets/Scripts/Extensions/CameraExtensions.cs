using UnityEngine;

namespace SRC
{
    public static class CameraExtensions
    {
        public static bool IsVisible(this Camera cam, Vector2 pos)
        {
            return pos.x >= cam.MinBounds().x && cam.MaxBounds().x >= pos.x;
        }

        public static Bounds Bounds(this Camera camera)
        {
            float screenAspect = Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;

            return new Bounds(camera.transform.position, new(cameraHeight * screenAspect, cameraHeight));
        }

        private static Vector2 MinBounds(this Camera camera)
        {
            Vector2 v2 = camera.Extents();

            return camera.transform.position - new Vector3(v2.x, v2.y);
        }

        private static Vector2 MaxBounds(this Camera camera)
        {
            Vector2 v2 = camera.Extents();

            return camera.transform.position + new Vector3(v2.x, v2.y);
        }

        private static Vector2 Extents(this Camera camera)
        {
            return new Vector2(camera.orthographicSize * Screen.width / Screen.height, camera.orthographicSize);
        }
    }
}
