using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    // Animation event
    public void OnDeath()
    {
        if (TryGetComponent(out EnemyLoot loot))
        {
            loot.Process();
        }

        Destroy(gameObject);
    }
}