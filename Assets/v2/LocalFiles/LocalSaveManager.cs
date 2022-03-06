using System.Collections;
using UnityEngine;
using GM.Common.Interfaces;

namespace GM
{
    public class LocalSaveManager : Common.MonoClass<LocalSaveManager>
    {
        const string STATIC_FILE = "staticdata";
        const string LOCAL_FILE = "localsave";

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
            var savefile = App.GMData.CreateLocalSaveFile();

            // = Write to File = //
            FileUtils.WriteModel(LOCAL_FILE, savefile);
        }

        public void WriteStaticData(IStaticGameData model) => FileUtils.WriteModel(STATIC_FILE, model);

        public void DeleteLocalFile() => FileUtils.DeleteFile(LOCAL_FILE);

        public LocalStateFile LoadSaveFile()
        {
            FileStatus status = FileUtils.LoadModel(LOCAL_FILE, out LocalStateFile model);

            return model;
        }
    }
}
