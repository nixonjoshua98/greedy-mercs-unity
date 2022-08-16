using System.Collections;
using UnityEngine;

namespace SRC.Units
{
    public class MovementController : MonoBehaviour
    {
        public UnitAvatar Avatar;
        readonly float MoveSpeed = 8.0f;

        public IEnumerator MoveToPositionEnumerator(Vector3 position)
        {
            while (!MoveTowards(position))
            {
                yield return new WaitForFixedUpdate();
            }
        }

        public bool LerpTowards(Vector3 original, Vector3 destination, float value)
        {
            var newPosition = Vector3.Lerp(original, destination, value);
            bool inPosition = newPosition == transform.position;

            if (!inPosition)
            {
                LookAtPosition(newPosition);
                transform.position = newPosition;
                Avatar.Animator.Play(Avatar.Animations.Walk);
            }
            else if (inPosition || value == 1.0f)
            {
                Avatar.Animator.Play(Avatar.Animations.Idle);
            }

            return inPosition;
        }

        public bool MoveTowards(Vector3 position, float speed, bool playAnimation = true)
        {
            LookAtPosition(position);

            transform.position = Vector2.MoveTowards(transform.position, position, Time.fixedDeltaTime * speed);

            bool inPosition = Vector2.Distance(transform.position, position) == 0.0f;

            if (playAnimation)
            {
                Avatar.Animator.Play(inPosition ? Avatar.Animations.Idle : Avatar.Animations.Walk);
            }

            return inPosition;
        }

        public bool MoveTowards(Vector3 position)
        {
            return MoveTowards(position, MoveSpeed);
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

        public void LookAtDirection(Vector3 dir)
        {
            LookAtPosition(transform.position + (dir * 100));
        }

        public void LookAtPosition(Vector3 pos)
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
