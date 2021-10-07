using GM.HTTP;
using GM.HTTP.Requests;
using Newtonsoft.Json;
using HTTPCodes = GM.Common.HTTPCodes;
using UnityEngine;

namespace GM
{
    public class GMApplicationInitialization : MonoBehaviour
    {
        public void Start()
        {

        }

        void StartInitialization()
        {
            HTTPClient.Instance.FetchGameData((gameDataResp) =>
            {
                if (gameDataResp.StatusCode == HTTPCodes.Success)
                {
                    HTTPClient.Instance.Login((loginResp) =>
                    {
                        if (loginResp.StatusCode == HTTPCodes.Success)
                        {
                            HTTPClient.Instance.FetchUserData((userDataResp) =>
                            {
                                if (userDataResp.StatusCode == HTTPCodes.Success)
                                {
                                    Initialize(loginResp, userDataResp, gameDataResp);
                                }
                                else
                                {
                                    ShowRequestError(userDataResp, "Data Request");
                                }
                            });
                        }
                        else
                        {
                            ShowRequestError(loginResp, "Login Request");
                        }
                    });
                }
                else
                {
                    ShowRequestError(gameDataResp, "Data Request");
                }
            });
        }


        void Initialize(UserLoginReponse loginResp, FetchUserDataResponse userDataResp, FetchGameDataResponse gameDataResp)
        {
            var app = Core.GMApplication.Create(userDataResp, gameDataResp);
        }


        void ShowRequestError(IServerResponse resp, string reqText)
        {
            Debug.LogError($"{reqText} Failed - {resp.StatusCode} - {resp.ErrorMessage}");
        }
    }
}