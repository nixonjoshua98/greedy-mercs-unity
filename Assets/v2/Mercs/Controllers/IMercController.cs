using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;
using System;
using GM.Units;

namespace GM.Mercs.Controllers
{
    public interface IMercController
    {
        void Move(Vector3 position, Action action);
    }
}
