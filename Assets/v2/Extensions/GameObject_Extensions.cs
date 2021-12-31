using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GM
{
    public static class GameObject_Extensions
    {
        public static bool TryGetComponentInChildren<T>(this GameObject obj,  out T component)
        {
            component = obj.GetComponentInChildren<T>();

            return component != null;
        }

        public static void Fade(this MonoBehaviour mono, float duration, Action action)
        {
            mono.StartCoroutine(FadeEnumerator(mono, duration, action));
        }

        static IEnumerator FadeEnumerator(MonoBehaviour mono, float duration, Action action)
        {
            SpriteRenderer[] renderers = mono.GetComponentsInChildren<SpriteRenderer>();

            Color[] colors = renderers.Select(sr => sr.color).ToArray();

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                for (int i = 0; i < renderers.Length; ++i)
                {
                    SpriteRenderer sr = renderers[i];

                    sr.color = new Color(colors[i].r, colors[i].g, colors[i].b, colors[i].a * (1 - progress));
                }

                progress += Time.fixedDeltaTime / duration;

                yield return new WaitForFixedUpdate();
            }

            action.Invoke();
        }
    }
}
