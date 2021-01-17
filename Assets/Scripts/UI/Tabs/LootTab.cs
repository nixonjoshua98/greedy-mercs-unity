using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using SimpleJSON;

using Vector3 = UnityEngine.Vector3;

namespace UI.Loot
{
    using LootData;

    public class LootTab : MonoBehaviour
    {
        static LootTab Instance = null;

        [Header("GameObjects")]
        [SerializeField] GameObject rowParent;

        [Space]
        [SerializeField] BuyAmountController buyAmount;

        [Header("Components")]
        [SerializeField] Button buyLootButton;

        [Header("Text Components")]
        [SerializeField] Text prestigePointText;
        [SerializeField] Text lootCostText;

        [Header("Prefabs")]
        [SerializeField] GameObject LootRowObject;

        [SerializeField] GameObject BlankPanel;

        public static int BuyAmount { get { return Instance.buyAmount.BuyAmount; } }

        GameObject spawnedBlankPanel;

        List<LootUpgradeRow> rows;

        void Awake()
        {
            Instance = this;

            rows = new List<LootUpgradeRow>();

            foreach (var relic in GameState.Loot.Unlocked())
            {
                AddRow(relic.Key);
            }
        }

        void AddRow(LootID item)
        {
            LootItemSO scriptable = StaticData.LootList.Get(item);

            GameObject inst = Utils.UI.Instantiate(LootRowObject, rowParent.transform, Vector3.zero);

            LootUpgradeRow row = inst.GetComponent<LootUpgradeRow>();

            row.Init(scriptable);

            rows.Add(row);
        }

        void FixedUpdate()
        {
            prestigePointText.text      = Utils.Format.FormatNumber(GameState.Player.prestigePoints);
            buyLootButton.interactable  = GameState.Loot.Count < StaticData.LootList.Count;

            if (GameState.Loot.Count < StaticData.LootList.Count)
            {
                lootCostText.text = string.Format("Buy Loot\n{0}", Utils.Format.FormatNumber(Formulas.CalcNextLootCost(GameState.Loot.Count)));
            }

            else
                lootCostText.text = "All Loot\nObtained";
        }

        // === Button Callbacks ===

        public void OnBuyLoot()
        {
            if (GameState.Player.prestigePoints < Formulas.CalcNextLootCost(GameState.Loot.Count))
            {
                Utils.UI.ShowMessage("Poor Player Alert", "You cannot afford to buy a new item");
            }

            else if (GameState.Loot.Count < StaticData.LootList.Count)
            {
                spawnedBlankPanel = Utils.UI.Instantiate(BlankPanel, Vector3.zero);

                Server.BuyLootItem(this, OnBuyCallback, Utils.Json.GetDeviceNode());
            }
        }

        public void OnBuyCallback(long code, string data)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(data);

                LootID item = (LootID)node["newLootId"].AsInt;

                GameState.Player.Update(node);

                GameState.Loot.Add(item);

                AddRow(item);
            }

            else
            {
                Utils.UI.ShowMessage("Buy Loot Item", "Failed to buy item :(");
            }

            Destroy(spawnedBlankPanel);
        }
    }
}