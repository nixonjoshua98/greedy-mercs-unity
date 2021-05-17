using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    [RequireComponent(typeof(RectTransform))]
    public class PanelPopup : MonoBehaviour
    {
        void OnEnable()
        {
            StartCoroutine(Animation());
        }

        IEnumerator Animation()
        {
            RectTransform rt = GetComponent<RectTransform>();

            yield return Utils.Lerp.RectTransform(rt, Vector3.zero, Vector3.one, 0.075f);

            yield return Utils.Lerp.RectTransform(rt, Vector3.one, Vector3.one * 1.1f, 0.075f);

            yield return Utils.Lerp.RectTransform(rt, Vector3.one * 1.1f, Vector3.one, 0.075f);
        }
    }
}