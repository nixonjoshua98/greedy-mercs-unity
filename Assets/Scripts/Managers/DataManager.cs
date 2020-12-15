using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    void Start()
    {
        Invoke("SaveToFile", 10.0f);
    }

    void SaveToFile()
    {
        PlayerData.ToFile("playersave");

        Invoke("SaveToFile", 5.0f);
    }
}
