using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public void OnDeath()
    {
        if (TryGetComponent(out EnemyLoot loot))
        {
            loot.Process();
        }

        Destroy(gameObject);
    }
}