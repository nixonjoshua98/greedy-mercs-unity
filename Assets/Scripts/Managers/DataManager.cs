﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class DataManager : MonoBehaviour
{
    public static string LOCAL_FILENAME = "local_31";

    public static string LOCAL_STATIC_FILENAME = "localstatic_15";

    void Start()
    {
        Invoke("WriteStateToFile", 1.0f);
    }

    void WriteStateToFile()
    {
        Utils.File.Write(LOCAL_FILENAME, GameState.ToJson());

        Invoke("WriteStateToFile", 1.0f);
    }
}
