using UnityEngine;

namespace GM.Data
{
    public class LocalDataSaveManager : MonoBehaviour
    {
        static LocalDataSaveManager Instance = null;
        public static LocalDataSaveManager Get => Instance;

        const float BACKUP_INTERVAL = 3.0f;

        float backupTimer = 10.0f; // 10s grace period

        bool isPaused;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Instance.Continue();

                Destroy(gameObject);
            }
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

            if (System.IO.Directory.Exists(path))
                System.IO.Directory.Delete(path, true);
        }


        bool CanBackup()
        {
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
