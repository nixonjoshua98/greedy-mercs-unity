using System.Collections;

using UnityEngine;

namespace GM.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class PanelPopupAnimation : MonoBehaviour
    {
        private void Awake()
        {
            Play();
        }

        void Play()
        {
            StartCoroutine(Animation());
        }

        private IEnumerator Animation()
        {
            RectTransform rt = GetComponent<RectTransform>();

            yield return Enumerators.LerpFromTo(0.25f, 1, 0.1f, progress =>
            {                 
                rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);             
            });

            Destroy(this);
        }
    }
}