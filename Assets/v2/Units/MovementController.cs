using System.Collections;
using UnityEngine;

namespace GM.Units
{
    public class MovementController : MonoBehaviour
    {
        public UnitAvatar Avatar;

        [Header("Properties")]
        public float MoveSpeed = 2.5f;

        Vector3 _CurrentMovingDirection = Vector3.zero;

        public void MoveTowards(Vector3 target, bool playAnimation = true)
        {
            UpdateCurrentMovingDirection(target);

            LookAtDirection(target - transform.position);

            transform.position = Vector3.MoveTowards(transform.position, target, Time.fixedDeltaTime * MoveSpeed);

            if (playAnimation)
            {
                Avatar.PlayAnimation(Avatar.Animations.Walk);
            }
        }

        void UpdateCurrentMovingDirection(Vector3 targetPosition)
        {
            _CurrentMovingDirection = (targetPosition - transform.position).normalized;
        }

        public void MoveDirection(Vector3 dir, bool playAnimation = true) => MoveTowards(transform.position + (dir * MoveSpeed), playAnimation: playAnimation);
        public void Continue() => MoveTowards(transform.position + (_CurrentMovingDirection * MoveSpeed));

        public IEnumerator MoveTowardsEnumerator(Vector3 target)
        {
            while (transform.position != target)
            {
                MoveTowards(target);

                yield return new WaitForFixedUpdate();
            }
        }

        void LookAtDirection(Vector3 dir)
        {
            LookAt(transform.position + (dir * MoveSpeed));
        }

        void LookAt(Vector3 pos)
        {
            bool isTargetRight = pos.x > transform.position.x;
            bool isFacingRight = Avatar.transform.localScale.x >= 0.0f;

            if (isFacingRight != isTargetRight)
            {
                Vector3 scale = Avatar.transform.localScale;

                scale.x *= -1.0f;

                Avatar.transform.localScale = scale;
            }
        }
    }
}
