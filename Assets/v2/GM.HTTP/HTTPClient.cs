using GM.HTTP.Requests;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace GM.HTTP
{
    class ServerAuthenticationDetails
    {
        public string UserId = null;
    }

    public class HTTPClient: Common.MonoBehaviourLazySingleton<HTTPClient>
    {
        HTTPServerConfig ServerConfig = new HTTPServerConfig
        {
            Port = 2122,
            Address = "86.153.58.47"
        };

        ServerAuthenticationDetails serverAuthDetails = new ServerAuthenticationDetails();


        // == Armoury Requests == //
        public void Armoury_Upgrade(UpgradeArmouryItemRequest req, UnityAction<UpgradeArmouryItemResponse> callback) =>
            Post(ServerConfig.UrlFor("armoury/upgrade"), req, callback);

        public void Armoury_Merge(UpgradeStarLevelArmouryItemRequest req, UnityAction<UpgradeStarLevelArmouryItemResponse> callback) =>
            Post(ServerConfig.UrlFor("armoury/merge"), req, callback);


        // == Login == //
        public void Login(UnityAction<UserLoginReponse> callback)
        {
            Post<UserLoginReponse>(ServerConfig.UrlFor("login"), new UserLoginRequest(), (resp) =>
            {
                serverAuthDetails = new ServerAuthenticationDetails();

                if (resp.StatusCode == HTTPCodes.Success)
                {
                    serverAuthDetails.UserId = resp.UserId;
                }

                callback.Invoke(resp);
            });
        }


        // == Prestige == //
        public void Prestige(UnityAction<ServerResponse> callback) => Post(ServerConfig.UrlFor("prestige"), new AuthenticatedRequest(), callback);


        // == Bounties == //
        public void Bounty_Claim(UnityAction<BountyClaimResponse> callback) => Post(ServerConfig.UrlFor("bounty/claim"), new AuthenticatedRequest(), callback);
        public void Bounty_UpdateActives(UpdateActiveBountiesRequest req, UnityAction<UpdateActiveBountiesResponse> callback) => Post(ServerConfig.UrlFor("bounty/setactive"), req, callback);


        // == Bounty Shop == //
        public void BShop_PurchaseItem(PurchaseBountyShopItemRequest req, UnityAction<PurchaseBountyShopItemResponse> callback) => Post(ServerConfig.UrlFor("bountyshop/purchase"), req, callback);


        // == Artefacts == //
        public void Artefact_UpgradeArtefact(UpgradeArtefactRequest req, UnityAction<UpgradeArtefactResponse> callback) => Post(ServerConfig.UrlFor("artefact/upgrade"), req, callback);
        public void Artefact_UnlockArtefact(UnityAction<UnlockArtefactResponse> callback) => Post(ServerConfig.UrlFor("artefact/unlock"), new AuthenticatedRequest(), callback);


        // == Data == //
        public void FetchUserData(UnityAction<FetchUserDataResponse> callback) => Post(ServerConfig.UrlFor("userdata"), new FetchUserDataRequest(), callback);
        public void FetchGameData(UnityAction<FetchGameDataResponse> callback) => Get(ServerConfig.UrlFor("gamedata"), callback);


        /// <summary>
        /// </summary>
        void Post<T>(string url, IAuthenticatedRequest req, UnityAction<T> callback) where T : IServerResponse, new() => 
            SendAuthenticatedRequest("POST", url, req, callback);

        /// <summary>
        /// </summary>
        void Post<T>(string url, ILoginRequest req, UnityAction<T> callback) where T : IServerResponse, new()
        {
            var www = UnityWebRequest.Post(url, PrepareRequest(req));

            StartCoroutine(Send<T>(www, callback));
        }

        /// <summary>
        /// </summary>
        void Get<T>(string url, UnityAction<T> callback) where T : IServerResponse, new()
        {
            var www = UnityWebRequest.Get(url);

            StartCoroutine(Send<T>(www, callback));
        }

        /// <summary>
        /// </summary>
        IEnumerator Send<T>(UnityWebRequest www, UnityAction<T> callback) where T : IServerResponse, new()
        {
            www.timeout = 5;

            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            InvokeRequestCallback(www, callback);
        }

        void SendAuthenticatedRequest<T>(string method, string url, IAuthenticatedRequest request, UnityAction<T> callback) where T : IServerResponse, new()
        {
            if (serverAuthDetails == null)
            {
                T resp = new T() {
                    ErrorMessage = "A game relaunch is required as you are playing in offline mode", 
                    StatusCode = HTTPCodes.IsOfflineMode
                };

                callback.Invoke(resp);
            }
            else
            {
                var www = method switch
                {
                    "POST" => UnityWebRequest.Post(url, PrepareRequest(request)),
                };

                StartCoroutine(Send<T>(www, callback));
            }
        }

        /// <summary>
        /// Performs some post request logic and calls the request callback
        /// </summary>
        void InvokeRequestCallback<T>(UnityWebRequest www, UnityAction<T> callback) where T : IServerResponse, new()
        {
            T resp = DeserializeResponse<T>(www);

            try
            {
                UpdateResponseErrorMessage(www, resp);

                callback.Invoke(resp);
            }
            finally
            {
                if (resp == null || resp.StatusCode != 200)
                {
                    Debug.Log($"{www.url} ({resp.StatusCode}) - {resp.ErrorMessage}");
                }
            }
        }

        /// <summary>
        /// Deserialize the response into the request response object
        /// </summary>
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

            UpdateResponseErrorMessage(www, model);

            return model;
        }

        /// <summary>
        /// Update the error message if a generic response
        /// </summary>
        void UpdateResponseErrorMessage(UnityWebRequest www, IServerResponse response)
        {
            switch (www.responseCode)
            {
                case HTTPCodes.NoServerResponse:
                    response.ErrorMessage = "Failed to connect to server";
                    break;
            }

        }

        /// <summary>
        /// Pre-request setup. Sets up Auth values etc
        /// </summary>
        string PrepareRequest(IAuthenticatedRequest request)
        {
            request.UserId = serverAuthDetails.UserId;
            request.DeviceId = SystemInfo.deviceUniqueIdentifier;

            return JsonConvert.SerializeObject(request);
        }

        /// <summary>
        /// Pre-request setup. Sets identifer details for login
        /// </summary>
        string PrepareRequest(ILoginRequest request)
        {
            request.DeviceId = SystemInfo.deviceUniqueIdentifier;

            return JsonConvert.SerializeObject(request);
        }
    }
}
