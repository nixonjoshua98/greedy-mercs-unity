﻿using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

using RequestStructs;

namespace RequestStructs
{
    struct Login
    {
        public string deviceId;
    }

    struct StageReward
    {
        public string deviceId; 
        public int stageReached;
    }
}


public static class Server
{
    public static void GetStaticData(MonoBehaviour mono, Action<long, string> callback)
    {
        mono.StartCoroutine(Put("staticdata", callback, "{}"));
    }

    // ===

    static IEnumerator Put(string endpoint, Action<long, string> callback, string json)
    {
        UnityWebRequest www = UnityWebRequest.Put("http://31.53.80.1:2122/api/" + endpoint, json);

        www.SetRequestHeader("Content-Type", "application/json");

        yield return Send(www, callback);
    }

    static IEnumerator Send(UnityWebRequest www, Action<long, string> callback)
    {
        Stopwatch timer = Stopwatch.StartNew();

        yield return www.SendWebRequest();

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }
}