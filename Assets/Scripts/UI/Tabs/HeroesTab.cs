using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HeroesTab : MonoBehaviour
{
    [SerializeField] GameObject HeroRow;

    [SerializeField] GameObject HeroesParent;

    Dictionary<HeroID, HeroRow> Rows;

    void Awake()
    {
        Rows = new Dictionary<HeroID, HeroRow>();
    }

    void OnEnable()
    {
        UpdateRows();
    }

    void UpdateRows()
    {
        for (int i = 0; i < GameState.heroes.Count; ++i)
        {
            HeroState currentHeroData = GameState.heroes[i];

            if (!Rows.ContainsKey(currentHeroData.heroId))
                CreateHeroRow(currentHeroData);

            HeroRow currentHeroRow = Rows[currentHeroData.heroId];

            currentHeroRow.SquadButtonText.text = currentHeroData.inSquad ? "Remove" : "Add";
        }
    }

    void CreateHeroRow(HeroState heroData)
    {
        GameObject spawnedRow = Instantiate(HeroRow, HeroesParent.transform);

        HeroRow row = spawnedRow.GetComponent<HeroRow>();

        // Get the HeroInfo from the actual hero object
        HeroInfo heroInfo = HeroResources.GetHeroGameObject(heroData.heroId).GetComponent<HeroInfo>();

        row.NameText.text = Enum.GetName(typeof(HeroID), heroData.heroId);

        row.SquadButton.onClick.AddListener(delegate () { ToggleSquadHero(heroData.heroId); });

        row.IconImage.sprite = heroInfo.Icon;

        Rows[heroData.heroId] = row;
    }

    void ToggleSquadHero(HeroID heroID)
    {
        SquadManager.ToggleSquadHero(heroID);

        UpdateRows();
    }
}
