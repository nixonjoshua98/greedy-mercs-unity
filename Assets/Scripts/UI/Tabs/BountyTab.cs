using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BountyTab : MonoBehaviour
{
    [SerializeField] Text BountyPointText;

    void OnEnable()
    {
        BountyPointText.text = GameState.Player.bountyPoints.ToString();
    }
}
