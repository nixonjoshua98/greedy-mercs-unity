using GM.HTTP;
using GM.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This needs reworking
 */

namespace GM.Core
{
    internal struct InitialisationData
    {
        public IServerUserData UserData;
        public IStaticGameData StaticData;
    }

    public class GMApplicationInitializer : MonoBehaviour
    {
        private InitialisationData Data = new();

        private void Start()
        {
            LoginRequest();
        }

        private void LoginRequest()
        {
            HTTPClient.Instance.DeviceLogin((resp) =>
            {
                Debug.Log("Fetched User Data " + resp.StatusCode);

                Data.UserData = resp.UserData;

                switch (resp.StatusCode)
                {
                    case HTTPCodes.Success:
                        FetchGameDataFromServer();
                        break;
                }
            });
        }

        private void FetchGameDataFromServer()
        {
            HTTPClient.Instance.FetchStaticData((resp) =>
            {
                Debug.Log("Fetched Static Data " + resp.StatusCode);

                Data.StaticData = resp;

                switch (resp.StatusCode)
                {
                    case HTTPCodes.Success:
                        Initialize();
                        break;

                    default:
                        break;
                }
            });
        }

        private void Initialize()
        {
            GMApplication app = GMApplication.Instance;

            app.Initialize(Data.UserData, Data.StaticData);

            app.Quests.FetchQuests((success) =>
            {
                SceneManager.LoadSceneAsync("GameScene");
            });
        }
    }
}