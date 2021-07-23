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
            // Slowly fade out (with a callback) all the attached sprite renderers

            IEnumerator _FadeOut(float duration, UnityAction action)
            {
                SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
                Color[] colors = renderers.Select(sr => sr.color).ToArray();

                float progress = 0.0f;

                while (progress < 1.0f)
                {
                    yield return new WaitForFixedUpdate();

                    for (int i = 0; i < renderers.Length; ++i)
                    {
                        SpriteRenderer sr = renderers[i];

                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, colors[i].a * (1 - progress));
                    }

                    progress += Time.fixedDeltaTime / duration;
                }

                action.Invoke();
            }

            StartCoroutine(_FadeOut(duration, action));
        }
    }
}
