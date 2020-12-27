using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] Image img;

    IEnumerator Start()
    {
        float progress = 1.0f;

        img.raycastTarget = true;

        while (progress > 0.0f)
        {
            img.fillAmount = progress;

            progress -= Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        img.raycastTarget = false;
    }
}
