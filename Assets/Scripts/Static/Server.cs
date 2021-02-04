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
        const string IP = AWS_IP;

#endif

        // === Bounties ===
        public static void ClaimBounty(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("bounty/claim", callback, node));

        // === Bounty Shop ===
        public static void RefreshBountyShop(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("bountyshop/refresh", callback, node));
        public static void BuyBountyShopItem(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("bountyshop/buy", callback, node));

        // === Prestige Items === 
        public static void BuyLootItem(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("loot/buy", callback, node));
        public static void UpgradeLootItem(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("loot/upgrade", callback, node));

        // === Player ===
        public static void Login(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("user/login", callback, node));
        public static void ChangeUsername(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("user/changeusername", callback, node));

        // === Leaderboards ===
        public static void GetPlayerLeaderboard(Action<long, string> callback) => PersistentMono.Inst.StartCoroutine(Get("leaderboard/player", callback));

        // # === Data === #
        public static void GetGameData(Action<long, string> callback) => PersistentMono.Inst.StartCoroutine(Get("gamedata", callback));

        // # === Armoury === #
        public static void UpgradeArmouryItem(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("armoury/upgrade", callback, node));
        public static void EvolveArmouryItem(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("armoury/upgradeevo", callback, node));

        // # === Quests === #
        public static void ClaimQuestReward(Action<long, string> callback, JSONNode node) => PersistentMono.Inst.StartCoroutine(Put("quest/claim", callback, node));



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