using GM.HTTP;
using GM.HTTP.Requests;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace GM.Core
{
    public class GMApplicationInitializer : MonoBehaviour
    {
        class InitializationPipeline
        {
            public UserLoginReponse Login;
            public FetchUserDataResponse UserData;
            public FetchGameDataResponse GameData;
        }

        void Start()
        {
            StartInitialize();
        }

        void StartInitialize()
        {
            var responses = new InitializationPipeline();

            LoginRequest(responses); // Start request pipeline
        }


        void LoginRequest(InitializationPipeline resps)
        {
            HTTPClient.Instance.Login((loginResp) =>
            {
                resps.Login = loginResp;

                switch (loginResp.StatusCode)
                {
                    case HTTPCodes.Success:
                        FetchGameDataFromServer(resps);
                        break;

                    case HTTPCodes.NoServerResponse:
                        break;
                }
            });
        }


        void FetchGameDataFromServer(InitializationPipeline resps)
        {
            HTTPClient.Instance.FetchStaticData((gameDataResp) =>
            {
                resps.GameData = gameDataResp;

                switch (gameDataResp.StatusCode)
                {
                    case HTTPCodes.Success:
                        FetchUserDataFromServer(resps);
                        break;

                    default:
                        break;
                }
            });
        }


        void FetchUserDataFromServer(InitializationPipeline resps)
        {
            HTTPClient.Instance.FetchUserData((userDataResp) =>
            {
                resps.UserData = userDataResp;

                switch (userDataResp.StatusCode)
                {
                    case HTTPCodes.Success:
                        Initialize(resps.UserData, resps.GameData);
                        break;

                    default:
                        break;
                }
            });
        }


        void Initialize(Common.Data.IServerUserData userDataResp, Common.Data.IStaticGameData gameDataResp)
        {
            GMApplication.Create(userDataResp, gameDataResp);

            SceneManager.LoadScene("GameScene");
        }
    }
}