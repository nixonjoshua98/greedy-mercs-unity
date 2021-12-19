using System;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public interface IMovementController
    {
        public void MoveTowards(Vector3 target);
        public void MoveTowards(Vector3 target, Action action);
        public void MoveDirection(Vector3 dir);
        public void FaceTowards(GameObject obj);
    }
}
