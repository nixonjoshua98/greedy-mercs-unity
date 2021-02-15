using System.Numerics;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

namespace GreedyMercs
{
    public class LootUpgradeRow : MonoBehaviour
    {
        LootID lootId;

        [Header("Components")]
        [SerializeField] Image icon;
        [Space]
        [SerializeField] Button buyButton;
        [Space]
        [SerializeField] Text costText;
        [SerializeField] Text nameText;
        [SerializeField] Text effectText;

        protected LootItemSO itemData => StaticData.LootList.Get(lootId);
        protected UpgradeState itemState => GameState.Loot.Get(lootId);

        int BuyAmount
        {
            get
            {
                return Mathf.Min(LootTab.BuyAmount, itemData.maxLevel - itemState.level);
            }
        }

        public void Init(LootItemSO data)
        {
            lootId = data.ItemID;

            nameText.text   = data.name;
            icon.sprite     = data.icon;

            UpdateUI();
        }

        void OnEnable()
        {
            if (GameState.Loot.Contains(lootId))
                InvokeRepeating("UpdateUI", 0.0f, 0.5f);
        }
        
        void OnDisable() => CancelInvoke("UpdateUI");

        void UpdateUI()
        {
            UpdateEffectText();

            nameText.text = string.Format("(Lvl. {0}) {1}", itemState.level, itemData.name);

            if (itemState.level < itemData.maxLevel)
            {
                string cost = Utils.Format.FormatNumber(Formulas.CalcLootItemLevelUpCost(lootId, BuyAmount));

                costText.text = string.Format("x{0}\n{1}", BuyAmount, cost);
            }

            else
                costText.text = "MAX";


            buyButton.interactable = itemState.level < itemData.maxLevel && GameState.Inventory.prestigePoints >= Formulas.CalcLootItemLevelUpCost(lootId, BuyAmount);
        }

        void UpdateEffectText()
        {
            double effect = Formulas.CalcLootItemEffect(lootId);

            switch (itemData.valueType)
            {
                case ValueType.MULTIPLY:
                    effectText.text = Utils.Format.FormatNumber(effect * 100) + "%";
                    break;

                case ValueType.ADDITIVE_PERCENT:
                    effectText.text = "+ " + Utils.Format.FormatNumber(effect * 100) + "%";
                    break;

                case ValueType.ADDITIVE_FLAT_VAL:
                    effectText.text = "+ " + Utils.Format.FormatNumber(effect);
                    break;
            }

            effectText.text += " " + Utils.Generic.BonusToString(itemData.bonusType);
        }

        // === Button Callbacks

        public void OnBuy()
        {
            int levelsBuying = BuyAmount;

            BigInteger cost = Formulas.CalcLootItemLevelUpCost(lootId, levelsBuying);

            void ServerCallback(long code, string compressed) => OnUpgradeCallback(levelsBuying, code, compressed);

            if (levelsBuying > 0 && GameState.Inventory.prestigePoints >= cost && (itemState.level + levelsBuying) <= itemData.maxLevel)
            {
                JSONNode node = Utils.Json.GetDeviceNode();

                node.Add("itemId", (int)lootId);
                node.Add("buyLevels", BuyAmount);

                Server.UpgradeLootItem(ServerCallback, node);
            }
        }

        void OnUpgradeCallback(int levelsBuying, long code, string compressed)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressed);

                itemState.level += levelsBuying;

                GameState.Inventory.prestigePoints = BigInteger.Parse(node["remainingPoints"].Value);
            }

            UpdateUI();
        }
    }
}