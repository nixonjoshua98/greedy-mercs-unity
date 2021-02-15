using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Shop.UI
{
    public class ShopTab : MonoBehaviour
    {
        [SerializeField] Text gemsText;

        void FixedUpdate()
        {
            gemsText.text = GameState.Inventory.gems.ToString();
        }
    }

}