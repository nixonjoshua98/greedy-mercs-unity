using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

using Utils = GM.Utils;
using PersistentMono = GM.PersistentMono;


public static class Server
{
    const int PORT = 2122;

    const string LOCAL_IP = "109.158.45.6";

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

    public static void Prestige(MonoBehaviour mono, Action<long, string> callback, JSONNode node)
    {
        mono.StartCoroutine(Put("prestige", callback, node));
    }



    public static void Put(string endpoint, string purpose, JSONNode node, Action<long, string> callback)
    {
        IEnumerator _Put()
        {
            UnityWebRequest www = UnityWebRequest.Put(GetUrl(endpoint) + string.Format("?purpose={0}", purpose), PrepareBody(node, purpose));

            yield return SendRequest(www, callback);
        }

        StartCoroutine(_Put());
    }

    public static void Put(string endpoint, string purpose, Action<long, string> callback)
    { 
        Put(endpoint, purpose, new JSONObject(), callback); 
    }

    static IEnumerator Put(string endpoint, Action<long, string> callback, JSONNode json)
    {
        UnityWebRequest www = UnityWebRequest.Put(string.Format("http://{0}:{1}/api/{2}", IP, PORT, endpoint), PrepareBody(json, ""));

        yield return SendRequest(www, callback);
    }


    // GET
    public static void Get(string endpoint, Action<long, JSONNode> callback)
    {
        void Callback(long code, string body)
        {
            bool _ = TryParseJSON(body, out JSONNode resp);

            callback(code, resp);
        }

        SendGet(endpoint, Callback);
    }

    // POST
    static void Post(string endpoint, string purpose, JSONNode node, Action<long, JSONNode> callback)
    {
        IEnumerator Send()
        {
            yield return SendRequestV2(UnityWebRequest.Post(GetUrl(endpoint), PrepareBody(node, purpose)), callback);
        }

        StartCoroutine(Send());
    }

    public static void Post(string endpoint, Action<long, JSONNode> callback)
    {
        Post(endpoint, "", DefaultJson(), callback);
    }

    // PUT
    static void Put(string endpoint, string purpose, JSONNode node, Action<long, JSONNode> callback)
    {
        IEnumerator Send()
        {
            yield return SendRequestV2(UnityWebRequest.Put(GetUrl(endpoint), PrepareBody(node, purpose)), callback);
        }

        StartCoroutine(Send());
    }


    // Requests
    static void SendGet(string endpoint, Action<long, string> callback)
    {
        IEnumerator Send()
        {
            yield return SendRequest(UnityWebRequest.Get(GetUrl(endpoint)), callback);
        }

        StartCoroutine(Send());
    }

    static void SendPut(string endpoint, string purpose, JSONNode node, Action<long, string> callback)
    {
        IEnumerator Send()
        {
            yield return SendRequest(UnityWebRequest.Put(GetUrl(endpoint), PrepareBody(node, purpose)), callback);
        }

        StartCoroutine(Send());
    }

    static IEnumerator SendRequest(UnityWebRequest www, Action<long, string> callback)
    {
        www.timeout = 3;

        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }

    static IEnumerator SendRequestV2(UnityWebRequest www, Action<long, JSONNode> callback)
    {
        www.timeout = 3;

        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();

        bool _ = TryParseJSON(www.downloadHandler.text, out JSONNode resp);

        callback.Invoke(www.responseCode, resp);
    }

    static string PrepareBody(JSONNode node, string purpose)
    {
        node["purpose"]     = purpose;
        node["deviceId"]    = SystemInfo.deviceUniqueIdentifier;

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