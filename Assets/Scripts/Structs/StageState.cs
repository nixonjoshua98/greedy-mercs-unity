﻿
[System.Serializable]
public class StageState
{
    public const int MIN_PRESTIGE_STAGE = 80;

    const int ENEMIES_PER_STAGE = 6;

    public int stage;
    public int enemy;

    public bool isStageCompleted;

    public StageState()
    {
        Reset();
    }

    public void Reset()
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