using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Backgrounds
{
    public class ParallaxLayer : MonoBehaviour
    {
        float length;

        SpriteRenderer[] renderers;

        void Start()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();

            length = renderers[0].bounds.size.x;
        }

        public void Update()
        {
            Vector2 minCameraBounds = Camera.main.MinBounds();

            foreach (SpriteRenderer renderer in renderers)
            {
                if (renderer.bounds.max.x <= minCameraBounds.x)
                {
                    renderer.transform.position = RightMostRenderer().transform.position + new Vector3(length, 0);
                }
            }
        }

        SpriteRenderer RightMostRenderer()
        {
            return renderers.ToList().OrderBy(r => r.transform.position.x).Last();
        }
    }
}