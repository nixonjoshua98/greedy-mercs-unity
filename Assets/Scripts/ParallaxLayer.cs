using System.Linq;
using UnityEngine;

namespace SRC.Parallax
{
    public class ParallaxLayer : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private float SpeedMultiplier = 0;
        private Camera FollowCamera;
        private SpriteRenderer[] Renderers;
        private float prevCamXPos;

        private void Awake()
        {
            FollowCamera = Camera.main;

            Renderers = GetComponentsInChildren<SpriteRenderer>();

            prevCamXPos = FollowCamera.transform.position.x;
        }

        private void Start()
        {
            AlignRenders();
        }

        private void AlignRenders()
        {
            for (int i = 1; i < Renderers.Length; i++)
            {
                SpriteRenderer prev = Renderers[i - 1];
                SpriteRenderer current = Renderers[i];

                current.transform.position = new Vector3(prev.bounds.max.x, prev.transform.position.y) + new Vector3(current.bounds.size.x / 2, 0);
            }
        }

        private void LateUpdate()
        {
            float camXPos = FollowCamera.transform.position.x;

            Bounds camBounds = FollowCamera.Bounds();

            float xMove = (camXPos - prevCamXPos) * SpeedMultiplier;

            transform.AddPositionVector(new Vector3(xMove, 0));

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

            prevCamXPos = camXPos;
        }
    }
}
