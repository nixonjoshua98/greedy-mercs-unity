using UnityEngine;
using UnityEngine.UI;

using System.Numerics;
using System.Collections.Generic;


namespace GM.Artefacts
{
    using GM.UI;

    public class ArtefactPanel : PanelController
    {
        [Header("References")]
        [SerializeField] Transform slotParent;

        [Header("Components")]
        [SerializeField] Button unlockButton;

        [Space]

        [SerializeField] TMPro.TMP_Text currentPointsText;
        [SerializeField] TMPro.TMP_Text unlockCostText;
        [SerializeField] TMPro.TMP_Text unlockCountText;

        [Space]

        [SerializeField] BuyController buyController;

        void Start()
        {
            InstantiateRows();
        }

        void InstantiateRows()
        {
            foreach (ArtefactState state in UserData.Get.Artefacts.StatesList)
            {
                InstantiateArtefactRow(state.ID);
            }
        }

        void InstantiateArtefactRow(int artId)
        {
            ArtefactData artData = GameData.Get.Artefacts.Get(artId);

            ArtefactSlot row = CanvasUtils.Instantiate<ArtefactSlot>(artData.Slot.gameObject, slotParent.gameObject);

            row.Init(artId, buyController);
        }

        protected override void PeriodicUpdate()
        {
            BigInteger pp = UserData.Get.Inventory.PrestigePoints;

            int numUnlockedArtefacts    = UserData.Get.Artefacts.Count;
            int maxUnlockableArts       = GameData.Get.Artefacts.Count;

            currentPointsText.text           = FormatString.Number(pp);
            unlockButton.interactable   = numUnlockedArtefacts < maxUnlockableArts;

            unlockCostText.text = "-";
            unlockCountText.text = $"Collected {numUnlockedArtefacts}/{maxUnlockableArts}";

            if (numUnlockedArtefacts < maxUnlockableArts)
            {
                unlockCostText.text = FormatString.Number(Formulas.CalcNextLootCost(numUnlockedArtefacts));
            }
        }

        // === Button Callbacks ===

        public void OnPurchaseArtefactBtn()
        {
            UserData.Get.Artefacts.UnlockArtefact((success, newArtefactId) =>
            {
                if (success)
                {
                    InstantiateArtefactRow(newArtefactId);
                }
            });
        }
    }
}