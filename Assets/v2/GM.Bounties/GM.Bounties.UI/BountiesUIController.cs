using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountiesUIController : Core.GMMonoBehaviour
    {
        [Header("Buttons")]
        public Button EditButton;
        public Button CancelButton;
        public Button ConfirmButton;

        bool isEditing = false;

        void Awake()
        {
            SetActiveButtons(isEditing);
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
