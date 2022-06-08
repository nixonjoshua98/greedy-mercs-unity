using GM.Artefacts.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AmountSelector = GM.UI.AmountSelector;

namespace GM.Artefacts.UI
{
    public class ArtefactsUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ArtefactSlotObject;
        public GameObject UnlockArtefactObject;

        [Header("References")]
        public Transform ArtefactsContent;
        public AmountSelector UpgradeAmountSelector;
        public TMP_Text UnlockedArtefactsText;
        [SerializeField] private TMP_Text UnlockArtefactCostText;
        [SerializeField] private Button UnlockArtefactButton;
        [SerializeField] private GameObject UnlockArtefactRow;
        private readonly Dictionary<int, ArtefactSlot> ArtefactSlots = new Dictionary<int, ArtefactSlot>();
        private BulkUpgradeController BulkUpgrades;

        private void Awake()
        {
            BulkUpgrades = new BulkUpgradeController(success: BulkUpgradeController_OnBulkUpgrade);

            StartCoroutine(_InternalUpdate());
        }

        private void Start()
        {
            UpdateArtefactSlots();
            UpdateUnlockArtefactText();

            UpdateUI();
        }

        private IEnumerator _InternalUpdate()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.5f);

                if (BulkUpgrades.RequestIsReady)
                {
                    BulkUpgrades.Process();
                }
            }
        }

        private void UpdateUI()
        {
            UnlockedArtefactsText.text = $"<color=white>{App.Artefacts.NumUnlockedArtefacts} of {App.Artefacts.MaxArtefacts}</color> Artefacts Unlocked";
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

                    slot.Setup(art.Id, UpgradeAmountSelector, ArtefactSlot_OnUpgradeButton);
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

            double unlockCost = App.GMCache.ArtefactUnlockCost(App.Artefacts.NumUnlockedArtefacts);

            UnlockArtefactButton.interactable = !App.Artefacts.UserUnlockedAll && App.Inventory.PrestigePoints >= unlockCost;

            UnlockArtefactCostText.text = Format.Number(unlockCost);
        }

        public void OnUnlockArtefactButton()
        {
            this.InstantiateUI<UnlockArtefactPopup>(UnlockArtefactObject).Init((artefact) =>
            {
                UpdateArtefactSlots();
                UpdateUnlockArtefactText();

                UpgradeAmountSelector.InvokeChangeEvent(); // Force a UI update
            });
        }

        private void BulkUpgradeController_OnBulkUpgrade(bool success)
        {
            UpgradeAmountSelector.InvokeChangeEvent();
        }

        private void ArtefactSlot_OnUpgradeButton(int artefactId, int levels)
        {
            BulkUpgrades.Add(artefactId, levels);

            UpgradeAmountSelector.InvokeChangeEvent();
        }
    }
}
