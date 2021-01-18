
using UnityEngine;

namespace GreedyMercs
{
    [RequireComponent(typeof(RectTransform))]
    public class PanelPopup : MonoBehaviour
    {
        void Awake()
        {
            StartCoroutine(Utils.Lerp.RectTransform(GetComponent<RectTransform>(), Vector3.zero, Vector3.one, 0.2f));
        }
    }
}