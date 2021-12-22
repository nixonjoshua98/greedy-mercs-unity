using System;
using System.Collections;
using UnityEngine;

namespace GM.Units
{
    public interface IMovementController
    {
        public void MoveTowards(Vector3 target);
        public void MoveTowards(Vector3 target, Action action);
        public void LookAt(Vector3 position);
    }


    public class MovementController : MonoBehaviour, IMovementController
    {
        public UnitAvatar Avatar;

        [Header("Properties")]
        public float MoveSpeed = 2.5f;

        public void MoveTowards(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.fixedDeltaTime * MoveSpeed);

            Avatar.PlayAnimation(Avatar.AnimationStrings.Walk);

            LookAt(target);
        }


        public void MoveTowards(Vector3 target, Action action)
        {
            IEnumerator _MoveTowards()
            {
                while (transform.position != target)
                {
                    yield return new WaitForFixedUpdate();

                    MoveTowards(target);
                }

                Avatar.PlayAnimation(Avatar.AnimationStrings.Idle);

                action.Invoke();
            }

            StartCoroutine(_MoveTowards());
        }

        public void LookAt(Vector3 pos)
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
