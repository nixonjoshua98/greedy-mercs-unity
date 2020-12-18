using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLoot : EnemyLoot
{
    public override void Process()
    {
        GameState.player.gold += Formulas.CalcBossGold(stageSpawned);
    }
}
