using GM.HTTP.Requests;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace GM.HTTP
{
    public class HTTPClient: Common.MonoBehaviourLazySingleton<HTTPClient>
    {
        HTTPServerConfig ServerConfig = new HTTPServerConfig
        {
            Port = 2122,
            Address = "86.153.58.47"
        };

        IServerAuthentication serverAuthDetails;

        // = Armoury = //
        public void Armoury_Upgrade(UpgradeArmouryItemRequest req, UnityAction<UpgradeArmouryItemResponse> callback) => Auth_POST(ServerConfig.UrlFor("armoury/upgrade"), req, callback);
        public void Armoury_Merge(UpgradeStarLevelArmouryItemRequest req, UnityAction<UpgradeStarLevelArmouryItemResponse> callback) => Auth_POST(ServerConfig.UrlFor("armoury/merge"), req, callback);

        // = Login = //
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

        // = Prestige = //
        public void Prestige(UnityAction<ServerResponse> callback) => Auth_POST(ServerConfig.UrlFor("prestige"), new ServerRequest(), callback);

        // = Bounties = //
        public void Bounty_Claim(UnityAction<BountyClaimResponse> callback) => Auth_GET(ServerConfig.UrlFor("bounty/claim"), callback);
        public void Bounty_UpdateActives(UpdateActiveBountiesRequest req, UnityAction<UpdateActiveBountiesResponse> callback) => Auth_POST(ServerConfig.UrlFor("bounty/setactive"), req, callback);

        /// <summary>
        /// Send the request for purchasing an armoury item from the bounty shop
        /// </summary>
        public void BShop_PurchaseArmouryItem(PurchaseBountyShopItemRequest req, UnityAction<PurchaseBountyShopItemResponse> callback) => Auth_POST(ServerConfig.UrlFor("bountyshop/purchase/armouryitem"), req, callback);

        // = Artefacts = //
        public void Artefact_UpgradeArtefact(UpgradeArtefactRequest req, UnityAction<UpgradeArtefactResponse> callback) => Auth_POST(ServerConfig.UrlFor("artefact/upgrade"), req, callback);
        public void Artefact_UnlockArtefact(UnityAction<UnlockArtefactResponse> callback) => Auth_GET(ServerConfig.UrlFor("artefact/unlock"), callback);

        // = Data = //
        public void FetchUserData(UnityAction<FetchUserDataResponse> callback) => Auth_GET(ServerConfig.UrlFor("userdata"), callback);
        public void FetchGameData(UnityAction<FetchGameDataResponse> callback) => Public_GET(ServerConfig.UrlFor("gamedata"), callback);

        /// <summary>
        /// </summary>
        void Public_GET<T>(string url, UnityAction<T> callback) where T : IServerResponse, new()
        {
            IEnumerator _Send()
            {
                yield return SendRequest(UnityWebRequest.Get(url), callback);
            }

            StartCoroutine(_Send());
        }

        /// <summary>
        /// </summary>
        void Auth_GET<T>(string url, UnityAction<T> callback) where T : IServerResponse, new()
        {
            var www = UnityWebRequest.Get(url);

            StartCoroutine(SendAuthenticatedRequest(www, callback));
        }

        /// <summary>
        /// </summary>
        void Auth_POST<T>(string url, IServerRequest request, UnityAction<T> callback) where T : IServerResponse, new()
        {
            var www = UnityWebRequest.Post(url, SerializeRequest(request));

            StartCoroutine(SendAuthenticatedRequest(www, callback));
        }

        /// <summary>
        /// Send all authenticated requests through here. Handles Auth checks and headers
        /// </summary>>
        IEnumerator SendAuthenticatedRequest<T>(UnityWebRequest www, UnityAction<T> callback) where T : IServerResponse, new()
        {
            if (serverAuthDetails == null)
            {
                callback.Invoke(InvalidAuthResponse<T>());
            }
            else
            {
                SetAuthenticationHeaders(ref www);
                yield return SendRequest(www, callback);
            }
        }

        /// <summary>
        /// Send the actual request
        /// </summary>
        IEnumerator SendRequest<T>(UnityWebRequest www, UnityAction<T> callback) where T : IServerResponse, new()
        {
            www.timeout = 5;

            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            InvokeRequestCallback(www, callback);
        }

        /// <summary>
        /// Set request headers used for authentication
        /// </summary>
        void SetAuthenticationHeaders(ref UnityWebRequest www)
        {
            www.SetRequestHeader("x-userid", serverAuthDetails.UserId);
            www.SetRequestHeader("x-sessionid", serverAuthDetails.SessionId);
            www.SetRequestHeader("x-deviceid", SystemInfo.deviceUniqueIdentifier);
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
        /// Serialize request (body) and encrypt
        /// </summary>
        string SerializeRequest<T>(T request) where T: IServerRequest
        {
            return JsonConvert.SerializeObject(request);
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
        /// Utility method to create the response for invalid client credentials
        /// </summary>
        T InvalidAuthResponse<T>() where T : IServerResponse, new() => new T { ErrorMessage = "A game relaunch is required as you are playing in offline mode", StatusCode = HTTPCodes.OfflineMode };
    }
}