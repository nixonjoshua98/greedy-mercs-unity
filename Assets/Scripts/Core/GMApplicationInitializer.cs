using SRC.HTTP;
using SRC.Models;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SRC.Core
{
    internal struct InitialisationData
    {
        public IServerUserData UserData;
        public IStaticGameData StaticData;
    }

    public class GMApplicationInitializer : MonoBehaviour
    {
        [SerializeField] private GMApplication App = new();

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
            App.Initialize(Data.UserData, Data.StaticData);

            App.Quests.FetchQuests((success) =>
            {
                SceneManager.LoadSceneAsync("GameScene");
            });
        }
    }
}