using System;
using System.Diagnostics;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

namespace GreedyMercs
{
    public static class Server
    {
        const int PORT = 2122;

        const string LOCAL_IP = "109.151.46.45";
        const string AWS_IP = "18.232.147.109";

#if UNITY_EDITOR
        const string IP = LOCAL_IP;
#else
        const string IP = LOCAL_IP;//AWS_IP;

#endif

        // === Bounties ===
        public static void ClaimBounty(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("bounty/claim", callback, node));

        // === Bounty Shop ===
        public static void RefreshBountyShop(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("bountyshop/refresh", callback, node));
        public static void BuyBountyShopItem(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("bountyshop/buy", callback, node));

        // === Prestige Items === 
        public static void BuyLootItem(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("loot/buy", callback, node));
        public static void UpgradeLootItem(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("loot/upgrade", callback, node));

        // === Player ===
        public static void Login(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("user/login", callback, node));
        public static void ChangeUsername(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("user/changeusername", callback, node));

        // === Leaderboards ===
        public static void GetPlayerLeaderboard(MonoBehaviour mono, Action<long, string> callback) => mono.StartCoroutine(Get("leaderboard/player", callback));

        // # === Data === #
        public static void GetGameData(MonoBehaviour mono, Action<long, string> callback) => mono.StartCoroutine(Get("gamedata", callback));


        public static void Prestige(MonoBehaviour mono, Action<long, string> callback, JSONNode node)
        {
            mono.StartCoroutine(Put("prestige", callback, node));
        }

        // ===

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
}