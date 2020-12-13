using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
    static HeroManager Instance = null;

    static Dictionary<HeroID, string> HeroIDStrings = new Dictionary<HeroID, string>()
    {
        { HeroID.WRAITH_LIGHTNING,  "WraithLightning" },
        { HeroID.GOLEM_STONE,       "GolemStone" },
        { HeroID.SATYR_FIRE,        "SatyrFire" }
    };

    [SerializeField] Transform HeroSpotParent;

    void Awake()
    {
        Instance = this;
    }

    public static bool TryAddHeroToSquad(HeroID heroID)
    {
        if (Instance.GetAvailableHeroSpot(out Transform spot))
        {
            GameObject hero = Instantiate(HeroIDToGameObject(heroID), spot);

            hero.transform.localPosition = Vector3.zero;

            return true;
        }

        return false;
    }

    bool GetAvailableHeroSpot(out Transform result)
    {
        result = null;

        for (int i = 0; i < HeroSpotParent.childCount; ++i)
        {
            Transform child = HeroSpotParent.GetChild(i);

            if (child.childCount == 0)
            {
                result = child;

                return true;
            }
        }

        return false;
    }


    public static GameObject HeroIDToGameObject(HeroID hero)
    {
        if (!HeroIDStrings.TryGetValue(hero, out string str))
        {
            Debug.LogError("Hero not found!!");

            Debug.Break();
        }

        return Resources.Load<GameObject>("Heroes/" + str);
    }
}
