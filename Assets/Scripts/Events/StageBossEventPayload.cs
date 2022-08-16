using UnityEngine;

namespace SRC.Events
{
    public class StageBossEventPayload
    {
        public GameObject GameObject;
        public int StageSpawned;
        public bool IsBounty;

        public StageBossEventPayload(GameObject go, int stage, bool isBounty)
        {
            GameObject = go;
            StageSpawned = stage;
            IsBounty = isBounty;
        }
    }
}
