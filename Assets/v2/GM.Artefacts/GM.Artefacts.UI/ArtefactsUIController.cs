using GM.Artefacts.Data;
using TMPro;
using UnityEngine;
using AmountSelector = GM.UI_.AmountSelector;
using BigInteger = System.Numerics.BigInteger;
using GameObjectUtils = GM.Utils.GameObjectUtils;

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
            App.Data.Arts.UnlockArtefact((success, newArtefactId) =>
            {
                if (success)
                {
                    InstantiateSingleArtefact(App.Data.Arts.GetArtefact(newArtefactId));
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
            ArtefactSlot slotScript = GameObjectUtils.Instantiate<ArtefactSlot>(ArtefactSlotObject, ArtefactsContent);

            slotScript.AssignArtefact(data.Id, UpgradeAmountSelector);
        }



        void UpdateUnlockArtefactText()
        {
            UnlockArtefactButton.SetText("All Unlocked", "-");

            if (!App.Data.Arts.UserOwnsAllArtefacts)
            {
                BigInteger unlockCost = App.Cache.NextArtefactUnlockCost(App.Data.Arts.NumUnlockedArtefacts);

                UnlockArtefactButton.SetText("Unlock", FormatString.Number(unlockCost));
            }
        }
    }
}
