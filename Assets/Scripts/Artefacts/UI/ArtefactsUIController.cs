using GM.Artefacts.Data;
using GM.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts.UI
{
    public class ArtefactsUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject ArtefactSlotObject;
        [SerializeField] GameObject UnlockArtefactObject;

        [Header("Text References")]
        [SerializeField] TMP_Text UnlockedText;
        [SerializeField] TMP_Text UnlockCostText;

        [Header("References")]
        [SerializeField] Transform ArtefactsContent;
        [SerializeField] IntegerSelector LevelsSelector;
        [SerializeField] Button UnlockArtefactButton;
        [SerializeField] GameObject UnlockArtefactRow;

        Dictionary<int, ArtefactSlot> ArtefactSlots = new Dictionary<int, ArtefactSlot>();
        BulkUpgradeController BulkUpgrades;

        private void Awake()
        {
            BulkUpgrades = new BulkUpgradeController(success: BulkUpgradeController_OnBulkUpgrade);

            InvokeRepeating(nameof(BulkUpgradeRequestLoop), 0.0f, 1.0f);
        }

        private void Start()
        {
            UpdateArtefactSlots();
            UpdateUnlockArtefactText();

            UpdateUI();
        }

        private void BulkUpgradeRequestLoop()
        {
            if (BulkUpgrades.IsReady)
            {
                BulkUpgrades.Process();
            }
        }

        private void UpdateUI()
        {
            UnlockedText.text = $"<color=white>{App.Artefacts.NumUnlockedArtefacts} of {App.Artefacts.MaxArtefacts}</color> Artefacts Unlocked";
        }

        private void UpdateArtefactSlots()
        {
            List<AggregatedArtefactData> artefacts = App.Artefacts.UserOwnedArtefacts;

            for (int i = 0; i < artefacts.Count; ++i)
            {
                AggregatedArtefactData art = artefacts[i];

                if (!ArtefactSlots.TryGetValue(art.Id, out ArtefactSlot slot))
                {
                    slot = ArtefactSlots[art.Id] = this.Instantiate<ArtefactSlot>(ArtefactSlotObject, ArtefactsContent);

                    slot.Intialize(art.Id, LevelsSelector, ArtefactSlot_OnUpgradeButton);
                }

                slot.transform.SetSiblingIndex(i + 1);
            }
        }

        private void UpdateUnlockArtefactText()
        {
            if (App.Artefacts.UserUnlockedAll)
            {
                if (UnlockArtefactRow is not null)
                    Destroy(UnlockArtefactRow);
                return;
            }

            double unlockCost = App.Values.ArtefactUnlockCost(App.Artefacts.NumUnlockedArtefacts);

            UnlockArtefactButton.interactable = !App.Artefacts.UserUnlockedAll && App.Inventory.PrestigePoints >= unlockCost;

            UnlockCostText.text = Format.Number(unlockCost);
        }

        private void BulkUpgradeController_OnBulkUpgrade(bool success)
        {
            LevelsSelector.InvokeChange();
        }

        private void ArtefactSlot_OnUpgradeButton(int artefactId, int levels)
        {
            BulkUpgrades.Add(artefactId, levels);

            LevelsSelector.InvokeChange();
        }

        /* Callbacks */

        public void OnUnlockArtefactButton()
        {
            this.InstantiateUI<UnlockArtefactPopup>(UnlockArtefactObject).Init((artefact) =>
            {
                UpdateArtefactSlots();
                UpdateUnlockArtefactText();

                LevelsSelector.InvokeChange(); // Force a UI update
            });
        }
    }
}
