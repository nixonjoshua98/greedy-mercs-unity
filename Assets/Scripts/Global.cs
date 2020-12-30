using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    void Awake()
    {
        Utils.File.Delete("log.log");

        Application.logMessageReceived += Application_logMessageReceived;

        DontDestroyOnLoad(gameObject);
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        Utils.File.Append("log.log", condition + " | " + stackTrace + " | " + type.ToString());
    }
}
