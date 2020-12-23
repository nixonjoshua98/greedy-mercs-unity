using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

public static class Server
{
    public static void Login(MonoBehaviour mono, Action<long, string> callback)
    {
        JSONObject node = new JSONObject();

        node.Add("deviceId", SystemInfo.deviceUniqueIdentifier);

        mono.StartCoroutine(Put("login", callback, node.ToString()));
    }

    public static void GetStaticData(MonoBehaviour mono, Action<long, string> callback)
    {
        mono.StartCoroutine(Put("staticdata", callback, "{}"));
    }

    // ===

    static IEnumerator Put(string endpoint, Action<long, string> callback, string json)
    {
        string encoded = Convert.ToBase64String(Utils.GZip.Zip(json));

        UnityWebRequest www = UnityWebRequest.Put("http://31.53.80.1:2122/api/" + endpoint, encoded);

        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }
}