
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

class SaveFile
{
    public double gold = 0;

    public List<HeroState> heroes = new List<HeroState>();
}

public class PlayerHeroes
{
    Dictionary<HeroID, HeroState> heroes = new Dictionary<HeroID, HeroState>();

    public static PlayerHeroes FromList(List<HeroState> ls)
    {
        PlayerHeroes playerHeroes = new PlayerHeroes();

        foreach (HeroState ele in ls)
        {
            playerHeroes.heroes[ele.heroId] = ele;
        }

        return playerHeroes;
    }

    public HeroState GetHeroData(HeroID hero)
    {
        return heroes[hero];
    }

    public List<HeroState> GetAllHeroData()
    {
        return heroes.Values.ToList();
    }
}


public class PlayerData
{
    static PlayerData Instance = null;

    double _gold;
    PlayerHeroes _heroes;

    public static PlayerHeroes heroes { get { return Instance._heroes; } }

    public static double gold { get { return Instance._gold; } set { Instance._gold = value; } }

    PlayerData(SaveFile save)
    {
        _gold = save.gold;

        _heroes = PlayerHeroes.FromList(save.heroes);
    }

    public static void FromJson(string json)
    {
        if (Instance == null)
            Instance = new PlayerData(JsonUtility.FromJson<SaveFile>(json));
    }
}