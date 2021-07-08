using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class EnemyHealth : AbstractHealthController
    {
        public override BigDouble GetIntialHealth()
        {
            CurrentStageState state = GameManager.Instance.State();

            return Formulas.EnemyHealth(state.currentStage);
        }
    }
}