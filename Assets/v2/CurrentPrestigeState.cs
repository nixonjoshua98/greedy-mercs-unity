﻿using GM.LocalSave;
using System;
using Newtonsoft.Json;

namespace GM
{
    public class CurrentPrestigeState
    {
        // Constants
        public readonly int EnemiesPerStage = 3;

        public int Stage = 1;
        public int EnemiesDefeated = 0;

        public int EnemiesRemaining => Math.Max(0, EnemiesPerStage - EnemiesDefeated);
        public bool HasBossSpawned { get; set; } = false;

        [JsonIgnore]
        public bool PreviouslyPrestiged;

        public static CurrentPrestigeState Deserialize(LocalSaveFileModel model)
        {
            return model.GameState;
        }

        public void Serialize(ref LocalSaveFileModel model)
        {
            model.GameState = this;
        }
    }
}