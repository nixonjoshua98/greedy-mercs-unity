using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formulas
{
    public static double CalcEnemyHealth(int stage)
    {
        return stage;
    }

    public static double CalcBossHealth(int stage)
    {
        return CalcEnemyHealth(stage) * 3;
    }
}
