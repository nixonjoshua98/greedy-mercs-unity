
using UnityEngine;

namespace GM.Characters
{
    public class CharacterController : AbstractCharacterController
    {
        [SerializeField] CharacterID characterId;

        // Components
        AbstractCharacterAttack attackController;

        void Awake()
        {
            GetComponents();
        }

        void GetComponents()
        {
            attackController = GetComponent<AbstractCharacterAttack>();
        }

        protected override void PeriodicUpdate()
        {
            if (CanAttack(null))
            {
                StartAttack(null);
            }
        }

        protected override void StartAttack(GameObject obj)
        {
            attackController.Attack(obj);
        }

        // = = = Callback = = = //
        public override void OnAttack()
        {
            GreedyMercs.GameManager.TryDealDamageToEnemy(GreedyMercs.StatsCache.TotalMercDamage(characterId));
        }

        protected override bool CanAttack(GameObject obj)
        {
            return attackController.IsAvailable();
        }
    }
}