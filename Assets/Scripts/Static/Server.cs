using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

using PersistentMono = GM.PersistentMono;


public static class Server
{
    const int PORT = 2122;

    const string LOCAL_IP = "86.140.161.85";

#if UNITY_EDITOR
    const string IP = LOCAL_IP;
#else
        const string IP = LOCAL_IP;
#endif

    static string GetUrl(string endpoint)
    {
        return string.Format("http://{0}:{1}/api/{2}", IP, PORT, endpoint);
    }

    // GET
    public static void Get(string endpoint, Action<long, JSONNode> callback)
    {
        IEnumerator Send()
        {
            yield return SendRequest(UnityWebRequest.Get(GetUrl(endpoint)), callback);
        }

        PersistentMono.Inst.StartCoroutine(Send());
    }

    // POST
    public static void Post(string endpoint, JSONNode node, Action<long, JSONNode> callback)
    {
        IEnumerator Send()
        {
            yield return SendRequest(UnityWebRequest.Post(GetUrl(endpoint), PrepareBody(node)), callback);
        }

        node.Update(DefaultJson());

        PersistentMono.Inst.StartCoroutine(Send());
    }


    public static void Post(string endpoint, Action<long, JSONNode> callback)
    {
        Post(endpoint, new JSONObject(), callback);
    }


    // Requests
    static IEnumerator SendRequest(UnityWebRequest www, Action<long, JSONNode> callback)
    {
        www.timeout = 3;

        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();

        bool _ = TryParseJSON(www.downloadHandler.text, out JSONNode resp);

        callback.Invoke(www.responseCode, resp);
    }

    static string PrepareBody(JSONNode node)
    {
        node["deviceId"] = SystemInfo.deviceUniqueIdentifier;

        return node.ToString();
    }

    // = = = Helper Methods = = = //

    public static JSONNode DefaultJson()
    {
        JSONObject node = new JSONObject();

        node["deviceId"] = SystemInfo.deviceUniqueIdentifier;

        return node;
    }

    public static bool TryParseJSON(string s, out JSONNode result)
    {
        result = new JSONObject();

        try
        {
            result = JSON.Parse(s);
        }
        catch (FormatException)
        {
            return false;
        }

        return true;
    }
}