using UnityEngine;

namespace SRC
{
    public static class CameraExtensions
    {
        public static Bounds Bounds(this Camera camera)
        {
            float screenAspect = Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;

            return new Bounds(camera.transform.position, new(cameraHeight * screenAspect, cameraHeight));
        }
    }
}
