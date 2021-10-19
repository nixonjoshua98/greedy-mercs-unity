using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountiesUIController : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject BountySlotObject;

        [Header("Buttons")]
        public Button EditButton;
        public Button CancelButton;
        public Button ConfirmButton;

        [Header("References")]
        public Transform BountySlotParent;
        public GM.UI_.Layouts.ExpandableGridLayout Bountylayout;

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
    }
}
