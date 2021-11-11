using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Controllers
{
    public class MercController : MonoBehaviour
    {
        [SerializeField] protected MercID _ID = MercID.NONE;
        public MercID ID { get => _ID; }

        [Header("References")]
        [SerializeField] MercAttackController attackController;

        [Header("Debug")]
        [SerializeField] MercActivity status = MercActivity.IDLE;

        GameObject currentTarget;

        void FixedUpdate()
        {
            switch (status)
            {
                case MercActivity.IDLE:
                    HandleIdleFixedUpdate();
                    break;

                case MercActivity.ATTACKING_BOSS:
                    HandleBossFightFixedUpdate();
                    break;
            }
        }

        void HandleIdleFixedUpdate()
        {
            if (attackController.IsReady)
            {
                // We do not have a current target or the current target is already dead
                if (currentTarget == null || (currentTarget.TryGetComponent(out HealthController health) && health.CurrentHealth == 0))
                {
                    GameManager.Instance.TryGetWaveEnemy(out currentTarget);
                }
                else
                {
                    transform.position = currentTarget.transform.position;

                    attackController.AttackTarget(currentTarget);
                }
            }
        }

        void HandleBossFightFixedUpdate()
        {
            if (attackController.IsReady)
            {
                attackController.AttackTarget(currentTarget);
            }
        }

        /// <summary>Move to a new position and callback once we reach the target position</summary>
        public void MoveTo(Vector3 pos, Action callback)
        {
            //status = MercActivity.PRIORITY_MOVING;

            transform.position = pos;

            status = MercActivity.IDLE;

            callback.Invoke();
        }

        /// <summary>Merc will instantly start attacking the boss from wherever they are currently</summary>
        public void StartBossBattle(GameObject boss)
        {
            status = MercActivity.ATTACKING_BOSS;

            currentTarget = boss;

            // Reset the activity once the boss has died
            boss.GetComponent<HealthController>().E_OnZeroHealth.AddListener(() => {
                status = MercActivity.IDLE;
            });
        }
    }
}