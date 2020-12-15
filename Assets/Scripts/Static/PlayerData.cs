using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

class SaveFile
{
    public long updateTime;

    public double gold = 0;

    public List<HeroData> heroes = new List<HeroData>();
}

[System.Serializable]
public class HeroData
{
    public HeroID heroId;

    public int level = 1;

    public bool inSquad = false;
}

public class PlayerHeroes
{
    Dictionary<HeroID, HeroData> heroes = new Dictionary<HeroID, HeroData>();

    public static PlayerHeroes FromList(List<HeroData> ls)
    {
        PlayerHeroes playerHeroes = new PlayerHeroes();

        foreach (HeroData ele in ls)
        {
            playerHeroes.heroes[ele.heroId] = ele;
        }

        return playerHeroes;
    }

    public bool TryGetHero(HeroID hero, out HeroData result)
    {
        return heroes.TryGetValue(hero, out result);
    }

    public HeroData GetHeroData(HeroID hero)
    {
        return heroes[hero];
    }

    public List<HeroData> GetAllHeroData()
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

    public static void FromFile(string filename)
    {
        using (StreamReader file = new StreamReader(Application.persistentDataPath + "/" + filename))
        {
            string json = file.ReadToEnd();

            PlayerData.FromJson(json);
        }
    }

    public static string ToFile(string filename)
    {
        string path = Application.persistentDataPath + "/" + filename;

        using (StreamWriter file = new StreamWriter(path))
        {
            string json = PlayerData.ToJson();

            file.Write(json);
        }
        
        return path;
    }

    public static string ToJson()
    {
        return JsonUtility.ToJson(

            new SaveFile()
            {
                updateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),

                gold = Instance._gold,

                heroes = heroes.GetAllHeroData()
            }
        );
    }
}