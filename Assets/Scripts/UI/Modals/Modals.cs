using GM.HTTP;
using GM.UI.InfoPopups;
using UnityEngine;

namespace GM
{
    /// <summary>
    /// Singleton with generic UI modal objects
    /// </summary>
    public class Modals : GM.Core.GMMonoBehaviour
    {
        static Modals Instance;

        [SerializeField] GameObject InfoPopupObject;

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public static void ShowServerError(IServerResponse resp)
        {
            Instance._ShowServerError(resp);
        }

        void _ShowServerError(IServerResponse resp)
        {
            InstantiateUI<InfoModal>(InfoPopupObject).Set($"Server Error ({resp.StatusCode})", resp.Message);
        }
    }
}
