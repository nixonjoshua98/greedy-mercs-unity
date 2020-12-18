﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroRow : MonoBehaviour
{
    [SerializeField] HeroID associatedHeroId;

    public HeroID heroId {  get { return associatedHeroId; } }

    [Space]

    [SerializeField] Text UpgradeButtonText;
    [SerializeField] Text SquadButtonText;
    [SerializeField] Text DamageText;
    [SerializeField] Text LevelText;

    [Space]

    [SerializeField] GameObject HeroInfoPanel;

    public void UpdateRow(HeroState state)
    {
        LevelText.text = "Level " + state.level.ToString();

        SquadButtonText.text = state.inSquad ? "Remove" : "Add";

        DamageText.text = Utils.Format.DoubleToString(Formulas.CalcHeroDamage(associatedHeroId));

        UpgradeButtonText.text = Utils.Format.DoubleToString(Formulas.CalcHeroLevelUpCost(associatedHeroId));
    }

    // === Button Callbacks ===
    
    public void OnSquadButton()
    {
        SquadManager.ToggleSquadHero(associatedHeroId);

        if (GameState.TryGetHeroState(associatedHeroId, out HeroState state))
        {
            UpdateRow(state);
        }
    }

    public void OnBuyButton()
    {
        double cost = Formulas.CalcHeroLevelUpCost(associatedHeroId);

        if (GameState.player.gold >= cost)
        {
            HeroState state = GameState.GetHeroState(associatedHeroId);

            state.level++;

            GameState.player.gold -= cost;

            UpdateRow(state);
        }
    }

    public void OnInfoButton()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        GameObject panel = Instantiate(HeroInfoPanel, Vector3.zero, Quaternion.identity);

        panel.transform.SetParent(canvas.transform, false);
    }
}