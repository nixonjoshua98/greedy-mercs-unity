using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM.Data
{
    public class LocalDataSaveManager : MonoBehaviour
    {
        const float BACKUP_INTERVAL = 3.0f;

        float backupTimer = 10.0f; // 10s grace period

        bool isPaused;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void FixedUpdate()
        {
            backupTimer -= Time.fixedUnscaledDeltaTime;

            if (backupTimer <= 0.0f)
            {
                backupTimer = BACKUP_INTERVAL;

                Backup();
            }
        }

        public void Pause() => isPaused = true;
        public void Continue() => isPaused = false;


        bool CanBackup()
        {
            Scene scene = SceneManager.GetActiveScene();

            if (scene.buildIndex == 0)
                return false;

            return !isPaused;
        }

        void Backup()
        {
            if (CanBackup())
            {
                UserData.Get.Inventory.GetLocalSaveData().Save();
            }
        }
    }
}
