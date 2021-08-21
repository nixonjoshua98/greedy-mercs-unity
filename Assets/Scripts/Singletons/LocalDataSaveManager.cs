using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM.Data
{
    public class LocalDataSaveManager : MonoBehaviour
    {
        const float BACKUP_INTERVAL = 3.0f;

        float backupTimer = 10.0f; // 10s grace period

        bool isPaused;

        public static LocalDataSaveManager Get = null;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Get = this;
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

        public void ClearLocalData(bool pause = false)
        {
            if (pause)
                Pause();

            string path = FileUtils.ResolvePath("local");

            if (System.IO.File.Exists(path))
                System.IO.Directory.Delete(path, true);
        }


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
