using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;


namespace GM.Artefacts
{
    using GM.Data;

    using GM;

    public class LootTab : ExtendedMonoBehaviour
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
                GameObject inst = CanvasUtils.Instantiate(LootRowObject, rowParent.transform);

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

        protected override void PeriodicUpdate()
        {
            int pp = UserData.Get().Inventory.PrestigePoints;

            int numUnlockedArtefacts    = ArtefactManager.Instance.Count;
            int maxUnlockableArts       = GameData.Get().Artefacts.Count;

            prestigePointText.text      = FormatString.Number(pp);
            buyLootButton.interactable  = numUnlockedArtefacts < maxUnlockableArts;

            lootCostText.text = "-";

            if (numUnlockedArtefacts < maxUnlockableArts)
            {
                lootCostText.text = string.Format("{0}", FormatString.Number(Formulas.CalcNextLootCost(numUnlockedArtefacts)));
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

            ArtefactManager.Instance.UnlockArtefact(ServerCallback);
        }
    }
}