
using UnityEngine;
using UnityEngine.UI;

namespace BountyUI
{
    public class BountyShop : MonoBehaviour
    {
        [SerializeField] Text bountyPoints;

        void FixedUpdate()
        {
            bountyPoints.text = Utils.Format.FormatNumber(GameState.Player.bountyPoints) + " Bounty Points";
        }
    }
}