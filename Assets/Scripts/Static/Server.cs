using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

public static class Server
{
    public static void Login(MonoBehaviour mono, Action<long, string> callback, JSONNode node)
    {
        mono.StartCoroutine(Put("login", callback, node));
    }

    public static void Prestige(MonoBehaviour mono, Action<long, string> callback, JSONNode node)
    {
        mono.StartCoroutine(Put("prestige", callback, node));
    }

    public static void GetStaticData(MonoBehaviour mono, Action<long, string> callback)
    {
        mono.StartCoroutine(Put("staticdata", callback));
    }

    // ===

    static IEnumerator Put(string endpoint, Action<long, string> callback, JSONNode json)
    {
        string encoded = Convert.ToBase64String(Utils.GZip.Zip(json.ToString()));

        UnityWebRequest www = UnityWebRequest.Put("http://31.53.80.1:2122/api/" + endpoint, encoded);

        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }

    static IEnumerator Put(string endpoint, Action<long, string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Put("http://31.53.80.1:2122/api/" + endpoint, Convert.ToBase64String(Utils.GZip.Zip("{}")));

        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }
}