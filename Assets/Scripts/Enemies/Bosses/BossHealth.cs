using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class BossHealth : AbstractHealthController
    {
        public override BigDouble GetIntialHealth()
        {
            CurrentStageState state = GameManager.Instance.GetState();

            return Formulas.BossHealth(state.currentStage);
        }
    }
}