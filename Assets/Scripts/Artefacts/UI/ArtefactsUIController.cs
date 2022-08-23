using SRC.Artefacts.Data;
using SRC.UI;
using System.Collections.Generic;
using UnityEngine;

namespace SRC.Artefacts.UI
{
    public class ArtefactsUIController : SRC.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject ArtefactSlotObject;
        [SerializeField] private GameObject UnlockArtefactObject;
        [SerializeField] private GameObject ArtefactsPanelObject;

        [Header("References")]
        [SerializeField] private Transform ArtefactsContent;
        [SerializeField] private IntegerSelector LevelsSelector;
        private readonly Dictionary<int, ArtefactSlot> ArtefactSlots = new Dictionary<int, ArtefactSlot>();
        private BulkUpgradeController BulkUpgrades;

        private void Awake()
        {
            BulkUpgrades = new BulkUpgradeController(success: BulkUpgradeController_OnBulkUpgrade);

            InvokeRepeating(nameof(BulkUpgradeRequestLoop), 0.0f, 1.0f);
        }

        private void Start()
        {
            UpdateArtefactSlots();
        }

        private void BulkUpgradeRequestLoop()
        {
            if (BulkUpgrades.IsReady)
            {
                BulkUpgrades.Process();
            }
        }

        private void UpdateArtefactSlots()
        {
            List<AggregatedArtefactData> artefacts = App.Artefacts.UnlockedArtefacts;

            for (int i = 0; i < artefacts.Count; ++i)
            {
                AggregatedArtefactData art = artefacts[i];

                if (!ArtefactSlots.TryGetValue(art.ArtefactID, out ArtefactSlot slot))
                {
                    slot = ArtefactSlots[art.ArtefactID] = this.Instantiate<ArtefactSlot>(ArtefactSlotObject, ArtefactsContent);

                    slot.Intialize(art.ArtefactID, LevelsSelector, ArtefactSlot_OnUpgradeButton);
                }

                slot.transform.SetSiblingIndex(i + 1);
            }
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

        public void ShowArtefactsPreviewPanel()
        {
            this.InstantiateUI<ArtefactsPreviewPanel>(ArtefactsPanelObject);
        }

        public void UnlockArtefact()
        {
            this.InstantiateUI<UnlockArtefactPopup>(UnlockArtefactObject).Init((artefact) =>
            {
                UpdateArtefactSlots();

                LevelsSelector.InvokeChange(); // Force a UI update
            });
        }
    }
}
