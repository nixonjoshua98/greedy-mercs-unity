using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class DataManager : MonoBehaviour
{
    public static string LOCAL_FILENAME = "local_15";

    void Start()
    {
        // Invoke("WriteStateToFile", 3.0f);
    }

    void WriteStateToFile()
    {
        Utils.File.Write(LOCAL_FILENAME, GameState.ToJson());

        Invoke("WriteStateToFile", 3.0f);
    }
}
