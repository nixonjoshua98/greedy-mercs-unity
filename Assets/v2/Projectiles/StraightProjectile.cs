using GM.Targets;
using System;
using UnityEngine;

namespace GM.Units.Projectiles
{
    public class StraightProjectile : Core.GMMonoBehaviour, IProjectile
    {
        [SerializeField] GameObject ImpactPS;

        bool hasConcluded = false;

        Action Callback;
        Target CurrentTarget;

        Vector3 Direction;

        [Header("Properties")]
        public float Speed = 1.0f;

        public void Init(Target target, Action action)
        {
            Callback = action;
            CurrentTarget = target;

            Direction = target.Position - transform.position;
        }

        void FixedUpdate()
        {
            MoveDirection();

            if (!hasConcluded)
            {
                if (CurrentTarget == null)
                {
                    InvokeCallback(1.0f);
                }

                else if (Mathf.Abs(transform.position.x - CurrentTarget.Position.x) < 0.1f)
                {
                    InstantiateImpactPS();
                    InvokeCallback(0.0f);
                }
            }
        }

        void InvokeCallback(float fadeDur)
        {
            hasConcluded = true;

            this.Fade(fadeDur, () => Destroy(gameObject));

            Callback.Invoke();
        }

        void InstantiateImpactPS()
        {
            Instantiate(ImpactPS, transform.position);
        }

        void MoveDirection()
        {
            Vector3 targetPosition = transform.position + (new Vector3(Direction.x, 0) * Speed);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.fixedDeltaTime);
        }
    }
}
