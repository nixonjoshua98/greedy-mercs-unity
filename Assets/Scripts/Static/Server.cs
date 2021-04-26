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

    const string LOCAL_IP = "109.154.20.217";
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


    // === Bounties ===
    public static void ClaimBounty(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("bounty/claim", callback, node));

    // === Bounty Shop ===
    public static void RefreshBountyShop(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("bountyshop/refresh", callback, node));
    public static void BuyBountyShopItem(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("bountyshop/buy", callback, node));

    // === Prestige Items === 
    public static void BuyLootItem(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("loot/buy", callback, node));
    public static void UpgradeLootItem(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("loot/upgrade", callback, node));


    // === Player ===
    public static void Login(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("user/login", callback, node));
    public static void ChangeUsername(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("user/changeusername", callback, node));

    // === Leaderboards ===
    public static void GetPlayerLeaderboard(Action<long, string> callback) => StartCoroutine(Get("leaderboard/player", callback));

    // # === Data === #
    public static void GetGameData(Action<long, string> callback) => StartCoroutine(Get("gamedata", callback));

    // # === Quests === #
    public static void ClaimQuestReward(Action<long, string> callback, JSONNode node) => StartCoroutine(Put("quest/claim", callback, node));

    public static void Prestige(MonoBehaviour mono, Action<long, string> callback, JSONNode node)
    {
        mono.StartCoroutine(Put("prestige", callback, node));
    }

    public static void Put(string endpoint, string purpose, JSONNode node, Action<long, string> callback)
    {
        IEnumerator _Put()
        {
            UnityWebRequest www = UnityWebRequest.Put(GetUrl(endpoint) + string.Format("?purpose={0}", purpose), Utils.Json.Compress(node));

            yield return SendRequest(www, callback);
        }

        StartCoroutine(_Put());
    }

    static IEnumerator Put(string endpoint, Action<long, string> callback, JSONNode json)
    {
        UnityWebRequest www = UnityWebRequest.Put(string.Format("http://{0}:{1}/api/{2}", IP, PORT, endpoint), Utils.Json.Compress(json));

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

        UnityEngine.Debug.Log(www.url + " | " + www.responseCode + " | " + sw.ElapsedMilliseconds + "ms");

        callback.Invoke(www.responseCode, www.downloadHandler.text);
    }
}