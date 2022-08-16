using System;

namespace SRC
{
    public class GameState
    {
        public readonly int EnemiesPerStage = 3;

        public int Stage = 1;
        public int EnemiesDefeated = 0;

        public int EnemiesRemaining => Math.Max(0, EnemiesPerStage - EnemiesDefeated);

        public void UpdateLocalSaveFile(ref LocalStateFile model)
        {
            model.GameState = this;
        }
    }
}