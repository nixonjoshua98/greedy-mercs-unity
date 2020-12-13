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
        CreateRows();
    }

    void CreateRows()
    {
        Rows = new Dictionary<HeroID, HeroRow>();

        foreach (HeroData data in PlayerData.GetAllHeroData())
        {
            GameObject spawnedRow = Instantiate(HeroRow, HeroesParent.transform);

            HeroRow row = spawnedRow.GetComponent<HeroRow>();

            row.NameText.text = Enum.GetName(typeof(HeroID), data.heroID);

            row.Button.onClick.AddListener(delegate () { ToggleSquadHero(data.heroID); });

            Rows[data.heroID] = row;
        }
    }

    void ToggleSquadHero(HeroID heroID)
    {
        switch (HeroManager.ToggleSquadHero(heroID))
        {
            case HeroFormationStatus.ADDED:
                Rows[heroID].ButtonText.text = "REMOVE";
                break;

            case HeroFormationStatus.REMOVED:
                Rows[heroID].ButtonText.text = "USE";
                break;
        }
    }
}
