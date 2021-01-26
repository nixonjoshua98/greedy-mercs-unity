using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class EnemyHealth : Health
    {
        public override BigDouble GetIntialHealth()
        {
            return Formulas.StageEnemy.CalcEnemyHealth(GameState.Stage.stage);
        }
    }
}