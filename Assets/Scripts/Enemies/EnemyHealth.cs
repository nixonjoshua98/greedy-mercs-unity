using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    public override double GetIntialHealth()
    {
        return Formulas.CalcEnemyHealth(GameState.Stage.stage);
    }
}
