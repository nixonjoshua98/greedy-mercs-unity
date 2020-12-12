using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    public override void SetHealth()
    {
        maxHealth = currentHealth = 10.0f + GameManager.CurrentStage;
    }
}
