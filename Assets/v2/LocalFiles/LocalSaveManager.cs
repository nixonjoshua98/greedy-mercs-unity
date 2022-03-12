using GM.Models;
using System.Collections;
using UnityEngine;

namespace GM
{
    public class LocalSaveManager : Common.MonoBehaviourLazySingleton<LocalSaveManager>
    {
        public const string STATIC_FILE = "staticdata";
        public const string LOCAL_FILE = "localsave";

        public bool Paused { get; set; } = false;

        void Awake()
        {
            StartCoroutine(SaveLoop());
        }

        IEnumerator SaveLoop()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);

                if (!Paused)
                {
                    Save();
                }

                App.PersistantLocalFile.WriteToFile();
            }
        }

        public void Save()
        {
            // = Models = //
            var savefile = App.CreateLocalStateFile();

            // = Write to File = //
            FileUtils.WriteModel(LOCAL_FILE, savefile);
        }

        public void WriteStaticData(IStaticGameData model) => FileUtils.WriteModel(STATIC_FILE, model);

        public void DeleteLocalFile() => FileUtils.DeleteFile(LOCAL_FILE);
    }
}
