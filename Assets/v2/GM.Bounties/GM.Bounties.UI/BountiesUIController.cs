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
        public Transform BountySlotParent;
        public GM.UI.Layouts.ExpandableGridLayout Bountylayout;

        bool isEditing = false;

        void Awake()
        {
            SetActiveButtons(isEditing);

            foreach (var bounty in App.Data.Bounties.UserBountiesList)
            {
                Instantiate<UI_.BountySlot>(BountySlotObject, BountySlotParent)
                    .Assign(bounty.Id);
            }

            Bountylayout.UpdateCellSize();
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
