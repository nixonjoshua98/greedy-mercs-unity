using GM.HTTP.Requests;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace GM.HTTP
{
    public class HTTPClient: Common.MonoBehaviourLazySingleton<HTTPClient>
    {
        HTTPServerConfig PyServer = new HTTPServerConfig
        {
            Port = 2122,
            Address = "109.154.72.134"
        };

        public void Bounty_Claim(UnityAction<BountyClaimResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("bounty/claim"), PrepareRequest(new AuthorisedRequest()));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<BountyClaimResponse>(www))));
        }

        public void Bounty_UpdateActives(UpdateActiveBountiesRequest req, UnityAction<UpdateActiveBountiesResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("bounty/setactive"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<UpdateActiveBountiesResponse>(www))));
        }

        public void BShop_PurchaseItem(PurchaseBountyShopItemRequest req, UnityAction<PurchaseBountyShopItemResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("bountyshop/purchase"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<PurchaseBountyShopItemResponse>(www))));
        }

        public void Armoury_UpgradeItem(UpgradeArmouryItemRequest req, UnityAction<UpgradeArmouryItemResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("armoury/upgrade"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<UpgradeArmouryItemResponse>(www))));
        }

        public void Armoury_EvolveItem(EvolveArmouryItemRequest req, UnityAction<EvolveArmouryItemResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("armoury/evolve"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<EvolveArmouryItemResponse>(www))));
        }

        public void Artefact_UpgradeArtefact(UpgradeArtefactRequest req, UnityAction<UpgradeArtefactResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("artefact/upgrade"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<UpgradeArtefactResponse>(www))));
        }

        public void Artefact_UnlockArtefact(UnityAction<UnlockArtefactResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("artefact/unlock"), PrepareRequest(new AuthorisedRequest()));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<UnlockArtefactResponse>(www))));
        }

        public void Login(UnityAction<UserLoginReponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("login"), PrepareLoginRequest());

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<UserLoginReponse>(www))));
        }

        public void FetchUserData(UnityAction<FetchUserDataResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("userdata"), PrepareRequest(new FetchUserDataRequest()));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<FetchUserDataResponse>(www))));
        }

        public void FetchGameData(UnityAction<FetchGameDataResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Get(PyServer.UrlFor("gamedata"));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<FetchGameDataResponse>(www))));
        }

        /// <summary>
        /// Send the web request and invoke the callback
        /// </summary>
        /// <param name="www">Web request object</param>
        /// <param name="callback">Callback</param>
        IEnumerator SendRequest(UnityWebRequest www, UnityAction callback)
        {
            www.timeout = 5;

            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            callback();
        }



        void SendGet(string url, Action<long, JSONNode> callback) => StartCoroutine(ProcessRequest(UnityWebRequest.Get(url), callback));
        void SendPost(string url, JSONNode node, Action<long, JSONNode> callback) => StartCoroutine(ProcessRequest(UnityWebRequest.Post(url, PrepareBody(node)), callback));
        public void Post(string endpoint, JSONNode node, Action<long, JSONNode> callback) => SendPost(PyServer.UrlFor(endpoint), node, callback);
        public void Get(string endpoint, Action<long, JSONNode> callback) => SendGet(PyServer.UrlFor(endpoint), callback);


        IEnumerator ProcessRequest(UnityWebRequest www, Action<long, JSONNode> callback)
        {
            www.timeout = 3;

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");

            yield return www.SendWebRequest();

            bool _ = TryParseJSON(www.downloadHandler.text, out JSONNode resp);

            callback.Invoke(www.responseCode, resp);
        }


        bool TryParseJSON(string s, out JSONNode result)
        {
            result = new JSONObject();

            try
            {
                result = JSON.Parse(s);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }


        string PrepareBody(JSONNode node)
        {
            node["deviceId"] = SystemInfo.deviceUniqueIdentifier;

            return node.ToString();
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
                        StatusCode = GM.Common.HTTPCodes.FailedToDeserialize
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
                    StatusCode = Common.HTTPCodes.FailedToDeserialize
                };
            }
            
            return model;
        }

        string PrepareRequest<T>(T request) where T : IAuthorisedRequest
        {
            request.DeviceId = SystemInfo.deviceUniqueIdentifier;

            return JsonConvert.SerializeObject(request);
        }

        string PrepareLoginRequest()
        {
            var request = new UserLoginRequest
            {
                DeviceId = SystemInfo.deviceUniqueIdentifier
            };

            return JsonConvert.SerializeObject(request);
        }
    }
}
