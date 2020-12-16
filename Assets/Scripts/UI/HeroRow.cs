using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroRow : MonoBehaviour
{
    [SerializeField] HeroID associatedHeroId;

    [Space]

    [SerializeField] Text UpgradeButtonText;
    [SerializeField] Text SquadButtonText;
    [SerializeField] Text DamageText;
    [SerializeField] Text LevelText;

    public void OnEnable()
    {
        UpdateRow();
    }

    void UpdateRow()
    {
        HeroState state = GameState.GetHeroState(associatedHeroId);

        LevelText.text = "Level " + state.level.ToString();

        SquadButtonText.text = state.inSquad ? "Remove" : "Add";
    }
    
    public void OnSquadButton()
    {
        SquadManager.ToggleSquadHero(associatedHeroId);

        UpdateRow();
    }

    public void OnBuyButton()
    {
        HeroState state = GameState.GetHeroState(associatedHeroId);

        if (GameState.player.gold >= state.level)
        {
            state.level++;

            GameState.player.gold -= state.level;

            UpdateRow();
        }
    }
}