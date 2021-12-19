using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;
using System;
using GM.Units;
using GM.Targets;


namespace GM.Mercs.Controllers
{
    public interface IMercController
    {
        void Pause();
        void Resume();

        void MoveTowards(Vector3 position, Action action);
        void SetPriorityTarget(Target target);
    }
}