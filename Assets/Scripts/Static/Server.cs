using System;
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

    static IEnumerator Get(string endpoint, Action<long, string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Post("http://165.120.118.254:2122/api/" + endpoint, "NO DATA");

        yield return www.SendWebRequest();

        Debug.Log("/" + endpoint + " | " + www.responseCode);

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }
}