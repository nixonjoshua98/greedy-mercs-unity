using System.Collections;
using System.Collections.Generic;

using UnityEngine;

class StageData
{
    const int ENEMIES_PER_STAGE = 3;

    int _currentStage;
    int _currentEnemy;

    bool _isStageCompleted;

    public int CurrentStage { get { return _currentStage; } }
    public int CurrentEnemy { get { return _currentEnemy; } }

    public bool IsStageCompleted { get { return _isStageCompleted; } }

    public StageData()
    {
        _currentStage = _currentEnemy = 1;

        _isStageCompleted = false;
    }

    public void AddKill()
    {
        if (_currentEnemy + 1 > ENEMIES_PER_STAGE)
        {
            _isStageCompleted = true;
        }
        else
        {
            _currentEnemy++;
        }
    }

    public void AdvanceStage()
    {
        _currentEnemy = 1;

        _currentStage++;

        _isStageCompleted = false;
    }
}
