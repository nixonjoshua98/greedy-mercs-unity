using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroFormationStatus
{
    ADDED = 1,
    FAILED_TO_ADD = -1,

    REMOVED = 2,
    FAILED_TO_REMOVE = -2,
}

class HeroFormationSpot
{
    public HeroID HeroID;

    public GameObject HeroObject;

    public Transform Parent;

    public bool IsAvailable { get { return HeroObject == null; } }

    public void Set(HeroID _HeroID, GameObject _HeroObject)
    {
        if (HeroObject != null)
            GameObject.Destroy(HeroObject);

        HeroID = _HeroID;
        HeroObject = _HeroObject;
    }

    public void Free()
    {
        if (HeroObject != null)
            GameObject.Destroy(HeroObject);

        HeroObject = null;
    }
}