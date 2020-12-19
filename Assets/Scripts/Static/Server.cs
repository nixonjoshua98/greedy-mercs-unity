using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


public static class Server
{
    public static void Login(MonoBehaviour mono, Action<long, string> callback)
    {
        mono.StartCoroutine(Get("login", callback));
    }

    public static void GetStaticData(MonoBehaviour mono, Action<long, string> callback)
    {
        mono.StartCoroutine(Get("staticdata", callback));
    }

    static IEnumerator Get(string endpoint, Action<long, string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Post("http://165.120.118.254:2122/api/" + endpoint, "NO DATA");

        www.timeout = 3;

        Stopwatch timer = Stopwatch.StartNew();

        yield return www.SendWebRequest();

        UnityEngine.Debug.Log("/" + endpoint + " | status " + www.responseCode.ToString() + " | " + timer.ElapsedMilliseconds.ToString() + "ms");

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }
}