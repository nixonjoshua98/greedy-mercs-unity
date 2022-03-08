using System;

namespace GM
{
    public class CurrentPrestigeState : ILocalStateFileSerializer
    {
        public readonly int EnemiesPerStage = 3;

        public int Stage = 1;
        public int EnemiesDefeated = 0;

        public int EnemiesRemaining => Math.Max(0, EnemiesPerStage - EnemiesDefeated);

        public static CurrentPrestigeState Deserialize(LocalStateFile model)
        {
            return model.GameState;
        }

        public void UpdateLocalSaveFile(ref LocalStateFile model)
        {
            model.GameState = this;
        }
    }
}