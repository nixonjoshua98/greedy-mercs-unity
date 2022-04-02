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
        public TMP_Text BonusText;
        public Image IconImage;
        [Space]
        public GameObject UpgradeButton;

        int AssignedBountyId = -1;
        public Models.AggregatedBounty Bounty { get => App.Bounties.GetUnlockedBounty(AssignedBountyId); }

        public virtual void Assign(int bountyId)
        {
            AssignedBountyId = bountyId;

            UpdateUI();
        }

        public void UpdateUI()
        {
            NameText.text = Bounty.Name;
            IconImage.sprite = Bounty.Icon;

            BonusText.text = $"<color=orange>{Format.Number(Bounty.BonusValue, Bounty.BonusType)}</color> {Format.Bonus(Bounty.BonusType)}";
            IncomeText.text = Format.Number(Bounty.Income);

            UpgradeButton.SetActive(Bounty.CanLevelUp);
        }

        // UI Events //

        public void Button_LevelUpBounty()
        {
            App.Bounties.UpgradeBounty(AssignedBountyId, (success, resp) =>
            {
                UpdateUI();
            });
        }
    }
}
