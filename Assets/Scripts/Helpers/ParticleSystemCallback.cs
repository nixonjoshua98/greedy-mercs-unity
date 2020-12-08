using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class ParticleSystemCallback : MonoBehaviour
{
    [SerializeField] public UnityEvent OnParticleSystemStoppedCallback;

    public void OnParticleSystemStopped()
    {
        OnParticleSystemStoppedCallback.Invoke();
    }
}
