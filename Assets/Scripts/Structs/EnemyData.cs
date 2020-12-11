using System.Collections;
using System.Collections.Generic;

using UnityEngine;

class EnemyData
{
    public EnemyHealth Health;

    public EnemyController Controller;

    public bool IsAvailable { get { return Controller != null && !Health.IsDead; } }

    public EnemyData()
    {
        Health = new EnemyHealth();
    }
}