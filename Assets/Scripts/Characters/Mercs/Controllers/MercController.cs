
using UnityEngine;

namespace GM.Units
{
    using GM.Targetting;
    public class MercController : AbstractMercController
    {
        [SerializeField] UnitID characterId;

        // Components
        AbstractCharacterAttack attackController;

        // IComponents
        IAttackTarget attackTargetter;

        void Start()
        {
            GetComponents();
        }

        void GetComponents()
        {
            attackTargetter = GetComponent<IAttackTarget>();

            attackController = GetComponent<AbstractCharacterAttack>();

            GetComponent<AbstractCharacterAttack>().E_OnAttackHit.AddListener(OnAttackHit);
        }

        protected override void PeriodicUpdate()
        {
            if (attackController.IsAvailable)
            {
                GameObject obj = attackTargetter.GetTarget();

                if (obj && CanAttack(obj))
                {
                    StartAttack(obj);
                }
            }
        }

        protected override void StartAttack(GameObject obj)
        {
            attackController.Attack(obj);
        }

        // obj may be Null under some cases such as an attack being delayed
        public override void OnAttackHit(GameObject obj)
        {
            if (obj && obj.TryGetComponent(out AbstractHealthController hp))
            {
                BigDouble dmg = StatsCache.TotalMercDamage(characterId);

                StatsCache.ApplyCritHit(ref dmg);

                hp.TakeDamage(dmg);
            }
        }

        protected override bool CanAttack(GameObject obj)
        {
            return attackController.CanAttack(obj);
        }
    }
}