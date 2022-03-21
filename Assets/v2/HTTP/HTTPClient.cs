using GM.Artefacts.Models;
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
        void BulkUpgradeArtefacts(Dictionary<int, int> artefacts, Action<BulkUpgradeResponse> callback);
        void BuyBountyShopArmouryItem(string item, Action<Requests.BountyShop.PurchaseArmouryItemResponse> callback);
        void ClaimBounties(Action<BountyClaimResponse> callback);
        void CompleteDailyQuest(int questId, Action<CompleteDailyQuestResponse> action);
        void CompleteMercQuest(int questId, Action<CompleteMercQuestResponse> action);
        void FetchQuests(Action<QuestsDataResponse> action);
        void FetchStaticData(Action<FetchGameDataResponse> callback);
        void FetchStats(Action<UserStatsResponse> action);
        void Login(Action<LoginResponse> callback);
        void Prestige(PrestigeRequest request, Action<PrestigeResponse> callback);
        void PurchaseBountyShopCurrencyType(string item, Action<Requests.BountyShop.PurchaseCurrencyResponse> callback);
        void SetActiveBounties(List<int> bounties, Action<UpdateActiveBountiesResponse> callback);
        void UnlockArtefact(Action<UnlockArtefactResponse> callback);
        void UpgradeArmouryItem(int item, Action<UpgradeArmouryItemResponse> callback);
    }

    public class HTTPClient : Common.MonoBehaviourLazySingleton<HTTPClient>, IHTTPClient
    {
        HTTPServerConfig ServerConfig = new HTTPServerConfig
        {
            Port = 2122,
            Address = "109.154.100.101"
        };

        string Authentication = null;

        public void FetchStats(Action<UserStatsResponse> action)
        {
            SendRequest("GET", "stats", ServerRequest.Empty, encrypt: false, action);
        }

        public void FetchQuests(Action<GM.Quests.QuestsDataResponse> action)
        {
            SendRequest("GET", "quests", ServerRequest.Empty, encrypt: false, action);
        }

        public void CompleteMercQuest(int questId, Action<CompleteMercQuestResponse> action)
        {
            var req = new CompleteMercQuestRequest() { QuestID = questId, HighestStageReached = App.Stats.HighestStageReached };

            SendRequest("POST", "quests/merc", req, encrypt: false, action);
        }

        public void CompleteDailyQuest(int questId, Action<CompleteDailyQuestResponse> action)
        {
            var req = new CompleteDailyQuestRequest() { QuestID = questId, LocalDailyStats = App.Stats.LocalDailyStats };

            SendRequest("POST", "quests/daily", req, encrypt: false, action);
        }

        public void FetchStaticData(Action<FetchGameDataResponse> callback)
        {
            SendRequest("GET", "static", ServerRequest.Empty, false, callback);
        }

        public void UnlockArtefact(Action<UnlockArtefactResponse> callback)
        {
            SendRequest("GET", "artefact/unlock", ServerRequest.Empty, false, callback);
        }

        public void BulkUpgradeArtefacts(Dictionary<int, int> artefacts, Action<BulkUpgradeResponse> callback)
        {
            var req = new BulkUpgradeRequest() { Artefacts = artefacts.Select(x => new BulkArtefactUpgrade(x.Key, x.Value)).ToList() };

            SendRequest("POST", "artefact/bulk-upgrade", req, false, callback);
        }

        public void UpgradeArmouryItem(int item, Action<UpgradeArmouryItemResponse> callback)
        {
            var req = new UpgradeArmouryItemRequest(item);

            SendRequest("POST", "armoury/upgrade", req, false, callback);
        }

        public void ClaimBounties(Action<BountyClaimResponse> callback)
        {
            SendRequest("GET", "bounty/claim", ServerRequest.Empty, false, callback);
        }

        public void SetActiveBounties(List<int> bounties, Action<UpdateActiveBountiesResponse> callback)
        {
            var req = new UpdateActiveBountiesRequest(bounties);

            SendRequest("POST", "bounty/setactive", req, false, callback);
        }

        public void Login(Action<LoginResponse> callback)
        {
            var req = new LoginRequest(SystemInfo.deviceUniqueIdentifier);
            var www = UnityWebRequest.Post(ResolveURL("login"), SerializeRequest(req));

            SendRequest<LoginRequest, LoginResponse>("POST", "login", req, false, (resp) =>
            {
                Authentication = null;

                if (resp.StatusCode == HTTPCodes.Success)
                {
                    Authentication = resp.Token;
                }

                callback.Invoke(resp);
            });
        }

        public void Prestige(PrestigeRequest request, Action<PrestigeResponse> callback)
        {
            SendRequest("POST", "prestige", request, false, callback);
        }

        public void BuyBountyShopArmouryItem(string item, Action<Requests.BountyShop.PurchaseArmouryItemResponse> callback)
        {
            var req = new Requests.BountyShop.PurchaseBountyShopItem(item);

            SendRequest("POST", "bountyshop/purchase/armouryitem", req, false, callback);
        }

        public void PurchaseBountyShopCurrencyType(string item, Action<Requests.BountyShop.PurchaseCurrencyResponse> callback)
        {
            var req = new Requests.BountyShop.PurchaseBountyShopItem(item);

            SendRequest("POST", "bountyshop/purchase/currency", req, false, callback);
        }

        UnityWebRequest CreateWebRequest<TRequest>(string method, string url, TRequest request, bool encrypt = false) where TRequest : IServerRequest
        {
            url = ResolveURL(url);

            UnityWebRequest www = method switch
            {
                "GET" => UnityWebRequest.Get(url),
                "POST" => UnityWebRequest.Post(url, SerializeRequest(request, encrypt)),
                _ => throw new Exception()
            };

            return www;
        }

        void SendRequest<TRequest, TResponse>(string method, string url, TRequest request, bool encrypt, Action<TResponse> action) where TRequest : IServerRequest where TResponse : IServerResponse, new()
        {
            UnityWebRequest www = CreateWebRequest(method, url, request, encrypt: encrypt);

            SetRequestHeaders(www);

            StartCoroutine(SendRequest(www, () => ResponseHandler(www, action)));
        }

        IEnumerator SendRequest(UnityWebRequest www, Action action)
        {
            using (www)
            {
                yield return www.SendWebRequest();

                action.Invoke();
            }
        }

        string ResolveURL(string endpoint) => $"{ServerConfig.Url}/{endpoint}";

        void SetRequestHeaders(UnityWebRequest www)
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("DeviceId", SystemInfo.deviceUniqueIdentifier);

            if (Authentication is not null)
            {
                www.SetRequestHeader("Authentication", Authentication);
            }
        }

        T DeserializeResponse<T>(UnityWebRequest www, bool encrpted) where T : IServerResponse, new()
        {
            string text = www.downloadHandler.text;

            T model;

            try
            {
                if (encrpted)
                    text = AES.Decrypt(text);

                model = JsonConvert.DeserializeObject<T>(text);

                if (model == null)
                {
                    GMLogger.WhenNull(model, "Failed to deserialize server response");

                    model = new T() { ErrorMessage = "Failed to deserialize server response" };
                }

            }
            catch (Exception e)
            {
                GMLogger.Exception("Failed to parse response", e);

                model = new T() { ErrorMessage = e.Message };
            }

            model.StatusCode = www.responseCode;

            return model;
        }

        string SerializeRequest<T>(T request, bool encrypt = false) where T : IServerRequest
        {
            return JsonConvert.SerializeObject(request);
        }


        void ResponseHandler<TResponse>(UnityWebRequest www, Action<TResponse> action) where TResponse : IServerResponse, new()
        {
            bool isEncrypted = www.GetBoolResponseHeader("Response-Encrypted", false);

            switch (www.responseCode)
            {
                case HTTPCodes.InvalidiateClient:
                    Response_InvalidateClient();
                    break;

                case HTTPCodes.Unauthorized:
                    Response_Unauthorized();
                    break;
            }

            TResponse resp = DeserializeResponse<TResponse>(www, isEncrypted);

            action.Invoke(resp);
        }

        // = Special Response Callbacks = //
        void Response_Unauthorized()
        {
            Authentication = null;
        }

        void Response_InvalidateClient()
        {
            Authentication = null;

            App.InvalidateClient();
        }
    }
}