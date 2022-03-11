using GM.Artefacts.Data;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using AmountSelector = GM.UI.AmountSelector;

namespace GM.Artefacts.UI
{
    public class ArtefactsUIController : GM.UI.Panels.TogglablePanel
    {
        [Header("Prefabs")]
        public GameObject ArtefactSlotObject;
        public GameObject UnlockArtefactObject;

        [Header("References")]
        public Transform ArtefactsContent;
        public AmountSelector UpgradeAmountSelector;
        public TMP_Text UnlockedArtefactsText;
        public GM.UI.VStackedButton UnlockArtefactButton;

        Dictionary<int, ArtefactSlot> ArtefactSlots = new Dictionary<int, ArtefactSlot>();

        BulkUpgradeController BulkUpgrades;

        void Awake()
        {
            BulkUpgrades = new BulkUpgradeController(success: BulkUpgradeController_OnBulkUpgrade);

            StartCoroutine(_InternalUpdate());
        }

        void Start()
        {
            UpdateArtefactSlots();
            UpdateUnlockArtefactText();

            UpdateUI();
        }

        IEnumerator _InternalUpdate()
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

        void UpdateUI()
        {
            UnlockedArtefactsText.text = $"<color=white>{App.GMData.Artefacts.NumUnlockedArtefacts} of {App.GMData.Artefacts.MaxArtefacts}</color> Artefacts unlocked";
        }

        void UpdateArtefactSlots()
        {
            List<AggregatedArtefactData> artefacts = App.GMData.Artefacts.UserOwnedArtefacts;

            for (int i = 0; i < artefacts.Count; ++i)
            {
                AggregatedArtefactData art = artefacts[i];

                if (!ArtefactSlots.TryGetValue(art.Id, out ArtefactSlot slot))
                {
                    slot = ArtefactSlots[art.Id] = Instantiate<ArtefactSlot>(ArtefactSlotObject, ArtefactsContent);

                    slot.Setup(art.Id, UpgradeAmountSelector, ArtefactSlot_OnUpgradeButton);
                }

                slot.transform.SetSiblingIndex(i);
            }
        }


        void UpdateUnlockArtefactText()
        {
            BigInteger unlockCost = App.GMCache.ArtefactUnlockCost(App.GMData.Artefacts.NumUnlockedArtefacts);

            UnlockArtefactButton.interactable = !App.GMData.Artefacts.UserUnlockedAll && App.GMData.Inv.PrestigePoints >= unlockCost;

            if (!App.GMData.Artefacts.UserUnlockedAll)
            {
                UnlockArtefactButton.SetText("Unlock", Format.Number(unlockCost));
            }
            else
            {
                UnlockArtefactButton.SetText("Unlocked", "");
            }
        }

        public void OnUnlockArtefactButton()
        {
            InstantiateUI<UnlockArtefactPopup>(UnlockArtefactObject).Init((artefact) =>
            {
                UpdateArtefactSlots();
                UpdateUnlockArtefactText();

                UpgradeAmountSelector.ReInvoke(); // Force a UI update
            });
        }


        void BulkUpgradeController_OnBulkUpgrade(bool success)
        {
            UpgradeAmountSelector.ReInvoke();
        }

        void ArtefactSlot_OnUpgradeButton(int artefactId, int levels)
        {
            BulkUpgrades.Add(artefactId, levels);

            UpgradeAmountSelector.ReInvoke();
        }
    }
}
