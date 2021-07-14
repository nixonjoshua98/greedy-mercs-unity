using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class ATS_MercMeleeAttack : ATS_Attack
    {
        float attackRange = 0.75f;

        public override Vector3 GetMoveVector(GameObject target)
        {
            return GetTargetMovePosition(target);
        }


        public override bool InAttackPosition(GameObject target)
        {
            Vector3 targetMovePosition = GetTargetMovePosition(target);

            float dist = Vector3.Distance(transform.position, targetMovePosition);

            return dist <= attackRange;
        }


        // We do not always want to directly go towards the target position, so we raycast in the general location of the target
        // and we choose the closest position to the target as our target position to move towards. If no collider was found
        // then we will jsut move directly to the target position (normally at the base of the avatar)
        // Note: We may run into issues with multiple colliders in the same direction since we only raycast once
        Vector3 GetTargetMovePosition(GameObject target)
        {
            Vector3 current         = target.transform.position;
            float currentDistance   = Vector3.Distance(transform.position, current);

            for (float yOffset = 2.0f; yOffset > 0.0; yOffset -= 0.25f)
            {
                Vector3 targetPosition = target.transform.position + new Vector3(0, yOffset);

                Vector3 dir = targetPosition - transform.position;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 10.0f);

                if (hit.collider && hit.collider.gameObject == target)
                {
                    float dist = Vector3.Distance(transform.position, targetPosition);

                    if (dist < currentDistance)
                    {
                        current         = targetPosition;
                        currentDistance = dist;
                    }

                    Debug.DrawRay(transform.position, dir, Color.green);
                }
            }

            Debug.DrawLine(transform.position, current, Color.red);

            return current;
        }
    }
}