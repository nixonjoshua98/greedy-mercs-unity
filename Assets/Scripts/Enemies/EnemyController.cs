using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void OnHurt()
    {
        anim.Play("Hurt");
    }

    public void OnDeath()
    {
        anim.Play("Dying");

        PushEnemyToBack();
    }

    public void OnDeathFinished()
    {
        if (TryGetComponent(out EnemyLoot loot))
        {
            loot.Process();
        }

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