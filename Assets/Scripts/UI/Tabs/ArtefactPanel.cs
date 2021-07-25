using UnityEngine;
using UnityEngine.UI;

using System.Numerics;
using System.Collections.Generic;


namespace GM.Artefacts
{
    using GM.UI;

    using GM.Data;

    public class ArtefactPanel : CloseablePanel
    {
        [Header("References")]
        [SerializeField] Transform slotParent;

        [Header("Components")]
        [SerializeField] Button unlockButton;

        [Space]

        [SerializeField] Text currencyText;
        [SerializeField] Text unlockCostText;

        [Space]

        [SerializeField] BuyController buyController;

        [Header("Prefabs")]
        [SerializeField] GameObject artefactSlotObject;

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
                GameObject inst = CanvasUtils.Instantiate(artefactSlotObject, slotParent);

                ArtefactSlot row = inst.GetComponent<ArtefactSlot>();

                row.Init(state.ID, buyController);

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
            BigInteger pp = UserData.Get().Inventory.PrestigePoints;

            int numUnlockedArtefacts    = ArtefactManager.Instance.Count;
            int maxUnlockableArts       = GameData.Get().Artefacts.Count;

            currencyText.text           = FormatString.Number(pp);
            unlockButton.interactable   = numUnlockedArtefacts < maxUnlockableArts;

            unlockCostText.text = "-";

            if (numUnlockedArtefacts < maxUnlockableArts)
            {
                unlockCostText.text = string.Format("{0}", FormatString.Number(Formulas.CalcNextLootCost(numUnlockedArtefacts)));
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