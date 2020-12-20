using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

using RequestStructs;

namespace RequestStructs
{
    struct Post_Login
    {
        public string deviceId;
    }
}


public static class Server
{
    public static void Login(MonoBehaviour mono, Action<long, string> callback)
    {
        var obj = new Post_Login { deviceId = SystemInfo.deviceUniqueIdentifier };

        mono.StartCoroutine(Put("login", callback, JsonUtility.ToJson(obj)));
    }

    public static void GetStaticData(MonoBehaviour mono, Action<long, string> callback)
    {
        mono.StartCoroutine(Put("staticdata", callback, "{}"));
    }

    static IEnumerator Put(string endpoint, Action<long, string> callback, string json)
    {
        UnityWebRequest www = UnityWebRequest.Put("http://165.120.118.254:2122/api/" + endpoint, json);

        www.timeout = 3;

        www.SetRequestHeader("Content-Type", "application/json");

        yield return Send(www, callback);
    }

    static IEnumerator Send(UnityWebRequest www, Action<long, string> callback)
    {
        Stopwatch timer = Stopwatch.StartNew();

        yield return www.SendWebRequest();

        UnityEngine.Debug.Log(www.url + " | status " + www.responseCode.ToString() + " | " + timer.ElapsedMilliseconds.ToString() + "ms");

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }
}