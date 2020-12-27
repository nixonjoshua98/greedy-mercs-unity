using System.Collections;

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
            progress -= (Time.fixedDeltaTime * 2);

            img.fillAmount = progress;

            yield return new WaitForFixedUpdate();
        }

        img.raycastTarget = false;
    }
}
