using System.Linq;
using UnityEngine;

namespace GM
{
    public class ParallaxBackground : MonoBehaviour
    {
        Camera FollowCamera;
        SpriteRenderer[] Renderers;

        void Awake()
        {
            FollowCamera = Camera.main;

            Renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        void Start()
        {
            AlignRenders();
        }

        void AlignRenders()
        {
            for (int i = 1; i < Renderers.Length; i++)
            {
                SpriteRenderer prev = Renderers[i - 1];
                SpriteRenderer current = Renderers[i];

                current.transform.position = new Vector3(prev.bounds.max.x, prev.transform.position.y) + new Vector3(current.bounds.size.x / 2, 0);
            }
        }

        void LateUpdate()
        {
            /*
                BUG:
                    Renderers will keep flickering between left and right side if they are far enough from the camera
             */

            Bounds camBounds = FollowCamera.Bounds();

            foreach (SpriteRenderer renderer in Renderers)
            {
                float width = renderer.bounds.size.x;

                bool isOutOfBoundsLeft = renderer.bounds.max.x <= (camBounds.min.x - width);
                bool isOutOfBoundsRight = renderer.bounds.min.x > (camBounds.max.x + width);

                // Left -> Right
                if (isOutOfBoundsLeft)
                {
                    Vector3 rightMostRenderer = Renderers.OrderBy(x => x.transform.position.x).Last().transform.position;

                    renderer.transform.position = new Vector3(rightMostRenderer.x + width, renderer.transform.position.y);
                }

                // Right -> Left
                else if (isOutOfBoundsRight)
                {
                    Vector3 leftMostRenderer = Renderers.OrderBy(x => x.transform.position.x).First().transform.position;

                    renderer.transform.position = new Vector3(leftMostRenderer.x - width, renderer.transform.position.y);
                }
            }
        }
    }
}
