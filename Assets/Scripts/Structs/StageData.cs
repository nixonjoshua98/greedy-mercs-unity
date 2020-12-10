using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GameDataStructs
{
    public class PlayerData
    {
        public static float Gold = 0.0f;
    }

    class EnemyData
    {
        public EnemyHealth Health;

        public EnemyController Controller;

        public bool IsEnemyAvailable { get { return Controller != null; } }

        public EnemyData()
        {
            Health = new EnemyHealth();
        }
    }

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
}