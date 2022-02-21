using GM.LocalSave;
using Newtonsoft.Json;

namespace GM.States
{
    public class GameState
    {
        // Constants
        public readonly int EnemiesPerStage = 3;

        public int Stage = 1;
        public int EnemiesDefeated = 0;

        public int EnemiesRemaining => EnemiesPerStage - EnemiesDefeated;
        public bool HasBossSpawned { get; set; } = false;

        [JsonIgnore]
        public bool PreviouslyPrestiged;

        public static GameState Deserialize(LocalSaveFileModel model)
        {
            return model.GameState;
        }

        public void Serialize(ref LocalSaveFileModel model)
        {
            model.GameState = this;
        }
    }
}