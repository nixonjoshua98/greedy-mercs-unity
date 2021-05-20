using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

namespace Interface
{
    using GM.Inventory;

    using Utils = GreedyMercs.Utils;

    public class ArmouryPointButton : MonoBehaviour
    {
        public void OnClick()
        {
            JSONNode node = Utils.Json.GetDeviceInfo();

            Server.Put("armoury", "buyIron", node, OnServerResponse);
        }

        void OnServerResponse(long code, string data)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(data);

               InventoryManager.Instance.armouryPoints += node["pointsGained"].AsInt;
            }
        }
    }
}
