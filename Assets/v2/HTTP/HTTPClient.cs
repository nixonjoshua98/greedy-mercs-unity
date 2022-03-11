using GM.HTTP.Requests;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using GM.Artefacts.UI;
using GM.Artefacts;
using GM.Artefacts.Models;

namespace GM.HTTP
{
    public class HTTPClient: Common.MonoBehaviourLazySingleton<HTTPClient>
    {
        HTTPServerConfig ServerConfig = new HTTPServerConfig
        {
            Port = 2122,
            Address = "localhost"
        };

        IServerAuthentication Authentication;

        public void UnlockArtefact(Action<UnlockArtefactResponse> callback)
        {
            var www = UnityWebRequest.Get(ResolveURL("artefact/unlock"));

            SendAuthenticatedRequest(www, callback);
        }

        public void BulkUpgradeArtefacts(Dictionary<int, int> artefacts, Action<BulkUpgradeResponse> callback)
        {
            var req = new BulkUpgradeRequest()
            {
                Artefacts = artefacts.Select(x => new BulkArtefactUpgrade(x.Key, x.Value)).ToList()
            };

            var www = UnityWebRequest.Post(ResolveURL("artefact/bulk-upgrade"), SerializeRequest(req));

            SendAuthenticatedRequest(www, callback);
        }

        public void UpgradeArmouryItem(int item, Action<UpgradeArmouryItemResponse> callback)
        {
            var req = new UpgradeArmouryItemRequest(item);
            var www = UnityWebRequest.Post(ResolveURL("armoury/upgrade"), SerializeRequest(req));

            SendAuthenticatedRequest(www, callback);
        }

        public void ClaimBounties(Action<BountyClaimResponse> callback)
        {
            var www = UnityWebRequest.Get(ResolveURL("bounty/claim"));

            SendAuthenticatedRequest(www, callback);
        }

        public void SetActiveBounties(List<int> bounties, Action<UpdateActiveBountiesResponse> callback)
        {
            var req = new UpdateActiveBountiesRequest(bounties);
            var www = UnityWebRequest.Post(ResolveURL("bounty/setactive"), SerializeRequest(req));

            SendAuthenticatedRequest(www, callback);
        }

        public void FetchStaticData(Action<FetchGameDataResponse> callback)
        {
            var www = UnityWebRequest.Get(ResolveURL("static"));

            SendAuthenticatedRequest(www, callback);
        }

        public void Login(Action<LoginResponse> callback)
        {
            var req = new LoginRequest(SystemInfo.deviceUniqueIdentifier);
            var www = UnityWebRequest.Post(ResolveURL("login"), SerializeRequest(req));

            SendPublicRequest<LoginResponse>(www, (response) =>
            {
                Authentication = null;

                if (response.StatusCode == HTTPCodes.Success)
                {
                    Authentication = response;
                }

                callback.Invoke(response);
            });
        }

        public void Prestige(PrestigeRequest request, Action<PrestigeResponse> callback)
        {
            var www = UnityWebRequest.Post(ResolveURL("prestige"), SerializeRequest(request));

            SendAuthenticatedRequest(www, callback);
        }

        public void BuyBountyShopArmouryItem(string item, Action<Requests.BountyShop.PurchaseArmouryItemResponse> callback)
        {
            var req = new Requests.BountyShop.PurchaseBountyShopItem(item);
            var www = UnityWebRequest.Post(ResolveURL("bountyshop/purchase/armouryitem"), SerializeRequest(req));

            SendAuthenticatedRequest(www, callback);
        }

        public void PurchaseBountyShopCurrencyType(string item, Action<Requests.BountyShop.PurchaseCurrencyResponse> callback)
        {
            var req = new Requests.BountyShop.PurchaseBountyShopItem(item);
            var www = UnityWebRequest.Post(ResolveURL("bountyshop/purchase/currency"), SerializeRequest(req));

            SendAuthenticatedRequest(www, callback);
        }

        string ResolveURL(string endpoint) => $"{ServerConfig.Url}/{endpoint}";

        void SendPublicRequest<T>(UnityWebRequest www, Action<T> callback) where T : IServerResponse, new()
        {
            StartCoroutine(SendRequest(www, callback));
        }

        void SendAuthenticatedRequest<T>(UnityWebRequest www, Action<T> callback) where T : IServerResponse, new()
        {
            SetAuthenticationHeader(ref www);

            StartCoroutine(SendRequest(www, callback));
        }

        IEnumerator SendRequest<T>(UnityWebRequest www, Action<T> callback) where T : IServerResponse, new()
        {
            SetRequiredHeaders(ref www);

            using (www)
            {
                yield return www.SendWebRequest();

                bool isEncrypted = www.GetBoolResponseHeader("Response-Encrypted", false);

                T resp = DeserializeResponse<T>(www, isEncrypted);

                callback.Invoke(resp);
            }
        }

        void SetAuthenticationHeader(ref UnityWebRequest www)
        {
            www.SetRequestHeader("Authentication", Authentication.Session);
        }

        void SetRequiredHeaders(ref UnityWebRequest www)
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("DeviceId", SystemInfo.deviceUniqueIdentifier);
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
                    model = new T() { ErrorMessage = "Failed to deserialize server response" };
                }

            }
            catch (Exception e)
            {
                model = new T() { ErrorMessage = e.Message };
            }

            model.StatusCode = www.responseCode;

            return model;
        }

        string SerializeRequest<T>(T request) where T: IServerRequest
        {
            return JsonConvert.SerializeObject(request);
        }
    }
}