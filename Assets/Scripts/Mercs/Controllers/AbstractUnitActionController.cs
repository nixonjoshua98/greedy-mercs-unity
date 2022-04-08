using UnityEngine;

namespace GM.Mercs.Controllers
{
    public abstract class AbstractUnitActionController : MonoBehaviour, IUnitActionController
    {
        [SerializeField] private int _Priority;
        private bool _HasControl;

        // Public accessors
        public int Priority { get => _Priority; protected set => _Priority = value; }
        public bool HasControl { get => _HasControl; protected set => _HasControl = value; }

        public abstract bool WantsControl();
        public abstract void GiveControl();
        public abstract void RemoveControl();
    }
}