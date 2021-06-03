using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class ParticleSystemCallback : MonoBehaviour
    {
        [SerializeField] public UnityEvent OnParticleSystemStoppedCallback;

        public void OnParticleSystemStopped()
        {
            OnParticleSystemStoppedCallback.Invoke();
        }
    }
}