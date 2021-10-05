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
    {
        HTTPServerConfig PyServer = new HTTPServerConfig
        {
            Port = 2122,
            Address = "109.154.72.134"
        };


        public void ClaimBounties(UnityAction<BountyClaimResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("bounty/claim"), PrepareRequest(new AuthorisedServerRequest()));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<BountyClaimResponse>(www))));
        }

        public void UpdateActiveBounties(UpdateActiveBountiesRequest req, UnityAction<UpdateActiveBountiesResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("bounty/setactive"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<UpdateActiveBountiesResponse>(www))));
        }

        public void PurchaseBountyShopCurrencyItem(PurchaseBountyShopItemRequest req, UnityAction<PurchaseBountyShopItemResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("bountyshop/purchase/item"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<PurchaseBountyShopItemResponse>(www))));
        }

        public void PurchaseBountyShopArmouryItem(PurchaseBountyShopItemRequest req, UnityAction<PurchaseBountyShopItemResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("bountyshop/purchase/armouryitem"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<PurchaseBountyShopItemResponse>(www))));
        }

        public void UpgradeArmouryItem(UpgradeArmouryItemRequest req, UnityAction<UpgradeArmouryItemResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("armoury/upgrade"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<UpgradeArmouryItemResponse>(www))));
        }

        public void EvolveArmouryItem(EvolveArmouryItemRequest req, UnityAction<EvolveArmouryItemResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("armoury/evolve"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<EvolveArmouryItemResponse>(www))));
        }

        public void UpgradeArtefact(Requests.UpgradeArtefactRequest req, UnityAction<Requests.UpgradeArtefactResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("artefact/upgrade"), PrepareRequest(req));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<Requests.UpgradeArtefactResponse>(www))));
        }

        public void UnlockArtefact(UnityAction<Requests.UnlockArtefactResponse> callback)
        {
            UnityWebRequest www = UnityWebRequest.Post(PyServer.UrlFor("artefact/unlock"), PrepareRequest(new AuthorisedServerRequest()));

            StartCoroutine(SendRequest(www, () => callback(DeserializeResponse<Requests.UnlockArtefactResponse>(www))));
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



        // = = = Private Requests = = = //
        void SendGet(string url, Action<long, JSONNode> callback) => StartCoroutine(ProcessRequest(UnityWebRequest.Get(url), callback));
        void SendPost(string url, JSONNode node, Action<long, JSONNode> callback) => StartCoroutine(ProcessRequest(UnityWebRequest.Post(url, PrepareBody(node)), callback));
        // = = = ^ //


        // POST
        public void Post(string endpoint, JSONNode node, Action<long, JSONNode> callback) => SendPost(PyServer.UrlFor(endpoint), node, callback);
        public void Post(string endpoint, Action<long, JSONNode> callback) => SendPost(PyServer.UrlFor(endpoint), new JSONObject(), callback);

        // GET
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

        /// <summary>
        /// Deserialize a response object. StatusCode and ErrorMessage are set here
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="www">Web request</param>
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
                    StatusCode = GM.Common.HTTPCodes.FailedToDeserialize
                };
            }
            
            return model;
        }

        /// <summary>
        /// Prepare the authorised server request. Involves serializing and setting Auth attributes
        /// </summary>
        /// <typeparam name="T">Request type</typeparam>
        /// <param name="request">Server request model</param>
        string PrepareRequest<T>(T request) where T : IAuthorisedServerRequest
        {
            request.DeviceId = SystemInfo.deviceUniqueIdentifier;

            return JsonConvert.SerializeObject(request);
        }
    }
}
