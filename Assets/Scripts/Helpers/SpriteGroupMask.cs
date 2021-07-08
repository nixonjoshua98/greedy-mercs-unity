using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class SpriteGroupMask : MonoBehaviour
    {
        public SpriteMaskInteraction maskInteraction = SpriteMaskInteraction.None;

        void Awake()
        {
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer r in renderers)
            {
                r.maskInteraction = maskInteraction;
            }
        }
    }
}
