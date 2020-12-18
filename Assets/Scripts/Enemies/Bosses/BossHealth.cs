using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BossHealth : Health
{
    public override double GetIntialHealth()
    {
        return Formulas.CalcBossHealth(GameState.stage.stage);
    }
}
