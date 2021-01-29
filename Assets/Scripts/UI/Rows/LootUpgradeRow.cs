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
        [SerializeField] Text descText;

        int BuyAmount
        {
            get
            {
                LootItemSO data     = StaticData.LootList.Get(lootId);
                UpgradeState state  = GameState.Loot.Get(lootId);

                return Mathf.Min(LootTab.BuyAmount, data.maxLevel - state.level);
            }
        }

        public void Init(LootItemSO data)
        {
            descText.text = data.description;
            nameText.text = data.name;

            lootId      = data.ItemID;
            icon.sprite = data.icon;

            LootTab.AddBuyAmountListener(UpdateUI);

            UpdateUI();
        }

        void OnEnable() => UpdateUI(LootTab.BuyAmount);

        void UpdateUI(int amount = 0)
        {
            LootItemSO scriptable   = StaticData.LootList.Get(lootId);
            UpgradeState state      = GameState.Loot.Get(lootId);

            UpdateEffectText();

            nameText.text = string.Format("(Lvl. {0}) {1}", state.level, scriptable.name);

            if (state.level < scriptable.maxLevel)
            {
                string cost = Utils.Format.FormatNumber(Formulas.CalcLootItemLevelUpCost(lootId, BuyAmount));

                costText.text = string.Format("x{0}\n{1}", BuyAmount, cost);
            }

            else
                costText.text = "MAX";


            buyButton.interactable = state.level < scriptable.maxLevel && GameState.Player.prestigePoints >= Formulas.CalcLootItemLevelUpCost(lootId, BuyAmount);
        }

        void UpdateEffectText()
        {
            LootItemSO scriptable = StaticData.LootList.Get(lootId);

            double effect = Formulas.CalcLootItemEffect(lootId);

            switch (scriptable.valueType)
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

            effectText.text += " " + Utils.Generic.BonusToString(scriptable.bonusType);
        }

        // === Button Callbacks

        public void OnBuy()
        {
            int levelsBuying = BuyAmount;

            LootItemSO data     = StaticData.LootList.Get(lootId);
            UpgradeState state  = GameState.Loot.Get(lootId);

            BigInteger cost = Formulas.CalcLootItemLevelUpCost(lootId, levelsBuying);

            void ServerCallback(long code, string compressed)
            {
                OnUpgradeCallback(levelsBuying, code, compressed);
            }

            if (levelsBuying > 0 && GameState.Player.prestigePoints >= cost && (state.level + levelsBuying) <= data.maxLevel)
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

                UpgradeState state = GameState.Loot.Get(lootId);

                state.level += levelsBuying;

                GameState.Player.prestigePoints -= BigInteger.Parse(node["upgradeCost"].Value);
            }

            GameState.Loot.Save();

            UpdateUI();
        }
    }
}