using System.Collections;
using UnityEngine;

namespace GM.Units
{
    public class MovementController : MonoBehaviour
    {
        public UnitAvatar Avatar;

        [Header("Properties")]
        public float MoveSpeed = 2.5f;

        public void MoveTowards(Vector3 target)
        {
            LookAtDirection(target - transform.position);

            transform.position = Vector3.MoveTowards(transform.position, target, Time.fixedDeltaTime * MoveSpeed);

            Avatar.PlayAnimation(Avatar.Animations.Walk);
        }

        public void MoveDirection(Vector3 dir) => MoveTowards(transform.position + (dir * MoveSpeed));

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
