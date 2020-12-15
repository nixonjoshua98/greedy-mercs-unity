using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ExpandButton : MonoBehaviour
{
    bool isExpanded, isExpanding;

    [SerializeField] RectTransform rt;

    void Awake()
    {
        isExpanded = isExpanding = false;
    }

    public void OnClick()
    {
        if (!isExpanding)
        {
            StartCoroutine(IChangeSize(isExpanded ? 1500 : 850));
        }
    }

    IEnumerator IChangeSize(float end)
    {
        isExpanding = true;

        float start = rt.offsetMax.y;

        float progress = 0.0f;

        while (progress < 1.0f)
        {
            progress = Mathf.Clamp01(progress + (Time.fixedDeltaTime * 4));

            rt.offsetMax = new Vector2(rt.offsetMax.x, Mathf.Lerp(start, end * -1, progress));

            yield return new WaitForFixedUpdate();
        }

        rt.offsetMax = new Vector2(rt.offsetMax.x, end * -1);

        isExpanding = false;

        isExpanded = !isExpanded;
    }
}
