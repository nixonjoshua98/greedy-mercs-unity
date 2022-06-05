using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MeleeMercController : AbstractMercController
    {
        protected override bool CanFetchTarget()
        {
            return SquadController.GetIndex(this) <= 1;
        }

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
            var attackSide = CurrentTarget.GetAttackSide(this);

            Vector3 pos = attackSide switch
            {
                Units.AttackSide.Left => CurrentTarget.Unit.Avatar.Bounds.min - new Vector3(Avatar.Bounds.size.x / 2, 0),
                Units.AttackSide.Right => CurrentTarget.Unit.Avatar.Bounds.max + new Vector3(Avatar.Bounds.size.x / 2, 0),
                _ => throw new System.Exception("Invalid attack side")
            };


            pos.y = CurrentTarget.Unit.transform.position.y;

            return pos;
        }

        protected override bool CanStartAttack()
        {
            return (transform.position == GetCurrentTargetPosition()) && base.CanStartAttack();
        }
    }
}
