﻿using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

public static class Server
{
    // === Bounties ===
    public static void ClaimBounty(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("bounty/claim", callback, node));

    // === Weapons ===
    public static void BuyWeapon(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("weapon/buy", callback, node));

    // === Prestige Items === 
    public static void BuyPrestigeItem(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("prestigeitems/buy", callback, node));
    public static void UpgradePrestigeItem(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("prestigeitems/upgrade", callback, node));


    public static void Login(MonoBehaviour mono, Action<long, string> callback, JSONNode node)
    {
        mono.StartCoroutine(Put("login", callback, node));
    }

    public static void Prestige(MonoBehaviour mono, Action<long, string> callback, JSONNode node)
    {
        mono.StartCoroutine(Put("prestige", callback, node));
    }

    public static void ResetRelics(MonoBehaviour mono, Action<long, string> callback)
    {
        mono.StartCoroutine(Put("resetrelics", callback, Utils.Json.GetDeviceNode()));
    }

    public static void GetStaticData(MonoBehaviour mono, Action<long, string> callback)
    {
        mono.StartCoroutine(Put("staticdata", callback));
    }

    // ===

    static IEnumerator Put(string endpoint, Action<long, string> callback, JSONNode json)
    {
        UnityWebRequest www = UnityWebRequest.Put("http://31.53.80.1:2122/api/" + endpoint, Utils.Json.Compress(json));

        yield return SendRequest(www, callback);
    }

    static IEnumerator Put(string endpoint, Action<long, string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Put("http://31.53.80.1:2122/api/" + endpoint, Utils.Json.Compress(JSON.Parse("{}")));

        yield return SendRequest(www, callback);
    }

    static IEnumerator SendRequest(UnityWebRequest www, Action<long, string> callback)
    {
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "application/json");

        www.timeout = 3;

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }
}