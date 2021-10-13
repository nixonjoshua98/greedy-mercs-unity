using GM.HTTP.Requests;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace GM.HTTP
{
    class AuthDetails
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

        AuthDetails Auth = new AuthDetails();
        

        // == Login == //
        public void Login(UnityAction<UserLoginReponse> callback)
        {
            Post<UserLoginReponse>(ServerConfig.UrlFor("login"), new UserLoginRequest(), (resp) =>
            {
                Auth = new AuthDetails();

                if (resp.StatusCode == HTTPCodes.Success)
                {
                    Auth.UserId = resp.UserId;
                }

                callback.Invoke(resp);
            });
        }


        // == Prestige == ..
        public void Prestige(UnityAction<ServerResponse> callback)
        {
            Post(ServerConfig.UrlFor("prestige"), new AuthorisedRequest(), callback);
        }


        // == Bounties == //
        public void Bounty_Claim(UnityAction<BountyClaimResponse> callback) => Post(ServerConfig.UrlFor("bounty/claim"), new AuthorisedRequest(), callback);
        public void Bounty_UpdateActives(UpdateActiveBountiesRequest req, UnityAction<UpdateActiveBountiesResponse> callback) => Post(ServerConfig.UrlFor("bounty/setactive"), req, callback);


        // == Bounty Shop == //
        public void BShop_PurchaseItem(PurchaseBountyShopItemRequest req, UnityAction<PurchaseBountyShopItemResponse> callback) => Post(ServerConfig.UrlFor("bountyshop/purchase"), req, callback);


        // == Armoury == //
        public void Armoury_UpgradeItem(UpgradeArmouryItemRequest req, UnityAction<UpgradeArmouryItemResponse> callback) => Post(ServerConfig.UrlFor("armoury/upgrade"), req, callback);
        public void Armoury_EvolveItem(EvolveArmouryItemRequest req, UnityAction<EvolveArmouryItemResponse> callback) => Post(ServerConfig.UrlFor("armoury/evolve"), req, callback);


        // == Artefacts == //
        public void Artefact_UpgradeArtefact(UpgradeArtefactRequest req, UnityAction<UpgradeArtefactResponse> callback) => Post(ServerConfig.UrlFor("artefact/upgrade"), req, callback);
        public void Artefact_UnlockArtefact(UnityAction<UnlockArtefactResponse> callback) => Post(ServerConfig.UrlFor("artefact/unlock"), new AuthorisedRequest(), callback);

        // == Data == //
        public void FetchUserData(UnityAction<FetchUserDataResponse> callback) => Post(ServerConfig.UrlFor("userdata"), new FetchUserDataRequest(), callback);
        public void FetchGameData(UnityAction<FetchGameDataResponse> callback) => Get(ServerConfig.UrlFor("gamedata"), callback);


        /// <summary>
        /// Authorised server POST request
        /// </summary>
        void Post<T>(string url, IAuthorisedRequest req, UnityAction<T> callback) where T: IServerResponse, new()
        {
            var www = UnityWebRequest.Post(url, PrepareRequest(req));

            StartCoroutine(Send<T>(www, (resp) => { InvokeRequestCallback(resp, callback); }));
        }


        /// <summary>
        /// Login POST request. Special kind of request seperate from Authorised, and Public
        /// </summary>
        void Post<T>(string url, ILoginRequest req, UnityAction<T> callback) where T : IServerResponse, new()
        {
            var www = UnityWebRequest.Post(url, PrepareRequest(req));

            StartCoroutine(Send<T>(www, (resp) => { InvokeRequestCallback(resp, callback); }));
        }

        /// <summary>
        /// Public server GET request
        /// </summary>
        void Get<T>(string url, UnityAction<T> callback) where T : IServerResponse, new()
        {
            var www = UnityWebRequest.Get(url);

            StartCoroutine(Send<T>(www, (resp) => { InvokeRequestCallback(resp, callback); }));
        }

        /// <summary>
        /// Sends the actual web request
        /// </summary>
        /// <returns></returns>
        IEnumerator Send<T>(UnityWebRequest www, UnityAction<T> callback) where T : IServerResponse, new()
        {
            www.timeout = 5;

            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            callback.Invoke(DeserializeResponse<T>(www));
        }

        /// <summary>
        /// Performs some post request logic and calls the request callback
        /// </summary>
        void InvokeRequestCallback<T>(T resp, UnityAction<T> callback) where T : IServerResponse
        {
            try
            {
                callback.Invoke(resp);
            }
            finally
            {

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
            
            return model;
        }

        /// <summary>
        /// Pre-request setup. Sets up Auth values etc
        /// </summary>
        string PrepareRequest(IAuthorisedRequest request)
        {
            request.UserId = Auth.UserId;
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
