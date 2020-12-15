using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class T_LoadData : MonoBehaviour
{
    [SerializeField] bool LoadScene;

    void Awake()
    {
        PlayerData.FromFile("playersave");

        if (LoadScene)
            SceneManager.LoadScene("GameScene");
    }
}
