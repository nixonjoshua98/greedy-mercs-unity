using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace GM.Units
{
    public class UnitMovementController : MonoBehaviour, Temp_IUnitMovement
    {
        [Header("Properties")]
        [SerializeField] float moveSpeed = 1.5f;

        public UnitAvatar Avatar;

        public void MoveTowards(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

            Avatar.Animator.Play(Avatar.AnimationStrings.Walk);

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

                action.Invoke();
            }

            StartCoroutine(_MoveTowards());
        }



        public void MoveDirection(Vector3 dir)
        {
            MoveTowards(transform.position + (dir * moveSpeed));
        }

        public void FaceDirection(Vector3 dir) => FaceTowardsTarget(transform.position + dir);


        public void FaceTowards(GameObject o) { FaceTowardsTarget(o.transform.position); }
        public void FaceTowardsTarget(Vector3 pos)
        {
            // Flip the avatar x scale to face the target position

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
