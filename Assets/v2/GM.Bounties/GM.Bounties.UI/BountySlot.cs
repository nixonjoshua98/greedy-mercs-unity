using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI_
{
    public class BountySlot : GM.Bounties.UI.UnlockedBountyUIObject
    {
        [Header("References")]
        public TMP_Text NameText;
        public TMP_Text IncomeText;
        public GameObject ActiveIndicator;
        public Image IconImage; 

        public override void Assign(int bountyId)
        {
            base.Assign(bountyId);

            NameText.text = AssignedBounty.Name;
            IconImage.sprite = AssignedBounty.Icon;

            IncomeText.text = $"<color=white>{AssignedBounty.Income}</color> / hour";
            ActiveIndicator.SetActive(AssignedBounty.IsActive);
        }
    }
}
