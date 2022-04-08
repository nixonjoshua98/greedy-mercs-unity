using System.Linq;

using UnityEngine;

namespace GM.Background
{
    public class ParallaxLayer : MonoBehaviour
    {
        private float length;
        private SpriteRenderer[] renderers;

        private void Start()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();

            length = renderers[0].bounds.size.x;
        }

        public void Update()
        {
            float xMinCam = Camera.main.MinBounds().x;
            float xMaxCam = Camera.main.MaxBounds().x;

            foreach (SpriteRenderer renderer in renderers)
            {
                // Renderer is out of bounds more than 1 renderer length from the left of the camera
                if (renderer.bounds.max.x <= (xMinCam - length))
                {
                    // Move the renderer to the right of all renderers
                    renderer.transform.position = new Vector3(RightRenderer.transform.position.x + length, renderer.transform.position.y);
                }

                // Renderer is out of bounds from the right of the camera
                else if (renderer.bounds.min.x > (xMaxCam + length))
                {
                    // Move the renderer to the left of the camera
                    renderer.transform.position = new Vector3(LeftRenderer.transform.position.x - length, renderer.transform.position.y);
                }
            }
        }

        private SpriteRenderer LeftRenderer => renderers.OrderBy(r => r.transform.position.x).First();

        private SpriteRenderer RightRenderer => renderers.OrderBy(r => r.transform.position.x).Last();
    }
}