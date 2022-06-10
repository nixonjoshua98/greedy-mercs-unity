using GM.Bounties.Models;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class ActiveBountySlot : MonoBehaviour
    {
        public int BountyID { get; private set; } = -1;

        [Header("Prefabs")]
        [SerializeField] GameObject PopupObject;

        [Header("Default Values")]
        [SerializeField] Sprite DefaultBackgroundImage;

        [Header("UI Components")]
        [SerializeField] Image BackgroundImage;
        [SerializeField] Image IconImage;

        public void Initialize(AggregatedBounty bounty)
        {
            BountyID = bounty.ID;

            IconImage.sprite = bounty.Icon;
            IconImage.color = Color.white;
        }

        public void Reset()
        {
            BountyID = -1;
            IconImage.color = new Color();
        }

        void InstantiateBountyPopup()
        {
            var popup = this.InstantiateUI<BountyPopup>(PopupObject);

            popup.Initialize(BountyID);
        }

        /* UI Events */

        public void OnSlotClicked()
        {
            if (BountyID >= 0)
            {
                InstantiateBountyPopup();
            }
        }
    }
}
