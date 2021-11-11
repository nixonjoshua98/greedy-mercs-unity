using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Mercs.Controllers
{
    public class MercMovement : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] float moveSpeed = 1.5f;

        [Header("Components")]
        [SerializeField] GameObject avatar;

        public void MoveTowards(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

            FaceTowards(target);
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