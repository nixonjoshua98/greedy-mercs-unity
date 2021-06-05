
using SimpleJSON;

namespace GM
{

    public class StageState
    {
        public const int MIN_PRESTIGE_STAGE = 80;

        const int ENEMIES_PER_STAGE = 6;

        public int currentStage;
        public int currentEnemy;

        public bool isStageCompleted;

        public StageState(JSONNode node)
        {
            Reset();
        }

        public void Reset()
        {
            currentStage = currentEnemy = 1;

            isStageCompleted = false;
        }

        public void AddKill()
        {
            if (currentEnemy + 1 > ENEMIES_PER_STAGE)
            {
                isStageCompleted = true;
            }
            else
            {
                currentEnemy++;
            }
        }

        public void AdvanceStage()
        {
            currentEnemy = 1;

            currentStage++;

            isStageCompleted = false;
        }
    }
}