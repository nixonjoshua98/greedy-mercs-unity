using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SquadManager : MonoBehaviour
{
    static SquadManager Instance = null;

    static Dictionary<HeroID, string> HeroIDStrings = new Dictionary<HeroID, string>()
    {
        { HeroID.WRAITH_LIGHTNING,  "WraithLightning" },
        { HeroID.GOLEM_STONE,       "GolemStone" },
        { HeroID.SATYR_FIRE,        "SatyrFire" }
    };

    [SerializeField] Transform HeroSpotsTransform;

    List<HeroFormationSpot> FormationList;

    void Awake()
    {
        Instance = this;

        CreateFormationList();
    }

    void Start()
    {
        CreateInitialSquad();
    }

    void CreateInitialSquad()
    {
        foreach (HeroState data in GameState.heroes)
        {
            if (data.inSquad)
            {
                if (!TryAddHeroToSquad(data.heroId))
                    data.inSquad = false;
            }
        }
    }

    void CreateFormationList()
    {
        FormationList = new List<HeroFormationSpot>();

        for (int i = 0; i < HeroSpotsTransform.childCount; ++i)
        {
            FormationList.Add(new HeroFormationSpot() { Parent = HeroSpotsTransform.GetChild(i) });
        }
    }

    // ======================================

    public static HeroFormationStatus ToggleSquadHero(HeroID heroID)
    {
        HeroState data = GameState.GetHeroState(heroID);

        if (data.inSquad)
        {
            if (Instance.TryRemoveHeroFromSquad(heroID))
                return HeroFormationStatus.REMOVED;

            return HeroFormationStatus.FAILED_TO_REMOVE;
        }

        else
        {
            if (Instance.TryAddHeroToSquad(heroID))
                return HeroFormationStatus.ADDED;

            return HeroFormationStatus.FAILED_TO_ADD;
        }
    }

    public static GameObject HeroIDToGameObject(HeroID hero)
    {
        if (!HeroIDStrings.TryGetValue(hero, out string str))
        {
            Debug.LogError("Error: Hero " + hero.ToString() + " was not found");
        }

        return Resources.Load<GameObject>("Heroes/" + str);
    }

    // ======================================

    bool TryAddHeroToSquad(HeroID heroID)
    {
        HeroState data = GameState.GetHeroState(heroID);

        HeroFormationSpot spot = null;

        // Find a free spot
        for (int i = 0; i < Instance.FormationList.Count; ++i)
        {
            if (Instance.FormationList[i].IsAvailable)
                spot = Instance.FormationList[i];
        }

        if (spot != null)
        {
            GameObject hero = Instantiate(HeroIDToGameObject(heroID), spot.Parent);

            hero.transform.localPosition = Vector3.zero;

            spot.Set(heroID, hero);

            data.inSquad = true;

            return true;
        }

        return false;
    }

    bool TryRemoveHeroFromSquad(HeroID heroID)
    {
        HeroState data = GameState.GetHeroState(heroID);

        foreach (HeroFormationSpot spot in Instance.FormationList)
        {
            if (!spot.IsAvailable && spot.HeroID == heroID)
            {
                spot.Free();

                data.inSquad = false;

                return true;
            }
        }

        return false;
    }
}
