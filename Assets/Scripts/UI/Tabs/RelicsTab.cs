using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class RelicsTab : MonoBehaviour
{
    [SerializeField] Text PrestigePointText;

    void OnEnable()
    {
        PrestigePointText.text = Utils.Format.DoubleToString(GameState.player.prestigePoints);
    }
}
