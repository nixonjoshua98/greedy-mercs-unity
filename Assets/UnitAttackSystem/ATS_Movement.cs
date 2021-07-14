using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class ATS_Movement : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] float moveSpeed = 1.0f;

        [Header("References")]
        [SerializeField] GameObject avatar;

        Animator anim;

        void Awake()
        {
            anim = avatar.GetComponentInChildren<Animator>();
        }

        public float MoveTowards(Vector3 target)
        {
            Vector3 originalPosition = transform.position;

            Vector3 targetPosition = new Vector3(target.x, target.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            float distanceTravelled = Vector3.Distance(originalPosition, transform.position);

            anim.Play("Walk");

            FaceTowardsTarget(target);

            return distanceTravelled;
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
            return anim.GetCurrentAnimatorStateInfo(0).IsName("Walk");
        }
    }
}
