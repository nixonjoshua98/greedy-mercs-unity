
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    public abstract class ExtendedMonoBehaviour : MonoBehaviour
    {
        [Header("Extended MonoBehaviour")]
        [Range(0.0f, 1.0f)] public float periodicUpdateDelay = 0.1f;

        void Start()
        {
            PeriodicUpdate();
        }

        void OnEnable()
        {           
            InvokeRepeating("OnPeriodicUpdate", 0.0f, periodicUpdateDelay);
        }

        void OnDisable()
        {
            CancelInvoke("OnPeriodicUpdate");
        }

        void OnPeriodicUpdate()
        {
            PeriodicUpdate();
        }

        protected abstract void PeriodicUpdate();
    }
}
