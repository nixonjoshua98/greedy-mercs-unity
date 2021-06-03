using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class EnemyHealth : Health
    {
        public override BigDouble GetIntialHealth()
        {
            return Formulas.StageEnemy.CalcEnemyHealth(GameState.Stage.stage);
        }
    }
}