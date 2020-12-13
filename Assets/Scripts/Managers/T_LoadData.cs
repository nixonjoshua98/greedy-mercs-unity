using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class T_LoadData : MonoBehaviour
{
    void Awake()
    {
        PlayerData.Create("{ \"Gold\":0, \"HeroList\": [{\"heroID\":10000,\"Level\":69, \"InSquad\": true}, {\"heroID\":10001,\"Level\":420}, {\"heroID\":10002,\"Level\":1}]}");

        SceneManager.LoadScene("GameScene");
    }
}
