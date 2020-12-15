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
        // TEMP: Gets called before PlayerData can be created so we have a small delay
        // Can be removed once data is loaded in a different scene for example
        Invoke("UpdateRows", 0.05f);
    }

    void UpdateRows()
    {
        List<HeroData> allHeroData = PlayerData.heroes.GetAllHeroData();

        for (int i = 0; i < allHeroData.Count; ++i)
        {
            HeroData currentHeroData = allHeroData[i];

            if (!Rows.ContainsKey(currentHeroData.heroId))
                CreateHeroRow(currentHeroData);

            HeroRow currentHeroRow = Rows[currentHeroData.heroId];

            currentHeroRow.ButtonText.text = currentHeroData.inSquad ? "Remove" : "Add";
        }
    }

    void CreateHeroRow(HeroData currentHeroData)
    {
        GameObject spawnedRow = Instantiate(HeroRow, HeroesParent.transform);

        HeroRow row = spawnedRow.GetComponent<HeroRow>();

        row.NameText.text = Enum.GetName(typeof(HeroID), currentHeroData.heroId);

        row.Button.onClick.AddListener(delegate () { ToggleSquadHero(currentHeroData.heroId); });

        Rows[currentHeroData.heroId] = row;
    }

    void ToggleSquadHero(HeroID heroID)
    {
        SquadManager.ToggleSquadHero(heroID);

        UpdateRows();
    }
}
