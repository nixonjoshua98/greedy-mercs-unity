using System.Collections;
using System.Collections.Generic;

using UnityEngine;

class StageData
{
    int currentStage;
    int currentEnemy;

    public int CurrentStage { get { return currentStage; } }
    public int CurrentEnemy { get { return currentEnemy; } }

    public StageData()
    {
        currentStage = currentEnemy = 1;
    }

    public void AddKill()
    {
        currentEnemy++;

        if (currentEnemy > 5)
        {
            currentEnemy = 1;

            currentStage++;
        }
    }
}
