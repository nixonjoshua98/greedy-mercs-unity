using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    IEnumerator Start()
    {
        foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sortingOrder = 100 - (int)transform.position.y;

            yield return new WaitForFixedUpdate();
        }
    }
}
