using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Character : MonoBehaviour
{
    SpriteRenderer[] renderers;

    IEnumerator Start()
    {
        renderers = transform.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in renderers)
        {
            sr.sortingOrder = 100 - (int)transform.position.y;

            yield return new WaitForEndOfFrame();
        }
    }

    public void Flip()
    {
        Vector3 scale = transform.localScale;

        scale.x *= -1.0f;

        transform.localScale = scale;
    }
}
