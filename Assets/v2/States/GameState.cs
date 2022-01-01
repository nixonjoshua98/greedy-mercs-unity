using GM.LocalSave;
using Newtonsoft.Json;

namespace GM.States
{
    public class GameState
    {
        public int Stage = 1;
        public int Wave = 1;

        public bool IsBossSpawned { get; set; } = false;

        [JsonIgnore]
        public bool PreviouslyPrestiged;

        public GameState()
        {

        }

        public void Reset(int stage, int wave)
        {
            Stage = stage;
            Wave = wave;
        }

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