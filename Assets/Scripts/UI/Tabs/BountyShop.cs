
using UnityEngine;
using UnityEngine.UI;

public class BountyShop : MonoBehaviour
{
    [SerializeField] Text bountyPoints;

    void FixedUpdate()
    {
        bountyPoints.text = GameState.Player.bountyPoints.ToString() + " Bounty Points";
    }
}
