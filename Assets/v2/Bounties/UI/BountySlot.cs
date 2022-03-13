using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    [System.Serializable]
    struct EditIcon
    {
        public GameObject Object;
        public Text Text;
        public Button Button;
    }

    public class BountySlot : UI.UnlockedBountyUIObject
    {
        [Header("References")]
        public TMP_Text NameText;
        public TMP_Text IncomeText;
        public GameObject ActiveIndicator;
        public Image IconImage;

        [SerializeField] EditIcon Icon;

        protected override void OnAssigned()
        {
            Icon.Object.SetActive(false);
            UpdateUI();
        }

        public void UpdateUI()
        {
            NameText.text = AssignedBounty.Name;
            IconImage.sprite = AssignedBounty.Icon;
            IncomeText.text = AssignedBounty.Income.ToString();
            ActiveIndicator.SetActive(AssignedBounty.IsActive);
        }

        public void StopEdit()
        {
            UpdateUI();

            Icon.Object.SetActive(false);
        }

        public void StartEdit(UnityAction<BountySlot> callback)
        {
            Icon.Object.SetActive(true);
            SetSelected(AssignedBounty.IsActive);

            Icon.Button.onClick.RemoveAllListeners();
            Icon.Button.onClick.AddListener(() =>
            {
                callback.Invoke(this);
            });
        }

        public void SetSelected(bool val)
        {
            Icon.Text.text = val ? "-" : "+";
            Icon.Text.color = val ? new Color(0.75f, 0, 0) : new Color(0, 0.75f, 0.25f);
        }
    }
}
