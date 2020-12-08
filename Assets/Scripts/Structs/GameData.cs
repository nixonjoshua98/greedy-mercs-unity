using System.Collections;
using System.Collections.Generic;

using UnityEngine;

class GameData
{
    int currentStage;
    int currentEnemy;

    public EnemyHealth Health;

    public int CurrentStage { get { return currentStage; } }
    public int CurrentEnemy { get { return currentEnemy; } }

    public GameData()
    {
        currentStage = currentEnemy = 1;

        Health = new EnemyHealth();
    }

    public void AddKills(int kills)
    {
        for (int i = 0; i < kills; ++i)
        {
            currentEnemy++;

            if (currentEnemy > 5)
            {
                currentEnemy = 1;

                currentStage++;
            }
        }

    }
}