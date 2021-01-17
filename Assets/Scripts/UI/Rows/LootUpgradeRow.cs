using System.Numerics;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

namespace UI.Loot
{
    using LootData;

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

        [Header("Prefabs")]
        [SerializeField] GameObject BlankPanel;

        GameObject spawnedBlankPanel;

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

            lootId = data.ItemID;

            Utils.UI.SetImageScaleW(icon, data.icon, 150.0f);

            UpdateRow();

            InvokeRepeating("UpdateRow", 0.0f, 0.25f);
        }

        void UpdateRow()
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


            buyButton.interactable = state.level < scriptable.maxLevel;
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

            if (levelsBuying > 0 && GameState.Player.prestigePoints >= cost && (state.level + levelsBuying) <= data.maxLevel)
            {
                spawnedBlankPanel = Utils.UI.Instantiate(BlankPanel, UnityEngine.Vector3.zero);

                JSONNode node = Utils.Json.GetDeviceNode();

                node.Add("itemId", (int)lootId);
                node.Add("buyLevels", BuyAmount);

                Server.UpgradeLootItem(this, OnUpgradeCallback, node);
            }
        }

        void OnUpgradeCallback(long code, string compressed)
        {
            if (code == 200)
            {
                JSONNode node       = Utils.Json.Decompress(compressed);
                UpgradeState state  = GameState.Loot.Get(lootId);

                state.level = node["itemLevel"].AsInt;

                GameState.Player.prestigePoints = BigInteger.Parse(node["prestigePoints"].Value);
            }

            else
            {
                Utils.UI.ShowMessage("Server Error ", "Failed to upgrade item :(");
            }

            UpdateRow();

            Destroy(spawnedBlankPanel);
        }
    }
}