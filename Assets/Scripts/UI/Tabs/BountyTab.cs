using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BountyTab : MonoBehaviour
{
    [SerializeField] Text BountyPointText;

    void OnEnable()
    {
        InvokeRepeating("UpdateTab", 0.5f, 0.5f);
    }

    void OnDisable()
    {
        CancelInvoke("UpdateTab");
    }

    void UpdateTab()
    {
        BountyPointText.text = GameState.Player.bountyPoints.ToString();
    }
}
