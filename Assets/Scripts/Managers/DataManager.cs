
using UnityEngine;

namespace GreedyMercs
{
    public class DataManager : MonoBehaviour
    {
        public static string LOCAL_FILENAME = "localsave_06";

        public static string LOCAL_STATIC_FILENAME = "localstatic";

        // ===
        public static bool IsPaused;

        void Awake()
        {
            IsPaused = false;
        }

        void Start()
        {
            Invoke("WriteStateToFile", 1.0f);
        }

        void WriteStateToFile()
        {
            if (!IsPaused)
                Save();

            Invoke("WriteStateToFile", 1.0f);
        }

        public static void Save()
        {
            Utils.File.WriteJson(LOCAL_FILENAME, GameState.ToJson());
        }
    }
}