using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public abstract class UnitController : MonoBehaviour
    {
        /*
         * UnitController is the base class for Mercs, Bosses, and Enemies.
         * Methods should have a generic cross-class purpose
         */


        protected void FadeOut(float duration, UnityAction action)
        {
            StartCoroutine(Fade(duration, action, fadeIn: false));
        }


        protected void FadeIn(float duration, UnityAction action)
        {
            StartCoroutine(Fade(duration, action, fadeIn: true));
        }


        IEnumerator Fade(float duration, UnityAction action, bool fadeIn)
        {
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

            Color[] colors = renderers.Select(sr => sr.color).ToArray();

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                for (int i = 0; i < renderers.Length; ++i)
                {
                    SpriteRenderer sr = renderers[i];

                    sr.color = new Color(colors[i].r, colors[i].g, colors[i].b, colors[i].a * (fadeIn ? progress : 1 - progress));
                }

                progress += Time.fixedDeltaTime / duration;

                yield return new WaitForFixedUpdate();
            }

            action.Invoke();
        }
    }
}
