using GM.Artefacts.Data;
using GM.UI;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Artefacts.UI
{
    public class ArtefactsUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject ArtefactSlotObject;
        [SerializeField] GameObject UnlockArtefactObject;
        [SerializeField] GameObject ArtefactsPanelObject;

        [Header("References")]
        [SerializeField] Transform ArtefactsContent;
        [SerializeField] IntegerSelector LevelsSelector;

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
