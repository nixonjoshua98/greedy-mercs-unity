﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class DataManager : MonoBehaviour
{
    public static string LOCAL_FILENAME = "local_44";

    public static string LOCAL_STATIC_FILENAME = "localstatic_17";

    void Start()
    {
        if (GameState.IsRestored())
            Invoke("WriteStateToFile", 1.0f);

        else
        {
            Debug.LogWarning("Game state was no restored (most likely started the wrong scene)");

            Debug.Break();
        }
    }

    void WriteStateToFile()
    {
        Utils.File.Write(LOCAL_FILENAME, GameState.ToJson());

        Invoke("WriteStateToFile", 1.0f);
    }
}
