using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MercMovement : MercComponent
    {
        [Header("Properties")]
        [SerializeField] float moveSpeed = 1.5f;

        [SerializeField] GameObject avatar;

        public void MoveTowards(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

            AvatarAnimator.Play(Animations.Walk);

            FaceTowards(target);
        }

        public void MoveDirection(Vector3 dir)
        {
            MoveTowards(transform.position + (dir * moveSpeed));
        }

        public void FaceTowards(GameObject o)
        {
            FaceTowards(o.transform.position);
        }

        public void FaceTowards(Vector3 pos)
        {
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