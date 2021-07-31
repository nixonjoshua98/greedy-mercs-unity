using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Events
{
    public struct WaveSpawnEventData
    {
        public BigDouble CombinedHealth;

        public List<GameObject> Objects;
        public List<HealthController> HealthControllers;
    }
}
