﻿using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class HeroUnlockPanel : MonoBehaviour
    {
        [SerializeField] Text CostText;

        void Start()
        {
            UpdatePanel();
        }

        void UpdatePanel()
        {
            if (StaticData.CharacterList.GetNextHero(out CharacterSO chara))
            {
                CostText.text = Utils.Format.FormatNumber(chara.purchaseCost);
            }

            else
                Destroy(gameObject);
        }

        // === Button Callbacks ===

        public void OnUnlockButton()
        {
            if (StaticData.CharacterList.GetNextHero(out CharacterSO chara))
            {
                if (GameState.Player.gold >= chara.purchaseCost)
                {
                    GameState.Player.gold -= chara.purchaseCost;

                    GameState.Characters.Add(chara.CharacterID);

                    Events.OnCharacterUnlocked.Invoke(chara.CharacterID);
                }

                UpdatePanel();
            }
        }
    }
}