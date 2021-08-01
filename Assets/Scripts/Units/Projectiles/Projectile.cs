using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GM.Projectiles
{
    public interface IProjectile
    {
        public void Setup(Transform target, UnityAction action);
    }


    public abstract class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField] protected float speed = 1.0f;

        public abstract void Setup(Transform target, UnityAction action);

        void FixedUpdate()
        {
            UpdatePosition();
        }

        protected abstract void UpdatePosition();
    }
}
