using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class NormalEnemyHealth : EnemyHealth
{
    public override double GetIntialHealth()
    {
        return Formulas.CalcEnemyHealth(GameState.stage.stage);
    }
}
