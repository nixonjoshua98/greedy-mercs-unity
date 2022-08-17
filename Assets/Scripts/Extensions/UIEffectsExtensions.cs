using Coffee.UIEffects;
using System.Collections;
using UnityEngine;

namespace SRC
{
    public static class UIEffectsExtensions
    {
        public static void PlayReverse(this UIDissolve effect)
        {
            effect.StartCoroutine(PlayReverseEnumerator(effect));
        }

        private static IEnumerator PlayReverseEnumerator(this UIDissolve effect)
        {
            float duration = effect.effectPlayer.duration;

            float progress = 0;

            effect.effectFactor = 1;

            while (progress <= duration)
            {
                float ts = Time.deltaTime;

                progress += ts;

                effect.effectFactor = progress / duration;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
