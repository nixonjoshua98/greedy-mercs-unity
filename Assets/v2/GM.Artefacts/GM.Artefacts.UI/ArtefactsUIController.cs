using GM.Artefacts.Data;
using TMPro;
using UnityEngine;
using AmountSelector = GM.UI.AmountSelector;
using BigInteger = System.Numerics.BigInteger;

namespace GM.Artefacts.UI
{
    public class ArtefactsUIController : GM.UI.Panels.Panel
    {
        [Header("Prefabs")]
        public GameObject ArtefactSlotObject;

        [Header("References")]
        public Transform ArtefactsContent;
        public AmountSelector UpgradeAmountSelector;
        public TMP_Text UnlockedArtefactsText;
        public GM.UI.VStackedButton UnlockArtefactButton;

        void Start()
        {
            InstantiateArtefactSlots();
            UpdateUnlockArtefactText();
        }

        void FixedUpdate()
        {
            UnlockedArtefactsText.text = $"<color=white>{App.Data.Arts.NumUnlockedArtefacts} / {App.Data.Arts.MaxArtefacts}</color> Artefacts unlocked";
        }

        protected override void OnHidden()
        {

        }

        protected override void OnShown()
        {

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
            UnlockArtefactButton.SetText("Unlocked", "");
            UnlockArtefactButton.interactable = !App.Data.Arts.UserUnlockedAll;

            if (!App.Data.Arts.UserUnlockedAll)
            {
                BigInteger unlockCost = App.Cache.ArtefactUnlockCost(App.Data.Arts.NumUnlockedArtefacts);

                UnlockArtefactButton.SetText("Unlock", Format.Number(unlockCost));
            }
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
    }
}
