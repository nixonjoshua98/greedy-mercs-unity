using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Coffee.UIEffects;

namespace GreedyMercs.Perks.UI
{
    using GreedyMercs.Perks.Data;

    using GreedyMercs.Shop.UI;
    using SimpleJSON;

    public class PerkPurchaseRow : MonoBehaviour
    { 
        const int PERK_COST = 100;

        [SerializeField] PerkID perkId;

        [Header("References")]
        [SerializeField] GemPurchaseLabel purchaseLabel;

        void OnEnable() => InvokeRepeating("UpdateUI", 0.0f, 0.5f);
        void OnDisable() => CancelInvoke("UpdateUI");

        void UpdateUI()
        {
            purchaseLabel.ToggleOverlay(GameState.Perks.IsPerkActive(perkId));

            if (GameState.Perks.IsPerkActive(perkId))
            {
                purchaseLabel.SetOverlayText(Utils.Format.FormatSeconds(GameState.Perks.PerkDurationRemaining(perkId)));
            }

            purchaseLabel.ToggleActive(!GameState.Perks.IsPerkActive(perkId) && GameState.Inventory.gems >= PERK_COST);
        }

        // === Button Callback ===

        public void OnPurchaseClick()
        {
            if (!GameState.Perks.IsPerkActive(perkId))
                Utils.UI.ShowYesNoPrompt("Purchase Perk?", Purchase);

            void Purchase()
            {
                JSONNode node = Utils.Json.GetDeviceNode();

                node["perkId"] = (int)perkId;

                Server.PurchasePerk(PurchaseCallback, node);
            }
        }

        void PurchaseCallback(long code, string compressed)
        {
            if (code == 200)
            {
                GameState.Inventory.gems -= PERK_COST;

                GameState.Perks.ActivatePerk(perkId);

                UpdateUI();
            }
            else
            {
                Utils.UI.ShowMessage("Failed Perk Purchase", string.Format("Failed to purchase perk (code: {0})", code));
            }
        }
    }
}