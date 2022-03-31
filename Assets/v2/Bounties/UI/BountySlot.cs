using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountySlot : GM.Core.GMMonoBehaviour
    {
        [Header("References")]
        public TMP_Text NameText;
        public TMP_Text IncomeText;
        public Image IconImage;

        int AssignedBountyId = -1;
        public Data.AggregatedBounty AssignedBounty { get => App.Bounties.GetUnlockedBounty(AssignedBountyId); }

        public virtual void Assign(int bountyId)
        {
            AssignedBountyId = bountyId;

            UpdateUI();
        }

        public void UpdateUI()
        {
            NameText.text = AssignedBounty.Name;
            IconImage.sprite = AssignedBounty.Icon;
            IncomeText.text = $"Produces <color=white>{Format.Number(AssignedBounty.Income)}</color>";
        }
    }
}
