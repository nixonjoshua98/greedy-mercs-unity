using GM.Common.Interfaces;
using GM.HTTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using GM.LocalFiles;

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
            LocalPersistantFile.LoadFromFile(out LocalPersistantFile persistFile);
            LocalStateFile.LoadFromFile(out LocalStateFile localStateFile);

            LocalSaveManager saveManager = LocalSaveManager.Create();

            saveManager.WriteStaticData(Data.StaticData);

            GMData dataContainer = new GMData();

            new GMApplication(persistFile, dataContainer, saveManager).SetInstance();

            dataContainer.Set(Data.UserData, Data.StaticData, localStateFile);

            SceneManager.LoadScene("GameScene");
        }
    }
}