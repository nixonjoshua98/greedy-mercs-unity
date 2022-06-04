using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MeleeMercController : AbstractMercController
    {
        protected override void MoveTowardsCurrentTarget()
        {
            Vector3 targetPosition = GetCurrentTargetPosition();

            Movement.MoveTowards(targetPosition);

            if (transform.position == targetPosition)
            {
                Movement.LookAt(CurrentTarget.Unit.transform.position);
            }
        }

        private Vector3 GetCurrentTargetPosition()
        {
            Vector3 pos = CurrentTarget.Unit.Avatar.Bounds.min - new Vector3(Avatar.Bounds.size.x / 2, 0);

            pos.y = CurrentTarget.Unit.transform.position.y;

            return pos;
        }

        protected override bool CanStartAttack()
        {
            return (transform.position == GetCurrentTargetPosition()) && base.CanStartAttack();
        }
    }
}
