using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class StageData
{
    const int ENEMIES_PER_STAGE = 3;

    public int stage;
    public int enemy;

    public bool isStageCompleted;

    public StageData()
    {
        stage = enemy = 1;

        isStageCompleted = false;
    }

    public void AddKill()
    {
        if (enemy + 1 > ENEMIES_PER_STAGE)
        {
            isStageCompleted = true;
        }
        else
        {
            enemy++;
        }
    }

    public void AdvanceStage()
    {
        enemy = 1;

        stage++;

        isStageCompleted = false;
    }
}
