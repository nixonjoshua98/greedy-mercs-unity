﻿
using UnityEngine;


public class DataManager : MonoBehaviour
{
    public static string LOCAL_FILENAME = "localsave_01";

    public static string LOCAL_STATIC_FILENAME = "localstatic";

    // ===
    public static bool IsPaused;

    void Awake()
    {
        IsPaused = false;
    }

    void Start()
    {
        if (GameState.IsRestored())
        {
            Invoke("WriteStateToFile", 1.0f);
        }

        else
        {
            Debug.LogWarning("Game state was no restored (most likely started the wrong scene)");

            Debug.Break();
        }
    }

    void WriteStateToFile()
    {
        if (!IsPaused)
            Save();

        Invoke("WriteStateToFile", 1.0f);
    }

    public static void Save()
    {
        Utils.File.Write(LOCAL_FILENAME, GameState.ToJson().ToString());
    }
}
