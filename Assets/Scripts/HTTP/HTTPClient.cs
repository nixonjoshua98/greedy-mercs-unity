using GM.Armoury.Requests;
using GM.Artefacts;
using GM.Artefacts.Models;
using GM.Bounties.Requests;
using GM.BountyShop.Requests;
using GM.Common;
using GM.HTTP.Requests;
using GM.PlayerStats;
using GM.Quests;
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
        void Prestige(PrestigeRequest request, Action<PrestigeResponse> callback);
        void PurchaseBountyShopCurrency(string item, Action<PurchaseCurrencyResponse> callback);
        void UnlockArtefact(Action<UnlockArtefactResponse> callback);
        void UpdateLifetimeStats(Action<UpdateLifetimeStatsResponse> action);
        void UpgradeArmouryItem(int item, Action<UpgradeArmouryItemResponse> callback);
        void UpgradeBounty(int bountyId, Action<UpgradeBountyResponse> callback);
        void FetchBountyShop(Action<GetBountyShopResponse> callback);
    }

    public class AbstractHTTPClient : MonoBehaviourLazySingleton<HTTPClient>
    {
        private HTTPServerConfig ServerConfig = new HTTPServerConfig
        {
            Port = 2122,
            Address = "81.136.113.53"
        };

        protected string Token = null;

        public bool IsOffline { get; private set; }       

        private UnityWebRequest CreateWebRequest(string method, string url, object request, bool encrypt = false)
        {
            url = ResolveURL(url);

            UnityWebRequest www = method switch
            {
                "GET" => UnityWebRequest.Get(url),
                "PUT" => UnityWebRequest.Put(url, SerializeRequest(request, encrypt)),
                _ => throw new Exception()
            };


            www.timeout = 3;

            return www;
        }

        protected void SendRequest<TResponse>(string method, string url, object request, bool encrypt, Action<TResponse> action) where TResponse : IServerResponse, new()
        {
            if (IsOffline)
            {
                TResponse resp = new TResponse() { Message = "Client is offline. Re-login to continue", StatusCode = 0 };

                action.Invoke(resp);

                return;
            }

            UnityWebRequest www = CreateWebRequest(method, url, request, encrypt);

            SetRequestHeaders(www);

            StartCoroutine(SendRequest(www, () =>
            {
                ResponseHandler(www, action);
            }));
        }

        private IEnumerator SendRequest(UnityWebRequest www, Action action)
        {
            using (www)
            {
                try
                {
                    yield return www.SendWebRequest();
                }
                finally
                {
                    action.Invoke();
                }
            }
        }

        private string ResolveURL(string endpoint)
        {
            return $"{ServerConfig.Url}/{endpoint}";
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

            if (!Serialization.TryDeserialize(in text, out T model))
            {
                model = new T() { Message = "Failed to deserialize response" };

                if (Serialization.TryDeserialize(in text, out ServerResponse resp))
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
                action.Invoke(resp);
            }
        }
    }


    public class HTTPClient : AbstractHTTPClient, IHTTPClient
    {
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
            var req = new { QuestID = questId, HighestStageReached = App.Stats.HighestStageReached };

            SendRequest("PUT", "Quests/Merc", req, encrypt: false, action);
        }

        public void CompleteDailyQuest(int questId, Action<CompleteDailyQuestResponse> action)
        {
            var req = new { QuestID = questId, LocalDailyStats = App.Stats.LocalDailyStats };

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
            var req = new { Artefacts = artefacts.Select(x => new BulkArtefactUpgrade() { ArtefactID = x.Key, Levels = x.Value }).ToList() };

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

        public void Prestige(PrestigeRequest request, Action<PrestigeResponse> callback)
        {
            SendRequest("PUT", "Prestige", request, false, callback);
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