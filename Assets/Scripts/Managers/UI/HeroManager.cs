using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
    static Dictionary<HeroID, string> HeroIDStrings = new Dictionary<HeroID, string>()
    {
        { HeroID.WRAITH_LIGHTNING,  "WraithLightning" },
        { HeroID.GOLEM_STONE,       "GolemStone" },
        { HeroID.SATYR_FIRE,        "SatyrFire" }
    };

    [SerializeField] Transform HeroSpotParent;

    [SerializeField] Transform PanelParent;
    [Space]
    [SerializeField] GameObject Panel;

    Dictionary<HeroID, HeroPanel> heroPanels;

    void Awake()
    {
        heroPanels = new Dictionary<HeroID, HeroPanel>();
    }

    void OnEnable()
    {
        foreach (HeroData hero in PlayerData.GetHeroData())
        {
            GameObject panel = Instantiate(Panel, Vector3.zero, Quaternion.identity);

            HeroPanel panelScript = panel.GetComponent<HeroPanel>();

            panelScript.NameText.text = Enum.GetName(typeof(HeroID), hero.heroID);

            panelScript.Button.onClick.AddListener(delegate () { OnSpawnHeroButton(hero.heroID); });

            heroPanels[hero.heroID] = panelScript;

            panel.transform.SetParent(PanelParent);
        }
    }

    void AddHeroToSquad(HeroID heroID, Transform spot)
    {
        GameObject hero = Instantiate(HeroIDToGameObject(heroID), spot);

        hero.transform.localPosition = Vector3.zero;

        HeroPanel panel = heroPanels[heroID];

        panel.gameObject.SetActive(false);
    }

    void OnSpawnHeroButton(HeroID heroID)
    {
        for (int i = 0; i < HeroSpotParent.childCount; ++i)
        {
            Transform child = HeroSpotParent.GetChild(i);

            if (child.childCount == 0)
            {
                AddHeroToSquad(heroID, child);

                break;
            }
        }
    }

    public static GameObject HeroIDToGameObject(HeroID hero)
    {
        string str;

        if (!HeroIDStrings.TryGetValue(hero, out str))
        {
            Debug.LogError("Hero not found!!");

            Debug.Break();
        }

        return Resources.Load<GameObject>("Heroes/" + str);
    }
}
