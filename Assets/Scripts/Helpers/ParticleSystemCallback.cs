using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GreedyMercs
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