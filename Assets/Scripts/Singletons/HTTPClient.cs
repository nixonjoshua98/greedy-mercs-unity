using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;


namespace GM.HTTP
{
    struct ServerConfig
    {
        public string Address;

        public int Port;

        public string Url(string endpoint) => string.Format("http://{0}:{1}/api/{2}", Address, Port, endpoint);
    }



    public class HTTPClient : Common.MonoBehaviourLazySingleton<HTTPClient>
    {
        // = = = Static = = = //
        static HTTPClient _instance = null;

        public static HTTPClient GetClient()
        {
            if (_instance == null)
            {
                _instance = new GameObject("HTTPClient").AddComponent<HTTPClient>();

                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
        // = = = ^ Static ^ = = = //


        ServerConfig config = new ServerConfig()
        {
            Port = 2122,
            Address = "212.140.123.165"
        };

        // = = = Private Requests = = = //
        void SendGet(string url, Action<long, JSONNode> callback) => StartCoroutine(ProcessRequest(UnityWebRequest.Get(url), callback));
        void SendPost(string url, JSONNode node, Action<long, JSONNode> callback) => StartCoroutine(ProcessRequest(UnityWebRequest.Post(url, PrepareBody(node)), callback));
        // = = = ^ //


        // POST
        public void Post(string endpoint, JSONNode node, Action<long, JSONNode> callback) => SendPost(config.Url(endpoint), node, callback);
        public void Post(string endpoint, Action<long, JSONNode> callback) => SendPost(config.Url(endpoint), new JSONObject(), callback);

        // GET
        public void Get(string endpoint, Action<long, JSONNode> callback) => SendGet(config.Url(endpoint), callback);


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
    }
}
