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

        IServerAuthentication serverAuthDetails;

        public int maxRequestId;
        public int currentRequestId;

        // == Armoury Requests == //
        public void Armoury_Upgrade(UpgradeArmouryItemRequest req, UnityAction<UpgradeArmouryItemResponse> callback) =>
            Post(ServerConfig.UrlFor("armoury/upgrade"), req, callback);

        public void Armoury_Merge(UpgradeStarLevelArmouryItemRequest req, UnityAction<UpgradeStarLevelArmouryItemResponse> callback) =>
            Post(ServerConfig.UrlFor("armoury/merge"), req, callback);


        // == Login == //
        public void Login(UnityAction<UserLoginReponse> callback)
        {
            UserLoginRequest request = new UserLoginRequest { DeviceId = SystemInfo.deviceUniqueIdentifier };

            var www = UnityWebRequest.Post(ServerConfig.UrlFor("login"), JsonConvert.SerializeObject(request));

            StartCoroutine(SendRequest<UserLoginReponse>(www, (resp) =>
            {
                serverAuthDetails = null;

                if (resp.StatusCode == HTTPCodes.Success)
                {
                    serverAuthDetails = resp;
                }

                callback.Invoke(resp);
            }));
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
            StartCoroutine(SendAuthenticatedRequest("POST", url, req, callback));

        /// <summary>
        /// </summary>
        void Get<T>(string url, UnityAction<T> callback) where T : IServerResponse, new()
        {
            var www = UnityWebRequest.Get(url);

            StartCoroutine(SendRequest<T>(www, callback));
        }

        /// <summary>
        /// </summary>
        IEnumerator SendRequest<T>(UnityWebRequest www, UnityAction<T> callback) where T : IServerResponse, new()
        {
            www.timeout = 5;

            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            InvokeRequestCallback(www, callback);
        }

        IEnumerator SendAuthenticatedRequest<T>(string method, string url, IAuthenticatedRequest request, UnityAction<T> callback) where T : IServerResponse, new()
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
                yield return WaitForRequestReady();

                var www = method switch
                {
                    "POST" => UnityWebRequest.Post(url, PrepareRequest(request)),
                    _ => throw new Exception($"Invalid HTTP method: {method}")
                };

                www.SetRequestHeader("x-userid", serverAuthDetails.UserId);
                www.SetRequestHeader("x-sessionid", serverAuthDetails.SessionId);
                www.SetRequestHeader("x-deviceid", SystemInfo.deviceUniqueIdentifier);

                yield return SendRequest<T>(www, callback);
            }
        }

        IEnumerator WaitForRequestReady()
        {
            maxRequestId++;

            int requestId = maxRequestId;

            // Send the request in order as they are created
            yield return new WaitUntil(() => currentRequestId == requestId);
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

                currentRequestId++; // Increment the current request id, so that the next queued request is sent and processed
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
    }
}
