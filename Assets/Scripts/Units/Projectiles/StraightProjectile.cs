
using UnityEngine;
using UnityEngine.Events;

namespace GM.Projectiles
{
    public class StraightProjectile : Projectile
    {
        protected UnityAction callback;

        Vector3 targetPosition;

        public override void Setup(Transform target, UnityAction action)
        {
            callback = action;

            targetPosition = new Vector3(target.transform.position.x, transform.position.y);
        }


        protected override void UpdatePosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
           
            if (Vector3.Distance(transform.position, targetPosition) == 0.0f)
            {
                OnProjectileConnected();
            }
        }

        void OnProjectileConnected()
        {
            callback.Invoke();

            Destroy(gameObject);
        }
    }
}
