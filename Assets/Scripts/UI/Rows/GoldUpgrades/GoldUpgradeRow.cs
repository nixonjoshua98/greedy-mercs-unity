
using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public abstract class GoldUpgradeRow : MonoBehaviour
    {
        protected GoldUpgradeID upgrade;

        [Header("Components")]
        [SerializeField] protected Button buyButton;
        [Space]
        [SerializeField] protected Text costText;
        [SerializeField] protected Text nameText;
        [SerializeField] protected Text damageText;

        protected abstract int BuyAmount { get; }

        void FixedUpdate()
        {
            UpdateRow();
        }

        public abstract void OnBuy();
        protected abstract void UpdateRow();

        protected void BuyUpgrade(BigDouble cost, int levelsBuying)
        {
            if (GameState.Player.gold >= cost)
            {
                UpgradeState state = GameState.Upgrades.GetUpgrade(upgrade);

                state.level += levelsBuying;

                GameState.Player.gold -= cost;
            }
        }
    }
}