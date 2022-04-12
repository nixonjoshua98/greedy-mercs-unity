using GM.Bounties.Models;
using GM.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountyPopup : GM.UI.PopupPanelBase
    {
        [Header("References")]
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private TMP_Text IncomeText;
        [SerializeField] private TMP_Text BonusText;
        [SerializeField] private TMP_Text DescriptionText;
        [Space]
        [SerializeField] private Slider LevelProgressSlider;
        [SerializeField] private Image IconImage;

        public void Set(AggregatedBounty bounty)
        {
            IconImage.sprite = bounty.Icon;

            NameText.text = $"<color=orange>Lv {bounty.Level}</color> {bounty.Name}";
            IncomeText.text = $"Produces <color=white>{bounty.Income}</color>";
            BonusText.text = $"<color=orange>{Format.Number(bounty.BonusValue, bounty.BonusType)}</color> {Format.Bonus(bounty.BonusType)}";
            DescriptionText.text = bounty.Description;

            LevelProgressSlider.value = !bounty.IsMaxLevel ? (bounty.NumDefeats / (float)bounty.Levels[bounty.Level].NumDefeatsRequired) : 1.0f;

            ShowInnerPanel();
        }
    }
}
