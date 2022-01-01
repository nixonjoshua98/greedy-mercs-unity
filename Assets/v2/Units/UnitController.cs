using System;
using UnityEngine;

namespace GM.Units
{
    public interface IUnitController
    {
        void MoveTowards(Vector3 position);
        void MoveTowards(Vector3 position, Action action);
        void LookAt(Vector3 position);
    }

    public abstract class UnitController : GM.Core.GMMonoBehaviour, IUnitController
    {
        public IMovementController Movement { get; set; }

        protected virtual void GetComponents()
        {
            Movement = GetComponent<IMovementController>();
        }

        public void MoveTowards(Vector3 position) => Movement.MoveTowards(position);
        public void MoveTowards(Vector3 position, Action callback) => Movement.MoveTowards(position, callback);
        public void LookAt(Vector3 position) => Movement.LookAt(position);
    }
}