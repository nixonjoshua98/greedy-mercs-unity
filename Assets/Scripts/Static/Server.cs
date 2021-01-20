using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;

namespace GreedyMercs
{
    public static class Server
    {
        const string IP = "31.53.80.1"; // "18.215.151.64"; // "18.224.19.50";

        // === Bounties ===
        public static void ClaimBounty(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("bounty/claim", callback, node));

        // === Bounty Shop ===
        public static void RefreshBountyShop(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("bountyshop/refresh", callback, node));
        public static void BuyBountyShopItem(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("bountyshop/buy", callback, node));

        // === Weapons ===
        public static void BuyWeapon(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("weapon/buy", callback, node));

        // === Prestige Items === 
        public static void BuyLootItem(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("loot/buy", callback, node));
        public static void UpgradeLootItem(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("loot/upgrade", callback, node));

        // === Player ===
        public static void ChangeUsername(MonoBehaviour mono, Action<long, string> callback, JSONNode node) => mono.StartCoroutine(Put("user/changeusername", callback, node));

        // === Leaderboards ===
        public static void GetPlayerLeaderboard(MonoBehaviour mono, Action<long, string> callback) => mono.StartCoroutine(Put("leaderboard/player", callback));


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
            UnityWebRequest www = UnityWebRequest.Put(string.Format("http://{0}:2122/api/{1}", IP, endpoint), Utils.Json.Compress(json));

            yield return SendRequest(www, callback);
        }

        static IEnumerator Put(string endpoint, Action<long, string> callback)
        {
            UnityWebRequest www = UnityWebRequest.Put(string.Format("http://{0}:2122/api/{1}", IP, endpoint), Utils.Json.Compress(JSON.Parse("{}")));

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
}