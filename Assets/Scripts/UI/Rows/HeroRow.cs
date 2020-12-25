using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroRow : MonoBehaviour
{
    [SerializeField] HeroID associatedHeroId;

    public HeroID heroId {  get { return associatedHeroId; } }

    [Space]

    [SerializeField] Text DamageText;
    [SerializeField] Text BuyText;
    [SerializeField] Text CostText;
    [SerializeField] Text LevelText;

    [Space]

    [SerializeField] GameObject HeroInfoPanel;

    void UpdateRow(HeroState state)
    {
        LevelText.text          = "Level " + state.level.ToString();
        BuyText.text            = "x" + HeroesTab.BuyAmount.ToString();
        DamageText.text         = Utils.Format.FormatNumber(StatsCache.GetHeroDamage(associatedHeroId));
        CostText.text           = Utils.Format.FormatNumber(Formulas.CalcHeroLevelUpCost(associatedHeroId, HeroesTab.BuyAmount));
    }

    public bool TryUpdate()
    {
        if (GameState.TryGetHeroState(associatedHeroId, out var state))
        {
            UpdateRow(state);

            return true;
        }

        return false;
    }

    // === Button Callbacks ===

    public void OnBuyButton()
    {
        int levelsBuying = HeroesTab.BuyAmount;

        double cost = Formulas.CalcHeroLevelUpCost(associatedHeroId, levelsBuying);

        if (GameState.Player.gold >= cost)
        {
            var state = GameState.GetHeroState(associatedHeroId);

            state.level += levelsBuying;

            GameState.Player.gold -= cost;

            UpdateRow(state);
        }
    }

    public void OnInfoButton()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        GameObject panel = Instantiate(HeroInfoPanel, Vector3.zero, Quaternion.identity);

        panel.GetComponent<HeroInfoPanel>().SetHero(associatedHeroId);

        panel.transform.SetParent(canvas.transform, false);
    }
}