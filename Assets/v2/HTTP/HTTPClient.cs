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
            Address = "86.191.239.53"
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

        public void MergeArmouryItem(int item, Action<MergeArmouryItemResponse> callback)
        {
            var req = new MergeArmouryItemRequest(item);
            var www = UnityWebRequest.Post(ResolveURL("armoury/merge"), SerializeRequest(req));

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

        public void FetchGameData(Action<FetchGameDataResponse> callback)
        {
            var www = UnityWebRequest.Get(ResolveURL("gamedata"));

            SendPublicRequest(www, callback);
        }

        public void FetchUserData(Action<FetchUserDataResponse> callback)
        {
            var www = UnityWebRequest.Get(ResolveURL("userdata"));

            SendAuthenticatedRequest(www, callback);
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

        public void BuyBountyShopArmouryItem(string item, Action<PurchaseArmouryItemResponse> callback)
        {
            var req = new PurchaseBountyShopItem(item);
            var www = UnityWebRequest.Post(ResolveURL("bountyshop/purchase/armouryitem"), SerializeRequest(req));

            SendAuthenticatedRequest(www, callback);
        }

        string ResolveURL(string endpoint) => $"{ServerConfig.Url_}/{endpoint}";

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
                    // Create a model, and populate the error message (and status code) to show an error happened
                    model = new T()
                    {
                        ErrorMessage = "Failed to deserialize server response",
                        StatusCode = HTTPCodes.FailedToDeserialize
                    };
                }
                else
                {
                    // If we deserialize then we should use the status code from the server
                    model.StatusCode = www.responseCode;
                }
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