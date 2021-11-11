using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Events
{
    public struct WaveSpawnEventData
    {
        public BigDouble CombinedHealth { get; set; }
        public List<HealthController> HealthControllers { get; set; }
    }
}
