using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

class SaveFile
{
    public int Gold;

    public List<HeroData> HeroList = new List<HeroData>();
}


public class PlayerData
{
    static PlayerData Instance = null;

    // # - Private Attributes - #
    int _Gold;

    Dictionary<HeroID, HeroData> _Heroes;
    // # - - - - - - - #

    // # - Public Static Attributes - #
    public static int Gold { get { return Instance._Gold; } set { Instance._Gold = value; } }
    // # - - - - - - - #

    // Private constructor
    PlayerData(SaveFile save)
    {
        _Gold = save.Gold;

        _Heroes = new Dictionary<HeroID, HeroData>();

        foreach (HeroData hero in save.HeroList)
        {
            _Heroes[hero.heroID] = hero;
        }

    }

    // Create the singleton instance
    public static void Create(string json)
    {
        if (Instance == null)
        {
            Instance = new PlayerData(JsonUtility.FromJson<SaveFile>(json));
        }
    }

    public static string ToJson()
    {
        return JsonUtility.ToJson(

            new SaveFile()
            {
                Gold = Instance._Gold,
                HeroList = Instance._Heroes.Values.ToList()
            }
        );
    }

    public static bool TryGetHero(HeroID hero, out HeroData result)
    {
        return Instance._Heroes.TryGetValue(hero, out result);
    }

    public static List<HeroData> GetHeroData()
    {
        return Instance._Heroes.Values.ToList();
    }
}