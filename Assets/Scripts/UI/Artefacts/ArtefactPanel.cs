using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

using FullArtefactData = GM.Data.Artefacts.FullArtefactData;

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
            foreach (FullArtefactData art in App.Data.Arts.Artefacts)
            {
                InstantiateArtefactRow(art.ID);
            }
        }

        void InstantiateArtefactRow(int artefact)
        {
            FullArtefactData data = App.Data.Arts.GetArtefact(artefact);

            ArtefactSlot row = CanvasUtils.Instantiate<ArtefactSlot>(data.Values.Slot.gameObject, slotParent);

            row.Init(artefact, buyController);
        }

        protected override void PeriodicUpdate()
        {
            BigInteger pp = UserData.Get.Inventory.PrestigePoints;

            int numUnlockedArtefacts    = App.Data.Arts.NumUnlockedArtefacts;
            int maxUnlockableArts       = App.Data.Arts.MaxArtefacts;

            currentPointsText.text      = FormatString.Number(pp);
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
            App.Data.Arts.UnlockArtefact((success, newArtefactId) =>
            {
                if (success)
                {
                    InstantiateArtefactRow(newArtefactId);
                }
            });
        }
    }
}