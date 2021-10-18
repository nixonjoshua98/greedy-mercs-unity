using GM.Artefacts.Data;
using TMPro;
using UnityEngine;
using AmountSelector = GM.UI_.AmountSelector;
using BigInteger = System.Numerics.BigInteger;

namespace GM.Artefacts.UI
{
    public class ArtefactsUIController : GM.UI.PanelController
    {
        [Header("Prefabs")]
        public GameObject ArtefactSlotObject;

        [Header("References")]
        public Transform ArtefactsContent;
        public AmountSelector UpgradeAmountSelector;

        [Header("UI References")]
        public TMP_Text UnlockedArtefactsText;
        public UI_.VStackedButton UnlockArtefactButton;

        void Start()
        {
            InstantiateArtefactSlots();
            UpdateUnlockArtefactText();
        }

        void FixedUpdate()
        {
            UnlockedArtefactsText.text = $"Artefacts Unlocked: {App.Data.Arts.NumUnlockedArtefacts}/{App.Data.Arts.MaxArtefacts}";
        }

        // == Callbacks == //
        public void OnUnlockArtefactButton()
        {
            App.Data.Arts.UnlockArtefact((success, newArtefact) =>
            {
                if (success)
                {
                    InstantiateSingleArtefact(newArtefact);
                    UpdateUnlockArtefactText();
                }
            });
        }

        // == Instantiation == //
        void InstantiateArtefactSlots()
        {
            ArtefactData[] unlockArtefacts = App.Data.Arts.UserOwnedArtefacts;

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
            UnlockArtefactButton.SetText("All Unlocked", "-");
            UnlockArtefactButton.interactable = !App.Data.Arts.UserUnlockedAll;

            if (!App.Data.Arts.UserUnlockedAll)
            {
                BigInteger unlockCost = App.Cache.NextArtefactUnlockCost(App.Data.Arts.NumUnlockedArtefacts);

                UnlockArtefactButton.SetText("Unlock", FormatString.Number(unlockCost));
            }
        }
    }
}
