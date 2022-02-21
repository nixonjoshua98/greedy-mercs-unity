using GM.HTTP.Requests;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GM.HTTP
{
    public class HTTPClient: Common.MonoBehaviourLazySingleton<HTTPClient>
    {
        HTTPServerConfig ServerConfig = new HTTPServerConfig
        {
            Port = 2122,
            Address = "109.154.100.101"
        };

        IServerAuthentication Authentication;

        public bool IsAuthenticated => Authentication.Session != null;

        public void UnlockArtefact(Action<UnlockArtefactResponse> callback)
        {
            var www = UnityWebRequest.Get(ResolveURL("artefact/unlock"));

            SendAuthenticatedRequest(www, callback);
        }

        public void UpgradeArtefact(int artefactId, int levelsUpgrading, Action<UpgradeArtefactResponse> callback)
        {
            var req = new UpgradeArtefactRequest(artefactId, levelsUpgrading);
            var www = UnityWebRequest.Post(ResolveURL("artefact/upgrade"), SerializeRequest(req));

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

            SendPublicRequest(www, callback);
        }

        public void Login(Action<UserLoginReponse> callback)
        {
            var req = new UserLoginRequest(SystemInfo.deviceUniqueIdentifier);
            var www = UnityWebRequest.Post(ResolveURL("login"), SerializeRequest(req));

            SendPublicRequest<UserLoginReponse>(www, (response) =>
            {
                Authentication = null;

                if (response.StatusCode == HTTPCodes.Success)
                {
                    Authentication = response;
                }

                callback.Invoke(response);
            });
        }

        public void Prestige(Action<PrestigeResponse> callback)
        {
            var req = new PrestigeRequest();
            var www = UnityWebRequest.Post(ResolveURL("prestige"), SerializeRequest(req));

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
            if (!IsAuthenticated)
            {
                callback.Invoke(InvalidAuthResponse<T>());
            }
            else
            {
                SetAuthenticationHeader(ref www);

                StartCoroutine(SendRequest(www, callback));
            }
        }

        IEnumerator SendRequest<T>(UnityWebRequest www, Action<T> callback) where T : IServerResponse, new()
        {
            www.timeout = 5;

            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            HandleRequestResponse(www, callback);
        }

        void HandleRequestResponse<T>(UnityWebRequest www, Action<T> callback) where T : IServerResponse, new()
        {
            T resp = DeserializeResponse<T>(www);

            try
            {
                callback.Invoke(resp);
            }
            finally
            {
                if (resp.StatusCode != HTTPCodes.Success)
                {
                    Debug.LogError(resp.ErrorMessage);
                }
            }
        }

        void SetAuthenticationHeader(ref UnityWebRequest www)
        {
            www.SetRequestHeader("Authentication", Authentication.Session);
        }

        T DeserializeResponse<T>(UnityWebRequest www) where T : IServerResponse, new()
        {
            T model;

            try
            {
                // Attempt to deserialize the response text
                model = JsonConvert.DeserializeObject<T>(www.downloadHandler.text);

                if (model == null)
                {
                    model = new T()
                    {
                        ErrorMessage = "Failed to deserialize server response"
                    };
                }

                model.StatusCode = www.responseCode;
            }
            catch (Exception e)
            {
                // We failed to deserialize for an unknown reason so we set the error message and status code
                model = new T()
                {
                    ErrorMessage = e.Message,
                    StatusCode = HTTPCodes.FailedToDeserialize
                };
            }

            return model;
        }

        string SerializeRequest<T>(T request) where T: IServerRequest
        {
            return JsonConvert.SerializeObject(request);
        }

        T InvalidAuthResponse<T>() where T : IServerResponse, new() => new T { ErrorMessage = "A game relaunch is required as you are playing in offline mode", StatusCode = HTTPCodes.OfflineMode };
    }
}