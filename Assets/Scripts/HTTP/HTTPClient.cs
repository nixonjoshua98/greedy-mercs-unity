using GM.Armoury.Requests;
using GM.Artefacts.Requests;
using GM.Bounties.Requests;
using GM.BountyShop.Requests;
using GM.Common;
using GM.HTTP.Models;
using GM.HTTP.Requests;
using GM.Quests;
using GM.UserStats;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace GM.HTTP
{
    public interface IHTTPClient
    {
        bool IsOffline { get; }

        void BulkUpgradeArtefacts(Dictionary<int, int> artefacts, Action<BulkArtefactUpgradeResponse> callback);
        void PurchaseBountyShopArmouryItem(string item, Action<PurchaseArmouryItemResponse> callback);
        void ClaimBounties(Action<BountyClaimResponse> callback);
        void CompleteDailyQuest(int questId, Action<CompleteDailyQuestResponse> action);
        void CompleteMercQuest(int questId, Action<CompleteMercQuestResponse> action);
        void FetchQuests(Action<QuestsDataResponse> action);
        void FetchStaticData(Action<FetchGameDataResponse> callback);
        void DeviceLogin(Action<LoginResponse> callback);
        void Prestige(Action<PrestigeResponse> callback);
        void PurchaseBountyShopCurrency(string item, Action<PurchaseCurrencyResponse> callback);
        void UnlockArtefact(Action<UnlockArtefactResponse> callback);
        void UpdateLifetimeStats(Action<UpdateLifetimeStatsResponse> action);
        void UpgradeArmouryItem(int item, Action<UpgradeArmouryItemResponse> callback);
        void UpgradeBounty(int bountyId, Action<UpgradeBountyResponse> callback);
        void FetchBountyShop(Action<GetBountyShopResponse> callback);
        void ToggleActiveBounty(int bountyId, bool isActive, Action<ServerResponse> callback);
    }

    public abstract class AbstractHTTPClient : MonoBehaviourLazySingleton<HTTPClient>
    {
        protected abstract HTTPServerConfig Server { get; }

        protected string Token = null;

        public bool IsOffline { get; private set; }

        protected void SendRequest<TResponse>(string method, string url, object request, bool encrypt, Action<TResponse> action) where TResponse : IServerResponse, new()
        {
            if (IsOffline)
            {
                TResponse resp = new TResponse() { Message = "Client is offline. Re-login to continue", StatusCode = 0 };

                action.Invoke(resp);

                return;
            }

            UnityWebRequest www = CreateWebRequest(method, url, request, encrypt);

            StartCoroutine(SendRequest(CreateWebRequest(method, url, request), action));
        }

        private UnityWebRequest CreateWebRequest(string method, string url, object request, bool encrypt = false)
        {
            url = ResolveURL(url);

            UnityWebRequest www = method switch
            {
                "GET" => UnityWebRequest.Get(url),
                "PUT" => UnityWebRequest.Put(url, SerializeRequest(request, encrypt)),
                _ => throw new Exception()
            };

            SetRequestHeaders(www);

            www.timeout = 3;

            return www;
        }

        private IEnumerator SendRequest<TResponse>(UnityWebRequest www, Action<TResponse> action) where TResponse : IServerResponse, new()
        {
            using (www)
            {
                try
                {
                    yield return www.SendWebRequest();
                }
                finally
                {
                    ResponseHandler(www, action);
                }
            }
        }

        private string ResolveURL(string endpoint)
        {
            return $"{Server.BaseURL}/{endpoint}";
        }

        private void SetRequestHeaders(UnityWebRequest www)
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("DeviceID", SystemInfo.deviceUniqueIdentifier);

            if (Token is not null)
            {
                www.SetRequestHeader("Authorization", $"Bearer {Token}");
            }
        }

        private T DeserializeResponse<T>(UnityWebRequest www, bool encrpted) where T : IServerResponse, new()
        {
            string text = www.downloadHandler.text;

            if (encrpted)
                text = AES.Decrypt(text);

            if (!Serialization.TryDeserialize(text, out T model))
            {
                model = new T() { Message = "Failed to deserialize response" };

                if (Serialization.TryDeserialize(text, out ServerResponse resp))
                {
                    model.Message = resp.Message;
                }
            }

            model.StatusCode = www.responseCode;

            return model;
        }

        private string SerializeRequest(object request, bool encrypt = false)
        {
            string str = JsonConvert.SerializeObject(request);

            if (encrypt)
                str = AES.Encrypt(str);

            return str;
        }

        private void ResponseHandler<TResponse>(UnityWebRequest www, Action<TResponse> action) where TResponse : IServerResponse, new()
        {
            IsOffline = IsOffline || www.responseCode == 0;

            bool isEncrypted = www.GetBoolResponseHeader(Constants.Headers.ResponseEncrypted, false);

            TResponse resp = DeserializeResponse<TResponse>(www, isEncrypted);

            if (www.responseCode == HTTPCodes.Unauthorised)
            {
                if (www.GetBoolResponseHeader(Constants.Headers.InvalidToken, false))
                {
                    Debug.LogError("Session has been invalidated and you should re-login");
                }
                else
                {
                    Debug.LogError("Session has expired and you should re-login");
                }
            }
            else
            {
                if (www.responseCode != 200)
                    GMLogger.Error($"{www.url} | {resp.Message} | {www.responseCode}");

                action.Invoke(resp);
            }
        }
    }


    public class HTTPClient : AbstractHTTPClient, IHTTPClient
    {
        protected override HTTPServerConfig Server => new($"http://86.175.180.154:2122/api");

        public void UpdateLifetimeStats(Action<UpdateLifetimeStatsResponse> action)
        {
            var req = new { Changes = App.Stats.LocalLifetimeStats };

            SendRequest("PUT", "User/LifetimeStats", req, false, action);
        }

        public void FetchQuests(Action<QuestsDataResponse> action)
        {
            SendRequest("GET", "Quests", null, encrypt: false, action);
        }

        public void CompleteMercQuest(int questId, Action<CompleteMercQuestResponse> action)
        {
            var req = new { QuestID = questId, GameState = LocalGameStateRequestModel.Create(App) };

            SendRequest("PUT", "Quests/Merc", req, encrypt: true, action);
        }

        public void CompleteDailyQuest(int questId, Action<CompleteDailyQuestResponse> action)
        {
            var req = new { QuestID = questId, App.Stats.LocalDailyStats };

            SendRequest("PUT", "Quests/Daily", req, encrypt: false, action);
        }

        public void FetchStaticData(Action<FetchGameDataResponse> callback)
        {
            SendRequest("GET", "DataFile", null, false, callback);
        }

        public void UnlockArtefact(Action<UnlockArtefactResponse> callback)
        {
            SendRequest("GET", "Artefacts/Unlock", null, false, callback);
        }

        public void BulkUpgradeArtefacts(Dictionary<int, int> artefacts, Action<BulkArtefactUpgradeResponse> callback)
        {
            var req = new { Artefacts = artefacts.Select(x => new { ArtefactID = x.Key, Levels = x.Value }).ToList() };

            SendRequest("PUT", "Artefacts/BulkUpgrade", req, false, callback);
        }

        public void UpgradeArmouryItem(int item, Action<UpgradeArmouryItemResponse> callback)
        {
            var req = new { ItemID = item };

            SendRequest("PUT", "Armoury/Upgrade", req, false, callback);
        }

        public void ClaimBounties(Action<BountyClaimResponse> callback)
        {
            SendRequest("GET", "Bounties/Claim", null, false, callback);
        }

        public void UpgradeBounty(int bountyId, Action<UpgradeBountyResponse> callback)
        {
            var req = new { BountyID = bountyId };

            SendRequest("PUT", "Bounties/Upgrade", req, false, callback);
        }

        public void ToggleActiveBounty(int bountyId, bool isActive, Action<ServerResponse> callback)
        {
            var req = new { BountyID = bountyId, IsActive = isActive };

            SendRequest("PUT", "Bounties/Toggle", req, encrypt: true, callback);
        }

        public void DeviceLogin(Action<LoginResponse> callback)
        {
            var req = new { DeviceId = SystemInfo.deviceUniqueIdentifier };

            SendRequest<LoginResponse>("GET", "Login/Device", req, false, (resp) =>
            {
                Token = null;

                if (resp.StatusCode == HTTPCodes.Success)
                {
                    Token = resp.Token;
                }

                callback.Invoke(resp);
            });
        }

        public void Prestige(Action<PrestigeResponse> callback)
        {
            var req = new
            {
                LocalState = App.LocalStateFile
            };

            SendRequest("PUT", "Prestige", req, encrypt: true, callback);
        }

        public void PurchaseBountyShopArmouryItem(string item, Action<PurchaseArmouryItemResponse> callback)
        {
            var req = new { ItemID = item };

            SendRequest("PUT", "BountyShop/Purchase/ArmouryItem", req, false, callback);
        }

        public void FetchBountyShop(Action<GetBountyShopResponse> callback)
        {
            SendRequest("GET", "BountyShop", null, false, callback);
        }

        public void PurchaseBountyShopCurrency(string item, Action<PurchaseCurrencyResponse> callback)
        {
            var req = new { ItemID = item };

            SendRequest("PUT", "BountyShop/Purchase/Currency", req, false, callback);
        }
    }
}