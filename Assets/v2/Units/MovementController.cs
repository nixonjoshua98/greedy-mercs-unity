using System.Collections;
using UnityEngine;

namespace GM.Units
{
    public class MovementController : MonoBehaviour
    {
        public UnitAvatar Avatar;

        [Header("Properties")]
        public float MoveSpeed = 2.5f;

        public void MoveTowards(Vector3 target, float moveSpeed, bool playAnimation = true)
        {
            LookAtDirection(target - transform.position);

            transform.position = Vector3.MoveTowards(transform.position, target, Time.fixedDeltaTime * moveSpeed);

            if (playAnimation)
            {
                Avatar.PlayAnimation(Avatar.Animations.Walk);
            }
        }

        public void MoveTowards(Vector3 target, bool playAnimation = true)
        {
            MoveTowards(target, MoveSpeed, playAnimation: playAnimation);
        }

        public void MoveDirection(Vector3 dir, bool playAnimation = true)
        {
            MoveDirection(dir, MoveSpeed, playAnimation: playAnimation);
        }

        public void MoveDirection(Vector3 dir, float moveSpeed, bool playAnimation = true)
        {
            MoveTowards(transform.position + (dir * 100), moveSpeed, playAnimation: playAnimation);
        }

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
            LookAt(transform.position + (dir * 100));
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
