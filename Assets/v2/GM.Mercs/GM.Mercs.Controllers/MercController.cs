using UnityEngine;
using MercID = GM.Common.Enums.MercID;
using GM.Targets;

namespace GM.Mercs.Controllers
{
    public class MercController : GM.Core.GMMonoBehaviour
    {
        [SerializeField] protected MercID _ID = MercID.NONE;
        public MercID ID { get => _ID; }

        [Header("Attributes")]
        public float AttackRange = 0.25f;

        [Header("References")]
        public AnimationStrings Animations;
        [Space]
        public Animator AvatarAnimator;
        [SerializeField] MercAttackController attackController;
        public MercMovement Movement;

        Target target;
        MercActivity status = MercActivity.IDLE;

        protected GM.Mercs.Data.FullMercData MercData => App.Data.Mercs.GetMerc(_ID);
        protected bool IsCurrentTargetValid { get => GameManager.Instance.IsTargetValid(target); }

        void FixedUpdate()
        {
            if (attackController.IsReady)
            {
                if (!IsCurrentTargetValid)
                {
                    bool hasTarget = GameManager.Instance.GetWaveTarget(ref target, gameObject, MercData.AttackType);

                    status = hasTarget ? MercActivity.FIGHTING : MercActivity.IDLE;
                }

                switch (status)
                {
                    case MercActivity.IDLE:
                        AvatarAnimator.Play(Animations.Idle);
                        break;

                    case MercActivity.FIGHTING:
                        HandleFightingUpdate();
                        break;
                }
            }
        }

        void HandleFightingUpdate()
        {
            Vector3 attackPosition = attackController.GetAttackPosition(target);

            if (Vector2.Distance(attackPosition, transform.position) <= AttackRange)
            {
                Movement.FaceTowards(target.Object);

                attackController.AttackTarget(target.Object);
            }
            else
            {
                Movement.MoveTowards(attackPosition);
            }
        }

        public void AssignTarget(Target newTarget)
        {
            status = MercActivity.FIGHTING;
            target = newTarget;

            target.Object.GetComponent<HealthController>().E_OnZeroHealth.AddListener(() => {
                status = MercActivity.IDLE;
            });
        }
    }
}