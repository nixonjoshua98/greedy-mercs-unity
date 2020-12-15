using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class T_LoadData : MonoBehaviour
{
    [SerializeField] bool LoadScene;

    void Awake()
    {
        string json = "{\"updateTime\":1608036362,\"gold\":2090291.2000000007,\"heroes\":[{\"heroId\":10000,\"level\":1,\"inSquad\":true},{\"heroId\":10001,\"level\":1,\"inSquad\":true},{\"heroId\":10002,\"level\":1,\"inSquad\":false}]}";

        PlayerData.FromJson(json);

        if (LoadScene)
            SceneManager.LoadScene("GameScene");
    }
}
