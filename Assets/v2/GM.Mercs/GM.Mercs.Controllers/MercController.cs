using UnityEngine;
using MercID = GM.Common.Enums.MercID;
using GM.Targets;

namespace GM.Mercs.Controllers
{
    public class MercController : GM.Core.GMMonoBehaviour
    {
        [SerializeField] protected MercID _ID = MercID.NONE;
        public MercID ID { get => _ID; }

        [Header("References")]
        [SerializeField] MercAttackController attackController;
        [SerializeField] MercMovement moveController;

        [SerializeField] MercActivity status = MercActivity.IDLE;
        [SerializeField] AttackerTarget target;

        protected GM.Mercs.Data.FullMercData MercData => App.Data.Mercs.GetMerc(_ID);
        protected bool IsCurrentTargetValid { get => GameManager.Instance.IsTargetValid(target); }

        void FixedUpdate()
        {
            if (attackController.IsReady)
            {
                if (!IsCurrentTargetValid)
                {
                    bool hasTarget = GameManager.Instance.GetWaveTarget(ref target, MercData.AttackType);

                    status = hasTarget ? MercActivity.FIGHTING : MercActivity.IDLE;
                }

                else if (status == MercActivity.FIGHTING)
                {
                    HandleFightingUpdate();
                }
            }
        }

        void HandleFightingUpdate()
        {
            Vector3 attackPosition = attackController.GetAttackPosition(target);

            if (Vector2.Distance(attackPosition, transform.position) == 0.0f)
            {
                moveController.FaceTowards(target.Object);

                attackController.AttackTarget(target.Object);
            }
            else
            {
                moveController.MoveTowards(attackPosition);
            }
        }

        public void AssignTarget(AttackerTarget newTarget)
        {
            status = MercActivity.FIGHTING;
            target = newTarget;

            target.Object.GetComponent<HealthController>().E_OnZeroHealth.AddListener(() => {
                status = MercActivity.IDLE;
            });
        }
    }
}