using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    public override double GetIntialHealth()
    {
        return base.GetIntialHealth() * 3.0f;
    }
}
