using System.Collections;

using UnityEngine;

namespace GM.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class PanelPopupAnimation : MonoBehaviour
    {
        void OnEnable()
        {
            StartCoroutine(Animation());
        }

        IEnumerator Animation()
        {
            RectTransform rt = GetComponent<RectTransform>();

            yield return Lerp(rt, Vector3.zero, Vector3.one, 0.075f);

            yield return Lerp(rt, Vector3.one, Vector3.one * 1.1f, 0.075f);

            yield return Lerp(rt, Vector3.one * 1.1f, Vector3.one, 0.075f);

            Destroy(this);
        }

        public IEnumerator Lerp(RectTransform rt, Vector3 start, Vector3 end, float dur)
        {
            float progress = 0.0f;

            rt.localScale = start;

            while (progress < 1.0f)
            {
                progress += (Time.deltaTime / dur);

                rt.localScale = Vector3.Lerp(start, end, progress);

                yield return new WaitForEndOfFrame();
            }

            rt.localScale = Vector3.one;
        }
    }
}