using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Targets
{
    public interface ITargetManager
    {
        bool TryGetMercTarget(ref Target target);
    }
}