using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroSquadManager : MonoBehaviour
{
    [SerializeField] Transform HeroSpotParent;

    [SerializeField] Transform PanelParent;
    [Space]
    [SerializeField] GameObject HeroPanel;

    void Awake()
    {
        foreach (HeroData hero in PlayerData.GetHeroData())
        {
            GameObject spawnedPanel = Instantiate(HeroPanel, Vector3.zero, Quaternion.identity);

            spawnedPanel.transform.SetParent(PanelParent);

            spawnedPanel.transform.Find("Name").GetComponent<Text>().text = Enum.GetName(typeof(HeroID), hero.heroID);
        }
    }
}
