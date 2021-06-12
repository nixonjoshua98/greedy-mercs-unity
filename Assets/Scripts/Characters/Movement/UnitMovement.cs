using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Movement
{
    public class UnitMovement : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] float moveSpeed = 0.5f;

        [Header("References")]
        [SerializeField] GameObject avatar;

        Animator anim;

        void Awake()
        {
            anim = avatar.GetComponentInChildren<Animator>();
        }

        public void MoveTowards(Vector3 target, bool animate = true)
        {
            Vector3 targetPosition = new Vector3(target.x, target.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            if (animate)
            {
                FaceTowardsTarget(target);

                anim.Play(Vector3.Distance(transform.position, targetPosition) == 0.0f ? "Idle" : "Walk");
            }
        }

        public void FaceTowardsTarget(GameObject o) { FaceTowardsTarget(o.transform.position); }

        public void FaceTowardsTarget(Vector3 pos)
        {
            // Flip the avatar x scale to face the target position

            bool isTargetRight = pos.x > transform.position.x;
            bool isFacingRight = avatar.transform.localScale.x >= 0.0f;

            if (isFacingRight != isTargetRight)
            {
                Vector3 scale = avatar.transform.localScale;

                scale.x *= -1.0f;

                avatar.transform.localScale = scale;
            }
        }
    }

}