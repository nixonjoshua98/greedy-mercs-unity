
using UnityEngine;

namespace GM.Characters
{
    public class CharacterController : AbstractCharacterController
    {
        [SerializeField] int characterId = -1;

        // Components
        AbstractCharacterAttack attackController;

        void Awake()
        {
            GetComponents();

            if (characterId == -1)
                Debug.LogError("CharacterID cannot be -1");
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
            GreedyMercs.GameManager.TryDealDamageToEnemy(GreedyMercs.StatsCache.CharacterDamage((CharacterID)characterId));
        }

        protected override bool CanAttack(GameObject obj)
        {
            return attackController.IsAvailable();
        }
    }
}