using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Numerics;
using System.Collections.Generic;

using Vector3 = UnityEngine.Vector3;

namespace GM.Artefacts
{
    using GM.Inventory;

    using GreedyMercs;

    using Utils = GreedyMercs.Utils;

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

        List<GameObject> rows;

        void Awake()
        {
            rows = new List<GameObject>();
        }

        void Start()
        {
            InstantiateRows();
        }

        void InstantiateRows()
        {
            Clear();

            foreach (ArtefactState state in ArtefactManager.Instance.StatesList)
            {
                GameObject inst = Funcs.UI.Instantiate(LootRowObject, rowParent.transform, Vector3.zero);

                ArtefactRow row = inst.GetComponent<ArtefactRow>();

                row.Init(state.ID);

                rows.Add(inst);
            }
        }

        void Clear()
        {
            foreach (GameObject r in rows)
            {
                Destroy(r);
            }

            rows.Clear();
        }

        void FixedUpdate()
        {
            InventoryManager inv = InventoryManager.Instance;
            ArtefactManager arts = ArtefactManager.Instance;

            prestigePointText.text      = Utils.Format.FormatNumber(inv.PrestigePoints);
            buyLootButton.interactable  = arts.Count < StaticData.Artefacts.Count;

            lootCostText.text = "-";

            if (arts.Count < StaticData.Artefacts.Count)
            {
                lootCostText.text = string.Format("{0}", Utils.Format.FormatNumber(Formulas.CalcNextLootCost(arts.Count)));
            }
        }

        // === Button Callbacks ===

        public void OnPurchaseArtefactBtn()
        {
            void ServerCallback(bool purchased)
            {
                if (purchased)
                {
                    InstantiateRows();
                }
            }

            ArtefactManager.Instance.PurchaseNewArtefact(ServerCallback);
        }
    }
}