using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GM.Units;
using System;

namespace GM.Mercs.Controllers
{
    public class MovementController : MonoBehaviour, IMovementController
    {
        public UnitAvatar Avatar;

        [Header("Properties")]
        public float MoveSpeed = 2.5f;

        public void MoveTowards(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MoveSpeed);

            Avatar.PlayAnimation(Avatar.AnimationStrings.Walk);

            FaceTowardsTarget(target);
        }


        public void MoveTowards(Vector3 target, Action action)
        {
            IEnumerator _MoveTowards()
            {
                while (transform.position != target)
                {
                    MoveTowards(target);

                    yield return new WaitForFixedUpdate();
                }

                Avatar.PlayAnimation(Avatar.AnimationStrings.Idle);

                action.Invoke();
            }

            StartCoroutine(_MoveTowards());
        }



        public void MoveDirection(Vector3 dir)
        {
            MoveTowards(transform.position + (dir * MoveSpeed));
        }

        public void FaceDirection(Vector3 dir) => FaceTowardsTarget(transform.position + dir);

        public void FaceTowards(GameObject o) { FaceTowardsTarget(o.transform.position); }

        public void FaceTowardsTarget(Vector3 pos)
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
