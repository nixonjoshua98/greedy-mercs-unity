using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class UnitMovement : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] float moveSpeed = 1.25f;

        [Header("Components")]
        [SerializeField] GameObject avatar;

        [Header("Animations")]
        [SerializeField] string walkAnimation = "Walk";

        Animator anim;

        void Awake()
        {
            anim = avatar.GetComponentInChildren<Animator>();
        }

        public void MoveTowards(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

            anim.Play(walkAnimation);

            FaceTowardsTarget(target);
        }

        public void MoveDirection(Vector3 dir)
        {
            MoveTowards(transform.position + (dir * 100.0f));
        }

        public void FaceTowards(GameObject o) { FaceTowardsTarget(o.transform.position); }

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

        // = = = Methods = = = //

        public bool IsCurrentAnimationWalk()
        {
            return anim.GetCurrentAnimatorStateInfo(0).IsName(walkAnimation);
        }
    }
}
