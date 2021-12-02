using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class _Target
    {
        public GameObject GameObject;

        public _Target(GameObject obj)
        {
            this.GameObject = obj;
        }

        public HealthController Health => this.GameObject.GetComponent<HealthController>();
    }
}
