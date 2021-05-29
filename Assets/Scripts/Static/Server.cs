using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

using Utils = GreedyMercs.Utils;
using PersistentMono = GreedyMercs.PersistentMono;


public static class Server
{
    const int PORT = 2122;

    const string LOCAL_IP = "86.140.92.56";
    const string AWS_IP = "18.232.147.109";

#if UNITY_EDITOR
    const string IP = LOCAL_IP;
#else
        const string IP = LOCAL_IP;
#endif

    static string GetUrl(string endpoint)
    {
        return string.Format("http://{0}:{1}/api/{2}", IP, PORT, endpoint);
    }

    static void StartCoroutine(IEnumerator f) => PersistentMono.Inst.StartCoroutine(f);

    // === Player ===
    public static void Login(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("user/login", callback, node));
    public static void ChangeUsername(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("user/changeusername", callback, node));

    // === Leaderboards ===
    public static void GetPlayerLeaderboard(Action<long, string> callback) => StartCoroutine(Get("leaderboard/player", callback));

    // # === Data === #
    public static void GetGameData(Action<long, string> callback) => StartCoroutine(Get("gamedata", callback));

    public static void Prestige(MonoBehaviour mono, Action<long, string> callback, JSONNode node)
    {
        mono.StartCoroutine(Put("prestige", callback, node));
    }

    public static void Put(string endpoint, string purpose, JSONNode node, Action<long, string> callback)
    {
        IEnumerator _Put()
        {
            string body = EncryptAndPrepareJSON(node, purpose);

            UnityWebRequest www = UnityWebRequest.Put(GetUrl(endpoint) + string.Format("?purpose={0}", purpose), body);

            yield return SendRequest(www, callback);
        }

        StartCoroutine(_Put());
    }

    public static void Put(string endpoint, string purpose, Action<long, string> callback) { Put(endpoint, purpose, new JSONObject(), callback); }

    static IEnumerator Put(string endpoint, Action<long, string> callback, JSONNode json)
    {
        UnityWebRequest www = UnityWebRequest.Put(string.Format("http://{0}:{1}/api/{2}", IP, PORT, endpoint), EncryptAndPrepareJSON(json, ""));

        yield return SendRequest(www, callback);
    }

    static IEnumerator Get(string endpoint, Action<long, string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(string.Format("http://{0}:{1}/api/{2}", IP, PORT, endpoint));

        yield return SendRequest(www, callback);
    }

    static IEnumerator SendRequest(UnityWebRequest www, Action<long, string> callback)
    {
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "application/json");

        www.timeout = 3;

        Stopwatch sw = Stopwatch.StartNew();

        yield return www.SendWebRequest();

        if (www.isHttpError)
            UnityEngine.Debug.Log(www.url + " | " + www.responseCode + " | " + sw.ElapsedMilliseconds + "ms");

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }

    static string EncryptAndPrepareJSON(JSONNode node, string purpose)
    {
        node["purpose"]     = purpose;
        node["deviceId"]    = SystemInfo.deviceUniqueIdentifier;

        return Funcs.EncryptServerJSON(node);
    }
}