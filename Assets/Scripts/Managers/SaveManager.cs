
using UnityEngine;

namespace GreedyMercs
{
    public class DataManager : MonoBehaviour
    {
        public static string DATA_FILE = "localsave01";
        public static string LOCAL_ONLY_FILE = "localonly01";

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
            GameState.Save();
        }
    }
}