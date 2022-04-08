using GM.HTTP;
using GM.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            LocalStateFile.LoadFromFile(out LocalStateFile localStateFile);

            GMApplication app = GMApplication.Create();

            app.UpdateDataContainers(Data.UserData, Data.StaticData, localStateFile);

            SceneManager.LoadSceneAsync("GameScene");
        }
    }
}