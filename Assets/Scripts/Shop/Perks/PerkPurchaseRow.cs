using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Perks.UI
{
    using GreedyMercs.Perks.Data;

    using GreedyMercs.Shop.UI;
    
    public class PerkPurchaseRow : MonoBehaviour
    {
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
        }

        // === Button Callback ===

        public void OnPurchaseClick()
        {
            if (!GameState.Perks.IsPerkActive(perkId))
                Utils.UI.ShowYesNoPrompt("Purchase Perk?", Purchase);

            void Purchase()
            {
                GameState.Perks.ActivatePerk(perkId);

                UpdateUI();
            }
        }
    }
}