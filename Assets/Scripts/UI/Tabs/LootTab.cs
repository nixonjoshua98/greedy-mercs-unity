using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Numerics;
using System.Collections.Generic;

using SimpleJSON;

using Vector3 = UnityEngine.Vector3;

namespace GreedyMercs
{
    using GM.Artefacts;
    using GM.Inventory;

    public class LootTab : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] GameObject rowParent;

        [Header("Components")]
        [SerializeField] Button buyLootButton;

        [Header("Text Components")]
        [SerializeField] Text prestigePointText;
        [SerializeField] Text lootCostText;

        [Header("Prefabs")]
        [SerializeField] GameObject LootRowObject;

        List<LootUpgradeRow> rows;

        void Start()
        {
            rows = new List<LootUpgradeRow>();

            foreach (ArtefactState state in ArtefactManager.Instance.StatesList)
            {
                AddRow(state.ID);
            }
        }

        void AddRow(int artefactId)
        {
            ArtefactData data = StaticData.Artefacts.Get(artefactId);

            GameObject inst = Utils.UI.Instantiate(LootRowObject, rowParent.transform, Vector3.zero);

            LootUpgradeRow row = inst.GetComponent<LootUpgradeRow>();

            row.Init(data.ID);

            rows.Add(row);
        }

        void FixedUpdate()
        {
            InventoryManager inv = InventoryManager.Instance;
            ArtefactManager arts = ArtefactManager.Instance;

            prestigePointText.text      = Utils.Format.FormatNumber(inv.prestigePoints);
            buyLootButton.interactable  = arts.Count < StaticData.Artefacts.Count;

            if (arts.Count < StaticData.Artefacts.Count)
            {
                lootCostText.text = string.Format("{0}", Utils.Format.FormatNumber(Formulas.CalcNextLootCost(arts.Count)));
            }

            else
                lootCostText.text = "All Loot\nObtained";
        }

        // === Button Callbacks ===

        public void OnBuyLoot()
        {
            Server.BuyLootItem(OnBuyCallback, Utils.Json.GetDeviceInfo());
        }

        public void OnBuyCallback(long code, string data)
        {
            InventoryManager inv = InventoryManager.Instance;

            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(data);

                int item = node["newLootId"].AsInt;

                inv.prestigePoints = node["remainingPoints"].AsInt;

                ArtefactManager.Instance.Temp_Add(item);

                AddRow(item);
            }

            else
            {
                Utils.UI.ShowMessage("Purchase Loot", "Failed to buy item :(");
            }
        }
    }
}