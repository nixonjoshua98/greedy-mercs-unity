using GM.HTTP;
using GM.Models;
using GM.ScriptableObjects;
using System.Collections.Generic;
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
        [Header("Scriptable Objects")]
        public List<ItemGradeConfig> ItemGradesConfigs;
        public List<CurrencyConfig> CurrencyItemsConfig;

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

            LocalGameDataContainer localGameData = new()
            {
                ItemGradesConfigs = ItemGradesConfigs,
                CurrencyItemsConfig = CurrencyItemsConfig
            };

            GMApplication app = GMApplication.Create(localGameData);

            app.UpdateDataContainers(Data.UserData, Data.StaticData, localStateFile);

            SceneManager.LoadSceneAsync("GameScene");
        }
    }
}