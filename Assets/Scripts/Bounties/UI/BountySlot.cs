using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountySlot : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject InfoPopupObject;

        [Header("References")]
        public TMP_Text NameText;
        public TMP_Text IncomeText;
        public TMP_Text BonusText;
        public Image IconImage;
        public GameObject Pillbadge;

        private int _bountyId = -1;
        public Models.AggregatedBounty _bounty => App.Bounties.GetBounty(_bountyId);

        public virtual void Assign(int bountyId)
        {
            _bountyId = bountyId;

            UpdateUI();
        }

        public void UpdateUI()
        {
            NameText.text = _bounty.Name;
            IconImage.sprite = _bounty.Icon;

            BonusText.text = $"<color=orange>{Format.Number(_bounty.BonusValue, _bounty.BonusType)}</color> {Format.Bonus(_bounty.BonusType)}";
            IncomeText.text = Format.Number(_bounty.Income);
        }

        void FixedUpdate()
        {
            Pillbadge.SetActive(_bounty.CanUpgrade);
        }

        // UI Events //

        public void Button_ShowInfoPopup()
        {
            InstantiateUI<BountyPopup>(InfoPopupObject).Set(_bounty);
        }
    }
}
