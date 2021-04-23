using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

namespace Interface
{
    using GreedyMercs;

    using Utils = GreedyMercs.Utils;

    using GreedyMercs.BountyShop.Data;

    public class ArmouryPointButton : MonoBehaviour
    {
        Text purchaseText;

        void Awake()
        {
            purchaseText = GetComponentInChildren<Text>();
        }

        void FixedUpdate()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            //purchaseText.text = string.Format("Buy Iron\n{0}x BP", StaticData.Armoury.IronCost);
        }

        public void OnClick()
        {
            JSONNode node = Utils.Json.GetDeviceNode();

            Server.Put("armoury", "buyIron", node, OnServerResponse);
        }

        void OnServerResponse(long code, string data)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(data);

                GameState.Inventory.armouryPoints += node["pointsGained"].AsInt;
            }
        }
    }
}
