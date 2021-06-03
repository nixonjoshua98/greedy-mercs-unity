﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class BossHealth : Health
    {
        public override BigDouble GetIntialHealth()
        {
            return Formulas.StageEnemy.CalcBossHealth(GameState.Stage.stage);
        }
    }
}