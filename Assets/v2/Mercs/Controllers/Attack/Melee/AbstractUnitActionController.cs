using GM.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public abstract class AbstractUnitActionController : MonoBehaviour, IUnitActionController
    {       
        // Private
        bool _HasControl;

        // Public accessors
        public bool HasControl { get => _HasControl; protected set => _HasControl = value; }

        public abstract bool WantsControl();
        public abstract void GiveControl();
        public abstract void RemoveControl();
    }
}
