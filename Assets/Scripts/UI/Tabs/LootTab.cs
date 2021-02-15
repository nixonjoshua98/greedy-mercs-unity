using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Numerics;
using System.Collections.Generic;

using SimpleJSON;

using Vector3 = UnityEngine.Vector3;

namespace GreedyMercs
{
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

        public static int BuyAmount { get { return Instance.buyAmount.BuyAmount; } }

        List<LootUpgradeRow> rows;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            rows = new List<LootUpgradeRow>();

            foreach (var relic in GameState.Loot.Unlocked())
            {
                AddRow(relic.Key);
            }

            InvokeRepeating("UpdateUI", 0.0f, 0.1f);
        }

        void AddRow(LootID item)
        {
            LootItemSO scriptable = StaticData.LootList.Get(item);

            GameObject inst = Utils.UI.Instantiate(LootRowObject, rowParent.transform, Vector3.zero);

            LootUpgradeRow row = inst.GetComponent<LootUpgradeRow>();

            row.Init(scriptable);

            rows.Add(row);
        }

        void UpdateUI()
        {
            prestigePointText.text      = Utils.Format.FormatNumber(GameState.Inventory.prestigePoints);
            buyLootButton.interactable  = GameState.Loot.Count < StaticData.LootList.Count;

            if (GameState.Loot.Count < StaticData.LootList.Count)
            {
                lootCostText.text = string.Format("Buy Loot\n{0}", Utils.Format.FormatNumber(Formulas.CalcNextLootCost(GameState.Loot.Count)));
            }

            else
                lootCostText.text = "All Loot\nObtained";
        }

        // === Static Methods ===

        public static void AddBuyAmountListener(UnityAction<int> callback)
        {
            Instance.buyAmount.OnBuyAmountChanged.AddListener(callback);
        }

        // === Button Callbacks ===

        public void OnBuyLoot()
        {
            if (GameState.Inventory.prestigePoints < Formulas.CalcNextLootCost(GameState.Loot.Count))
            {
                Utils.UI.ShowMessage("Poor Player Alert", "You cannot afford to buy a new item");
            }

            else if (GameState.Loot.Count < StaticData.LootList.Count)
            {
                Server.BuyLootItem(OnBuyCallback, Utils.Json.GetDeviceNode());
            }
        }

        public void OnBuyCallback(long code, string data)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(data);

                LootID item = (LootID)node["newLootId"].AsInt;

                GameState.Inventory.prestigePoints = BigInteger.Parse(node["remainingPoints"].Value);

                GameState.Loot.Add(item);

                AddRow(item);
            }

            else
            {
                Utils.UI.ShowMessage("Purchase Loot", "Failed to buy item :(");
            }

            UpdateUI();
        }
    }
}