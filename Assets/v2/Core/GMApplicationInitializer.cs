using GM.Common.Interfaces;
using GM.HTTP;
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

                    case HTTPCodes.NoServerResponse:
                        break;
                }
            });
        }


        void FetchGameDataFromServer()
        {
            HTTPClient.Instance.FetchStaticData((resp) =>
            {
                Data.StaticData = resp.StaticData;

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
            GMApplication app = GMApplication.Create(Data.UserData, Data.StaticData);

            app.SaveManager.WriteStaticData(Data.StaticData);

            SceneManager.LoadScene("GameScene");
        }
    }
}