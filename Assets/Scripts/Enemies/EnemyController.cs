using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void StartHurtSequence()
    {
        anim.Play("Hurt");
    }

    public void StartDeathSequence()
    {
        anim.Play("Dying");

        PushEnemyToBack();
    }

    public void OnDeathFinished()
    {
        Destroy(gameObject);
    }

    void PushEnemyToBack()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer r in renderers)
        {
            r.sortingOrder -= 500;
        }
    }
}