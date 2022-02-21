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

        // = Properties = //
        bool IsTargetValid => CurrentTarget != null && CurrentTarget.GameObject != null;
        bool IsInContactWithTarget => Mathf.Abs(transform.position.x - CurrentTarget.GameObject.transform.position.x) < (Speed * Time.fixedDeltaTime);


        public void Init(Target target, Action action)
        {
            Callback = action;
            CurrentTarget = target;

            Direction = target.GameObject.transform.position - transform.position;
        }

        void FixedUpdate()
        {
            MoveDirection();

            if (!hasConcluded)
            {
                if (!IsTargetValid)
                {
                    hasConcluded = true;

                    Callback.Invoke();

                    this.Fade(1, () => Destroy(gameObject));
                }

                else if (IsInContactWithTarget)
                {
                    hasConcluded = true;

                    InstantiateImpactPS();

                    Callback.Invoke();

                    Destroy(gameObject);
                }
            }
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
