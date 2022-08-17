using SRC.Bounties.Models;
using SRC.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.UI
{
    public class RarityIcon : SRC.Core.GMMonoBehaviour
    {
        [Header("Image Elements")]
        [SerializeField] private Image Background;
        [SerializeField] private Image Frame;
        [SerializeField] private Image Icon;

        public void Initialize(AggregatedBounty bounty)
        {
            Icon.sprite = bounty.Icon;

            UpdateUI(bounty.Tier);
        }

        private void UpdateUI(Rarity rarity)
        {
            var config = App.Local.GetItemGrade(rarity);

            Background.sprite = config.BackgroundSprite;
            Frame.color = config.FrameColour;
        }
    }
}
