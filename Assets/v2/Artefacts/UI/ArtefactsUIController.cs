using GM.Artefacts.Data;
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

        void Start()
        {
            InstantiateArtefactSlots();
            UpdateUnlockArtefactText();

            UpdateUI();
        }

        void UpdateUI()
        {
            UnlockedArtefactsText.text = $"<color=white>{App.Data.Artefacts.NumUnlockedArtefacts} of {App.Data.Artefacts.MaxArtefacts}</color> Artefacts unlocked";
        }


        void InstantiateArtefactSlots()
        {
            ArtefactData[] unlockArtefacts = App.Data.Artefacts.UserOwnedArtefacts;

            foreach (ArtefactData art in unlockArtefacts)
            {
                InstantiateSingleArtefact(art);
            }
        }

        void InstantiateSingleArtefact(ArtefactData data)
        {
            ArtefactSlot slotScript = Instantiate<ArtefactSlot>(ArtefactSlotObject, ArtefactsContent);

            slotScript.AssignArtefact(data.Id, UpgradeAmountSelector);
        }

        void UpdateUnlockArtefactText()
        {
            UnlockArtefactButton.SetText("Unlocked", "");
            UnlockArtefactButton.interactable = !App.Data.Artefacts.UserUnlockedAll;

            BigInteger unlockCost = App.Cache.ArtefactUnlockCost(App.Data.Artefacts.NumUnlockedArtefacts);

            if (!App.Data.Artefacts.UserUnlockedAll && App.Data.Inv.PrestigePoints >= unlockCost)
            {
                UnlockArtefactButton.SetText("Unlock", Format.Number(unlockCost));
            }
        }

        public void OnUnlockArtefactButton()
        {
            InstantiateUI<UnlockArtefactPopup>(UnlockArtefactObject).Init((artefact) =>
            {
                InstantiateSingleArtefact(artefact);
                UpdateUnlockArtefactText();
                UpdateUI();

                UpgradeAmountSelector.ReInvoke(); // Force a UI update
            });
        }
    }
}
