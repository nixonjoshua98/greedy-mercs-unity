using GM.Artefacts.Data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using AmountSelector = GM.UI.AmountSelector;
using BigInteger = System.Numerics.BigInteger;

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

        void Start()
        {
            UpdateArtefactSlots();
            UpdateUnlockArtefactText();

            UpdateUI();
        }

        void UpdateUI()
        {
            UnlockedArtefactsText.text = $"<color=white>{App.Data.Artefacts.NumUnlockedArtefacts} of {App.Data.Artefacts.MaxArtefacts}</color> Artefacts unlocked";
        }

        void UpdateArtefactSlots()
        {
            List<ArtefactData> artefacts = App.Data.Artefacts.UserOwnedArtefacts;

            for (int i = 0; i < artefacts.Count; ++i)
            {
                ArtefactData art = artefacts[i];

                if (!ArtefactSlots.TryGetValue(art.Id, out ArtefactSlot slot))
                {
                    slot = ArtefactSlots [art.Id] = Instantiate<ArtefactSlot>(ArtefactSlotObject, ArtefactsContent);

                    slot.AssignArtefact(art.Id, UpgradeAmountSelector);
                }

                slot.transform.SetSiblingIndex(i);
            }
        }


        void UpdateUnlockArtefactText()
        {
            BigInteger unlockCost = App.Cache.ArtefactUnlockCost(App.Data.Artefacts.NumUnlockedArtefacts);

            UnlockArtefactButton.interactable = !App.Data.Artefacts.UserUnlockedAll && App.Data.Inv.PrestigePoints >= unlockCost;

            if (!App.Data.Artefacts.UserUnlockedAll)
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
                UpdateUI();

                UpgradeAmountSelector.ReInvoke(); // Force a UI update
            });
        }
    }
}