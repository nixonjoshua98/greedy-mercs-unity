
using UnityEngine;

namespace GM
{
    public abstract class ExtendedMonoBehaviour : MonoBehaviour
    {
        [Header("Extended MonoBehaviour")]
        [Range(0.0f, 5.0f)] public float periodicUpdateDelay = 0.1f;

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

        protected virtual void PeriodicUpdate()
        {

        }
    }
}
