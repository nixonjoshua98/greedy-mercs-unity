using GM.HTTP;
using GM.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM.Core
{
    struct InitialisationData
    {
        public IServerUserData UserData;
        public IStaticGameData StaticData;
    }

    public class GMApplicationInitializer : MonoBehaviour
    {
        InitialisationData Data = new InitialisationData();

        void Start()
        {
            LoginRequest();
        }


        void LoginRequest()
        {
            HTTPClient.Instance.Login((resp) =>
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


        void FetchGameDataFromServer()
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

        void Initialize()
        {
            LocalStateFile.LoadFromFile(out LocalStateFile localStateFile);

            GMApplication app = GMApplication.Create();

            app.SaveManager.WriteStaticData(Data.StaticData);

            app.UpdateDataContainers(Data.UserData, Data.StaticData, localStateFile);

            SceneManager.LoadSceneAsync("GameScene");
        }
    }
}