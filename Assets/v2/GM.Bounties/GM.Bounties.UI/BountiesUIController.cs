using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GM.Bounties.UI
{
    public class BountiesUIController : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject BountySlotObject;

        [Header("References")]
        public Button EditButton;
        public Button CancelButton;
        public Button ConfirmButton;
        [Space]
        public TMP_Text IncomeText;
        public TMP_Text UnclaimedText;
        [Space]
        public Transform BountySlotParent;
        public GM.UI.Layouts.ExpandableGridLayout BountiesLayout;

        bool isEditing = false;

        void Awake()
        {
            SetActiveButtons(isEditing);

            foreach (var bounty in App.Data.Bounties.UnlockedBountiesList)
            {
                Instantiate<UI_.BountySlot>(BountySlotObject, BountySlotParent)
                    .Assign(bounty.Id);
            }

            BountiesLayout.UpdateCellSize();
        }

        void FixedUpdate()
        {
            IncomeText.text = $"Points per Hour: <color=white>{FormatString.Number(App.Data.Bounties.TotalHourlyIncome)}</color>";
            UnclaimedText.text = $"Max. Unclaimed Points: <color=white>{FormatString.Number(App.Data.Bounties.MaxUnclaimedCapacity)}</color>";
        }

        void SetActiveButtons(bool isEditMode)
        {
            EditButton.gameObject.SetActive(!isEditMode);
            CancelButton.gameObject.SetActive(isEditMode);
            ConfirmButton.gameObject.SetActive(isEditMode);
        }

        // == Callbacks == //
        public void OnEditButton()
        {
            SetActiveButtons(true);
        }

        public void OnCancelButton()
        {
            SetActiveButtons(false);
        }

        public void OnConfirmButton()
        {
            SetActiveButtons(false);
        }

        public void OnClaimButton()
        {
            App.Data.Bounties.ClaimPoints((success, resp) => {
            });
        }
    }
}
