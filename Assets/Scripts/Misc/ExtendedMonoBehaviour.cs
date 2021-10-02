
using UnityEngine;

namespace GM
{
    public abstract class ExtendedMonoBehaviour : Core.GMMonoBehaviour
    {
        [Range(0.0f, 5.0f)] public float periodicUpdateDelay = 0.1f;

        void OnEnable()
        {
            InvokeRepeating("OnPeriodicUpdate", periodicUpdateDelay, periodicUpdateDelay);
        }

        void OnDisable()
        {
            CancelInvoke("OnPeriodicUpdate");
        }

        void OnPeriodicUpdate()
        {
            PeriodicUpdate();
        }

        protected virtual void PeriodicUpdate()
        {

        }
    }
}
