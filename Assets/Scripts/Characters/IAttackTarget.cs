using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Targetting
{

    public enum TargetPriority
    {
        FIRST   = 0,
        LAST    = 1,
        RANDOM  = 2    
    }

    public interface IAttackTarget
    {
        GameObject GetTarget();
    }
}
