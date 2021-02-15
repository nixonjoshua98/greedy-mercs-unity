using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.BountyShop.UI
{
    public class ArmouryChest_BS : BountyShopItemBase
    {
        [Space]

        [SerializeField] Text tierText;

        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryChestPanelObject;

        protected override void UpdateUI()
        {
            base.UpdateUI();

            tierText.text = string.Format("Tier {0} - {1}", itemData.GetInt("minTier"), itemData.GetInt("maxTier"));
        }

        public void PurchaseChest()
        {
            void Purchase()
            {
                JSONNode node = Utils.Json.GetDeviceNode();

                node.Add("itemId", (int)item);

                Server.BuyBountyShopItem(ServerCallback, node);
            }

            bool inStock = itemData.dailyPurchaseLimit > itemState.dailyPurchased;

            if (GameState.BountyShop.IsValid && inStock)
            {
                Utils.UI.ShowYesNoPrompt("Purchase Chest?", Purchase);
            }
        }

        void ServerCallback(long code, string compressed)
        {
            if (code == 200)
            {
                GameState.BountyShop.ProcessPurchase(item);

                JSONNode node = Utils.Json.Decompress(compressed);

                int itemReceived = node["itemReceived"].AsInt;

                GameState.Armoury.AddItem(itemReceived);

                Utils.UI.Instantiate(ArmouryChestPanelObject, Vector3.zero).GetComponent<ArmouryChestPanel>().Init(itemReceived);
            }

            UpdateUI();
        }
    }
}