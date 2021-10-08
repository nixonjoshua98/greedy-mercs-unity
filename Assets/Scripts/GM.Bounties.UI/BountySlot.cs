using GM.Bounties.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountySlot : Core.GMMonoBehaviour
    {
        [Header("Components")]
        public Animator Animator;

        [SerializeField] TMPro.TMP_Text pointsText;
        [SerializeField] TMPro.TMP_Text bountyName;

        [SerializeField] Image icon;

        [HideInInspector] public UnityEngine.Events.UnityAction<int> E_OnClick;
        [HideInInspector] public int BountyID;

        public void SetBounty(int bounty)
        {
            BountyID = bounty;

            Bounties.Models.BountyGameData data = App.Data.Bounties.Game[BountyID];

            bountyName.text = data.Name.ToUpper();
            pointsText.text = string.Format("{0}", data.HourlyIncome);

            icon.sprite = data.Icon;
        }
        

        // Button Callback
        public void OnClick()
        {
            E_OnClick?.Invoke(BountyID);
        }
    }
}