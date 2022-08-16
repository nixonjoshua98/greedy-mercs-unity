using SRC.Common;
using SRC.UI;
using TMPro;
using UnityEngine;

namespace SRC.Artefacts.UI
{
    public class ArtefactUnlockSlot : Core.GMMonoBehaviour
    {
        [Header("Text References")]
        [SerializeField] TMP_Text UnlockedText;
        [SerializeField] TMP_Text UnlockCostText;
        [SerializeField] StackedButton UnlockArtefactButton;
        [Space]
        [SerializeField] GenericGradeItem Icon;

        public void Awake()
        {
            InvokeRepeating(nameof(UpdateUI), 0.0f, 0.5f);
            InvokeRepeating(nameof(UpdateIcon), 0.0f, 3.0f);
        }

        private void UpdateUI()
        {
            double unlockCost = App.Artefacts.NextUnlockCost;
            bool allUnlocked = App.Artefacts.UserUnlockedAll;

            UnlockedText.text = $"{App.Artefacts.NumUnlockedArtefacts} / {App.Artefacts.MaxArtefacts} Artefacts Collected";
            UnlockCostText.text = allUnlocked ? "Unlocked" : $"Next Artefact: <color=orange>{Format.Number(unlockCost)}</color>";
            UnlockArtefactButton.interactable = !allUnlocked && App.Inventory.PrestigePoints >= unlockCost;
            UnlockArtefactButton.BtmText.text = allUnlocked ? "" : Format.Number(unlockCost);
        }

        void UpdateIcon()
        {
            double unlockCost = App.Artefacts.NextUnlockCost;
            bool allUnlocked = App.Artefacts.UserUnlockedAll;

            Utility.Ternary(allUnlocked, Icon.Empty, () => Icon.Intialize(App.Artefacts.LockedArtefacts.Random()));
        }
    }
}
