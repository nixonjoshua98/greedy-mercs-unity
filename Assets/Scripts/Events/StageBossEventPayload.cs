using UnityEngine;

namespace GM.Events
{
    public class StageBossEventPayload
    {
        public GameObject GameObject;
        public int StageSpawned;

        public StageBossEventPayload(GameObject go, int stage)
        {
            GameObject = go;
            StageSpawned = stage;
        }
    }
}
