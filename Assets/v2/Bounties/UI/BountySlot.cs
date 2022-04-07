using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountySlot : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject InfoPopupObject;

        [Header("References")]
        public TMP_Text NameText;
        public TMP_Text IncomeText;
        public TMP_Text BonusText;
        public Image IconImage;
        [Space]
        public GameObject UpgradeButton;
        private int _bountyId = -1;
        public Models.AggregatedBounty Bounty => App.Bounties.GetUnlockedBounty(_bountyId);

        public virtual void Assign(int bountyId)
        {
            _bountyId = bountyId;

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

        public void Button_ShowInfoPopup()
        {
            InstantiateUI<BountyPopup>(InfoPopupObject).Set(Bounty);
        }

        public void Button_LevelUpBounty()
        {
            App.Bounties.UpgradeBounty(_bountyId, (success, resp) =>
            {
                UpdateUI();

                if (!success)
                {
                    Modals.ShowServerError(resp);
                }
            });
        }
    }
}
